using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Pong.Rendering;
using Pong.PaddleControllers;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.PongClasses
{
    /// <summary>
    /// The object containing all data about the game of Pong that is being played..
    /// </summary>
    public class PongWorld
    {
        /// <summary>
        /// Get the left paddle.
        /// </summary>
        /// <value>The left paddle.</value>
        public Paddle PaddleLeft { get; private set; }

        /// <summary>
        /// Gets the right paddle.
        /// </summary>
        /// <value>The right paddle.</value>
        public Paddle PaddleRight { get; private set; }

        /// <summary>
        /// Gets the ball.
        /// </summary>
        /// <value>The ball.</value>
        public Ball ball { get; private set; }

        /// <summary>
        /// Gets the playing field.
        /// </summary>
        /// <value>The playing field.</value>
        public Rectangle PlayingField { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use rounded paddles].
        /// </summary>
        /// <value><c>true</c> if [use rounded paddles]; otherwise, <c>false</c>.</value>
        public bool UseRoundedPaddles { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PongWorld"/> class.
        /// </summary>
        /// <param name="playingField">The playing field.</param>
        /// <param name="initialBallVelocity">The initial ball velocity.</param>
        /// <param name="useRoundedPaddles">if set to <c>true</c> [use rounded paddles].</param>
        public PongWorld(Rectangle playingField, float initialBallVelocity, bool useRoundedPaddles, AIType leftPaddleAI, AIType rightPaddleAI)
        {
            this.PlayingField = playingField;
            this.UseRoundedPaddles = useRoundedPaddles;
            this.PaddleLeft = new Paddle(
                PaddleControllerFactory.Create(this, leftPaddleAI, this.PaddleLeft),
                this, 
                new Vector2(playingField.Left - 50, playingField.Center.Y), 
                true);
            this.PaddleRight = new Paddle(
                PaddleControllerFactory.Create(this, rightPaddleAI, this.PaddleRight),
                this, 
                new Vector2(playingField.Right + 50, playingField.Center.Y), 
                false);
            
            Random temp = new Random();
            Vector2 ballVelocity = new Vector2(
                (float)((temp.Next(2) == 0 ? 1 : -1) * (temp.NextDouble() * 0.9 + 0.1)),
                (float)((temp.Next(2) == 0 ? 1 : -1) * (temp.NextDouble() * 0.9 + 0.1)));
            ballVelocity.Normalize();
            ballVelocity *= initialBallVelocity;
            this.ball = new Ball(this, new Vector2(playingField.Center.X, playingField.Center.Y), ballVelocity);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            this.ball.Update();
            this.PaddleLeft.Update();
            this.PaddleRight.Update();
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            // Draw the background
            Drawer.Draw(
                TextureStatic.Get("background"),
                Drawer.FullScreenRectangle,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0f);

            this.ball.Draw();
            this.PaddleLeft.Draw();
            this.PaddleRight.Draw();

            // Draw the playing field rectangle.
            Drawer.DrawRectangle(this.PlayingField);
        }

        /// <summary>
        /// Given the ball angle, this returns the value scaled between 0 and 1.
        /// </summary>
        /// <param name="ballAngle">The ball angle.</param>
        /// <returns>The value of the specified ball angle scaled between 0 and 1.</returns>
        public float GetScaledBallAngle(float ballAngle)
        {
            // Find the minimum it could be. Same with the max.
            float min = 0;
            float max = MathHelper.Pi;

            // If the specified angle is less than min or more than max, return 0 or 1, respecitively.
            if (ballAngle < min)
            {
                return 0;
            }

            if (ballAngle > max)
            {
                return 1;
            }

            // Otherwise, scale the value to be between 0 and 1.
            return (ballAngle - min) / (max - min);
        }

        /// <summary>
        /// Given the ball position, this returns the value scaled between 0 and 1.
        /// </summary>
        /// <param name="ballPosition">The ball position.</param>
        /// <returns>The value of the specified ball position scaled between 0 and 1.</returns>
        public float GetScaledBallPosition(float ballPosition)
        {
            // Find the minimum it could be. Same with the max.
            float min = this.PlayingField.Top + Ball.Radius;
            float max = this.PlayingField.Bottom - Ball.Radius;

            // If the specified position is less than min or more than max, return 0 or 1, respecitively.
            if (ballPosition < min)
            {
                return 0;
            }

            if (ballPosition > max)
            {
                return 1;
            }

            // Failsafe to make sure max != min
            if (min == max)
            {
                return 0;
            }

            // Otherwise, scale the value to be between 0 and 1.
            return (ballPosition - min) / (max - min);
        }

        /// <summary>
        /// Gets the un-scaled ball position from the scaled ball position.
        /// Note that this does the opposite of GetScaledBallPosition.
        /// </summary>
        /// <param name="scaledBallPosition">The scaled ball position.</param>
        /// <returns>The un-scaled ball position from the scaled ball position.</returns>
        public float GetUnScaledBallPosition(float scaledBallPosition)
        {
            // Find the minimum it could be. Same with the max.
            float min = this.PlayingField.Top + Ball.Radius;
            float max = this.PlayingField.Bottom - Ball.Radius;

            // Failsafe checks.
            if (scaledBallPosition < 0)
            {
                return min;
            }

            if (scaledBallPosition > 1)
            {
                return max;
            }

            return (max - min) * scaledBallPosition + min;
        }

        /// <summary>
        /// Given the paddle position, this returns the value scaled between 0 and 1.
        /// </summary>
        /// <param name="paddlePosition">The paddle position.</param>
        /// <returns>The value of the specified paddle position scaled between 0 and 1.</returns>
        public float GetScaledPaddlePosition(float paddlePosition)
        {
            // Find the minimum it could be. Same with the max.
            float min = this.PlayingField.Top + Paddle.Height / 2f;
            float max = this.PlayingField.Bottom - Paddle.Height / 2f;

            // If the specified position is less than min or more than max, return 0 or 1, respecitively.
            if (paddlePosition < min)
            {
                return 0;
            }

            if (paddlePosition > max)
            {
                return 1;
            }

            // Failsafe to make sure max != min
            if (min == max)
            {
                return 0;
            }

            // Otherwise, scale the value to be between 0 and 1.
            return (paddlePosition - min) / (max - min);
        }

        /// <summary>
        /// Gets the un-scaled paddle position from the scaled paddle position.
        /// Note that this does the opposite of GetScaledPaddlePosition.
        /// </summary>
        /// <param name="scaledPaddlePosition">The scaled paddle position.</param>
        /// <returns>The un-scaled paddle position from the scaled paddle position.</returns>
        public float GetUnScaledPaddlePosition(float scaledPaddlePosition)
        {
            // Find the minimum it could be. Same with the max.
            float min = this.PlayingField.Top + Paddle.Height / 2f;
            float max = this.PlayingField.Bottom - Paddle.Height / 2f;

            // Failsafe checks.
            if (scaledPaddlePosition < 0)
            {
                return min;
            }

            if (scaledPaddlePosition > 1)
            {
                return max;
            }

            return (max - min) * scaledPaddlePosition + min;
        }
    }
}
