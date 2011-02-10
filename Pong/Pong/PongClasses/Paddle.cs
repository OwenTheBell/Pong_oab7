using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Pong.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Pong.PaddleControllers;
using Pong.Mathematics;
using System.IO;
using Pong.Clock;
using Pong.GameElements;
using System.Threading;

namespace Pong.PongClasses
{
    /// <summary>
    /// Reprents a paddle which can be controlled by an AI agent or a human.
    /// </summary>
    public class Paddle
    {
        /// <summary>
        /// The paddle width.
        /// </summary>
        public const float Width = 100;

        /// <summary>
        /// The paddle height.
        /// </summary>
        public const float Height = 400;

        /// <summary>
        /// The previous paddle positions.
        /// </summary>
        /// <value>The previous positions.</value>
        public List<PaddleState> PreviousStates { get; private set; }

        /// <summary>
        /// The target to go to.
        /// </summary>
        private float target;

        /// <summary>
        /// The position of the paddle.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The current game of pong.
        /// </summary>
        private PongWorld pongWorld;

        /// <summary>
        /// Is this the left paddle? Else, it is the right paddle.
        /// </summary>
        private bool isLeftPaddle;

        /// <summary>
        /// The drawing origin.
        /// </summary>
        private Vector2 drawOrigin;

        /// <summary>
        /// The controller that is controlling the paddle, whether it is
        /// human based or AI.
        /// </summary>
        private IPaddleController controller;

        /// <summary>
        /// The total number of positive responces.
        /// </summary>
        private float totalPositiveResponces;

        /// <summary>
        /// How many times has the ball hit this paddle's side?
        /// </summary>
        private float totalResponces;

        /// <summary>
        /// Initializes a new instance of the <see cref="Paddle"/> class.
        /// </summary>
        /// <param name="pongWorld">The pong world.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="isLeft">if set to <c>true</c> [is left].</param>
        public Paddle(IPaddleController controller, PongWorld pongWorld, Vector2 initialPosition, bool isLeft)
        {
            this.totalPositiveResponces = 0;
            this.totalResponces = 0;
            this.isLeftPaddle = isLeft;
            this.position = initialPosition;
            this.target = initialPosition.Y;
            this.pongWorld = pongWorld;
            this.drawOrigin = new Vector2(Width / 2f, Height / 2f);
            this.controller = controller;
            this.controller.paddle = this;
            this.PreviousStates = new List<PaddleState>();
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            this.controller.Update();

            if (this.position.Y > this.target)
            {
                if (this.position.Y - this.target < Settings.MaxPaddleSpeed)
                {
                    this.MovePaddleUp(this.position.Y - this.target);
                }
                else
                {
                    this.MovePaddleUp(Settings.MaxPaddleSpeed);
                }
            }
            else if (this.position.Y < this.target)
            {
                if (this.target - this.position.Y < Settings.MaxPaddleSpeed)
                {
                    this.MovePaddleDown(this.target - this.position.Y);
                }
                else
                {
                    this.MovePaddleDown(Settings.MaxPaddleSpeed);
                }
            }

            // Make sure the paddle is not out of bounds.
            if (this.position.Y - Height / 2f < this.pongWorld.PlayingField.Top)
            {
                this.position.Y = this.pongWorld.PlayingField.Top + Height / 2f;
            }
            if (this.position.Y + Height / 2f > this.pongWorld.PlayingField.Bottom)
            {
                this.position.Y = this.pongWorld.PlayingField.Bottom - Height / 2f;
            }
        }

        /// <summary>
        /// Gives the paddle "positive feedback", indicating that it hit the ball.
        /// </summary>
        /// <param name="ballAngle">The ball angle.</param>
        /// <param name="ballPosition">The ball position.</param>
        public void PositiveFeedback(float ballAngle, float ballPosition)
        {
            this.totalPositiveResponces++;
            this.PreviousStates.Add(new PaddleState(true, this.position.Y, ballAngle, ballPosition));

            // Play the positive hit sound.
            GameWorld.audio.PlaySound(this.isLeftPaddle ? "beep1" : "beep2");
        }

        /// <summary>
        /// Gives the paddle "negative feedback", indicating that it missed the ball.
        /// </summary>
        /// <param name="ballAngle">The ball angle.</param>
        /// <param name="ballPosition">The ball position.</param>
        public void NegativeFeedback(float ballAngle, float ballPosition)
        {
            this.PreviousStates.Add(new PaddleState(false, this.position.Y, ballAngle, ballPosition));
            GameWorld.audio.PlaySound(this.isLeftPaddle ? "beep1" : "beep2", 1.0f, this.isLeftPaddle ? -1.0f : 1.0f, 0.0f);
        }

        /// <summary>
        /// Gets the paddle that is not this paddle.
        /// </summary>
        /// <returns>The paddle that is not this paddle.</returns>
        public Paddle GetOtherPaddle()
        {
            if (this.isLeftPaddle)
            {
                return this.pongWorld.PaddleRight;
            }

            return this.pongWorld.PaddleLeft;
        }

        /// <summary>
        /// Gets the position of the paddle.
        /// </summary>
        /// <returns>The position of the paddle.</returns>
        public float GetPosition()
        {
            return this.position.Y;
        }

        /// <summary>
        /// Goes to the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        public void GoTo(float target)
        {
            this.target = target;
        }

        /// <summary>
        /// Moves the paddle up.
        /// </summary>
        /// <param name="amount">The amount.</param>
        private void MovePaddleUp(float amount)
        {
            this.position.Y -= amount;
        }

        /// <summary>
        /// Moves the paddle down.
        /// </summary>
        /// <param name="amount">The amount.</param>
        private void MovePaddleDown(float amount)
        {
            this.position.Y += amount;
        }

        /// <summary>
        /// Gets the percentage of times that the paddle
        /// has successfully struck the ball.
        /// </summary>
        /// <returns>The percentage of times that the paddle
        /// has successfully struck the ball.</returns>
        public float GetAccuracy()
        {
            if (this.totalResponces == 0)
            {
                return 0;
            }

            return this.totalPositiveResponces / this.totalResponces;
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            Drawer.Draw(
                TextureStatic.Get(this.isLeftPaddle ? "paddleA" : "paddleB"),
                this.position,
                null,
                Color.White,
                0f,
                this.drawOrigin,
                1f,
                SpriteEffects.None,
                0.9f);

            // Draw the score.
            Drawer.DrawString(
                this.totalPositiveResponces.ToString(),
                new Vector2(this.isLeftPaddle ? 270 : 1920 - 270, 1080 - 100),
                Color.Black,
                0f,
                Drawer.font.MeasureString(this.totalPositiveResponces.ToString()) / 2f,
                0.2f,
                SpriteEffects.None,
                0.89f);
        }
    }
}
