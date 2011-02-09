using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pong.PongClasses;
using Microsoft.Xna.Framework;

namespace Pong.PaddleControllers
{
    /// <summary>
    /// This is used for implemented the lead pursuit AI,
    /// which attempts to figure out where the ball will hit and
    /// go there, rather than just following the ball like pure pursuit.
    /// </summary>
    public class LeadPursuitPaddleController : IPaddleController
    {
        /// <summary>
        /// Gets or sets the paddle.
        /// </summary>
        /// <value>The paddle.</value>
        public Paddle paddle { get; set; }

        /// <summary>
        /// Gets or sets the pong world.
        /// </summary>
        /// <value>The pong world.</value>
        public PongWorld pongWorld { get; set; }

        /// <summary>
        /// The previous number of examples for the other paddle.
        /// </summary>
        private int previousOpponentExampleCount;

        /// <summary>
        /// The previous number of examples.
        /// </summary>
        private int previousExampleCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="LeadPursuitPaddleController"/> class.
        /// </summary>
        /// <param name="pongWorld">The pong world.</param>
        /// <param name="paddle">The paddle.</param>
        public LeadPursuitPaddleController(PongWorld pongWorld, Paddle paddle)
        {
            this.paddle = paddle;
            this.pongWorld = pongWorld;
            this.previousExampleCount = 0;
            this.previousOpponentExampleCount = 0;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            this.TryDefend();
            this.PrePredict();
        }

        /// <summary>
        /// The method is used for pre-predicting the location of the ball imediately after it is hit.
        /// </summary>
        private void PrePredict()
        {
            // If there is nothing to do.
            if (this.previousExampleCount == this.paddle.PreviousStates.Count ||
                this.paddle.PreviousStates.Count < 1 ||
                this.paddle.GetOtherPaddle().PreviousStates.Count < 1)
            {
                return;
            }

            // Based on the current position of the ball and the angle of the ball, 
            // where will the ball hit the wall?
            float intersect = this.GetIntersect(
                this.paddle.PreviousStates[this.paddle.PreviousStates.Count - 1].ballAngle,
                this.paddle.PreviousStates[this.paddle.PreviousStates.Count - 1].ballPosition,
                2 * (this.pongWorld.PlayingField.Width - 2 * Ball.Radius),
                this.pongWorld.PlayingField.Top + Ball.Radius,
                this.pongWorld.PlayingField.Bottom - Ball.Radius);

            // Intersect is known, so go there
            this.paddle.GoTo(intersect);

            this.previousExampleCount = this.paddle.PreviousStates.Count;
        }

        /// <summary>
        /// If the opponent has just hit the ball, this moves the paddle to the spot
        /// where the ball will hit.
        /// </summary>
        private void TryDefend()
        {
            if (this.previousOpponentExampleCount == this.paddle.GetOtherPaddle().PreviousStates.Count ||
                this.paddle.PreviousStates.Count < 1 ||
                this.paddle.GetOtherPaddle().PreviousStates.Count < 1)
            {
                return;
            }

            // Based on the current position of the ball and the angle of the ball, 
            // where will the ball hit the wall?
            float intersect = this.GetIntersect(
                this.paddle.GetOtherPaddle().PreviousStates[this.paddle.GetOtherPaddle().PreviousStates.Count - 1].ballAngle,
                this.paddle.GetOtherPaddle().PreviousStates[this.paddle.GetOtherPaddle().PreviousStates.Count - 1].ballPosition,
                this.pongWorld.PlayingField.Width - 2 * Ball.Radius,
                this.pongWorld.PlayingField.Top + Ball.Radius,
                this.pongWorld.PlayingField.Bottom - Ball.Radius);

            // Intersect is known, so go there
            this.paddle.GoTo(intersect);

            this.previousOpponentExampleCount = this.paddle.GetOtherPaddle().PreviousStates.Count;
        }

        /// <summary>
        /// Gets the intersection point of the opposite wall of where the ball is lauched.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="position">The position.</param>
        /// <param name="width">The width.</param>
        /// <param name="top">The top.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns>The intersection point of the opposite wall of where the ball is lauched.</returns>
        private float GetIntersect(float angle, float position, float width, float top, float bottom)
        {
            float intersect = position - (float)Math.Tan(angle - MathHelper.PiOver2) * width;

            // Now that the intersect is known when there is no bouncing, simply "bound" the intersect into position!
            while (intersect < top || intersect > bottom)
            {
                // If the intersection happens above the top part, flip it by the amount it goes over.
                if (intersect < top)
                {
                    intersect = 2 * top - intersect;
                }

                // If the intersection happens below the bottom part, flip it by the amount it goes under.
                if (intersect > bottom)
                {
                    intersect = 2 * bottom - intersect;
                }
            }

            return intersect;
        }
    }
}
