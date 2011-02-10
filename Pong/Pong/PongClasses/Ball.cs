using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Pong.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Pong.Mathematics;

namespace Pong.PongClasses
{
    /// <summary>
    /// Represents the ball in the game of pong.
    /// </summary>
    public class Ball
    {
        /// <summary>
        /// The radius of the ball.
        /// </summary>
        public const float Radius = 75;

        /// <summary>
        /// Gets the position of the ball.
        /// </summary>
        public Vector2 position;

        /// <summary>
        /// Gets the velocity of the ball.
        /// </summary>
        public Vector2 velocity;

        /// <summary>
        /// The current game of pong.
        /// </summary>
        private PongWorld pongWorld;

        /// <summary>
        /// The origin for which to draw the ball.
        /// </summary>
        private Vector2 drawOrigin;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ball"/> class.
        /// </summary>
        /// <param name="pongWorld">The pong world.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="intialVelocity">The intial velocity.</param>
        public Ball(PongWorld pongWorld, Vector2 initialPosition, Vector2 intialVelocity)
        {
            this.pongWorld = pongWorld;
            this.position = initialPosition;
            this.velocity = intialVelocity;
            this.drawOrigin = new Vector2(Radius);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            // Failsafe
            if (this.velocity.Y == 0)
            {
                this.velocity.Y = 0.01f;
            }
            if (-1 < this.velocity.X && this.velocity.X < 1)
            {
                if (0 < this.velocity.X)
                {
                    this.velocity.X = 1f;
                }
                if (this.velocity.X < 0)
                {
                    this.velocity.X = -1f;
                }
            }


            //Limit the maximum speed of the ball
            if (this.velocity.Y > 40) this.velocity.Y = 40;
            else if (this.velocity.Y < -40) this.velocity.Y = 40;
            if (this.velocity.X > 40) this.velocity.X = 40;
            else if (this.velocity.X < -40) this.velocity.X = -40;
            
            this.position += this.velocity;

            // Has it collided with the left paddle?
            if (this.position.X - Radius < this.pongWorld.PlayingField.Left)
            {
                // First set the position to be correct (i.e., on the wall).
                Vector2 oldPosition = position;
                this.position.Y += ((this.position.X - Radius) - this.pongWorld.PlayingField.Left)
                    * this.velocity.Y / this.velocity.X;
                this.position.X = this.pongWorld.PlayingField.Left + Radius;
                this.velocity.X *= -1;

                if (Math.Abs(this.pongWorld.PaddleLeft.GetPosition() - this.position.Y) <= Paddle.Height / 2f)
                {
                    float ballPaddleRelation = (this.position.Y - this.pongWorld.PaddleLeft.GetPosition()) / (Paddle.Height / 2f);
                    // Rotate the velocity if applicable.
                    if (this.pongWorld.UseRoundedPaddles)
                    {
                        float magnitude = velocity.Length();
                        this.velocity = magnitude * RotationHelper.AngleToVector2(
                            MathHelper.Pi +
                            0.4f * ballPaddleRelation);
                        //vary the speed imparted to the ball based on where on the paddle it hits
                        this.velocity.Y += 5 * this.pongWorld.PaddleLeft.GetYSpeed() * (1 + Math.Abs(ballPaddleRelation));
                    }

                    this.pongWorld.PaddleLeft.PositiveFeedback(RotationHelper.Vector2ToAngle(
                        this.velocity.X, this.velocity.Y), this.position.Y);
                }
                else
                {
                    this.pongWorld.PaddleLeft.NegativeFeedback(RotationHelper.Vector2ToAngle(
                        this.velocity.X, this.velocity.Y), this.position.Y);
                }

                // Now apply the negation of the velocity.
                // TODO: Make this curved on the paddle? Or just always an even bounce?
                this.position.Y = oldPosition.Y;
                this.position.X += (this.position.X - oldPosition.X);
            }

            // Has it collided with the right paddle?
            if (this.position.X + Radius > this.pongWorld.PlayingField.Right)
            {
                // First set the position to be correct (i.e., on the wall).
                Vector2 oldPosition = position;
                this.position.Y += ((this.position.X + Radius) - this.pongWorld.PlayingField.Right)
                    * this.velocity.Y / this.velocity.X;
                this.position.X = this.pongWorld.PlayingField.Right - Radius;
                this.velocity.X *= -1;

                if (Math.Abs(this.pongWorld.PaddleRight.GetPosition() - this.position.Y) <= Paddle.Height / 2f)
                {
                    float ballPaddleRelation = (this.position.Y - this.pongWorld.PaddleRight.GetPosition()) / (Paddle.Height / 2f);
                    // Rotate the velocity if applicable.
                    if (this.pongWorld.UseRoundedPaddles)
                    {
                        float magnitude = velocity.Length();
                        this.velocity = magnitude * RotationHelper.AngleToVector2(
                            MathHelper.TwoPi -
                            0.4f * ballPaddleRelation);
                        //vary the speed imparted to the ball based on where on the paddle it hits
                        this.velocity.Y += this.pongWorld.PaddleRight.GetYSpeed() * (1 + Math.Abs(ballPaddleRelation));
                    }

                    this.pongWorld.PaddleRight.PositiveFeedback(RotationHelper.Vector2ToAngle(
                        this.velocity.X, this.velocity.Y), this.position.Y);
                }
                else
                {
                    this.pongWorld.PaddleRight.NegativeFeedback(RotationHelper.Vector2ToAngle(
                        this.velocity.X, this.velocity.Y), this.position.Y);
                }

                // Now apply the negation of the velocity.
                // TODO: Make this curved on the paddle? Or just always an even bounce?
                this.position.Y = oldPosition.Y;
                this.position.X -= (oldPosition.X - this.position.X);
            }

            // Bounce off of top
            if (this.position.Y - Radius < this.pongWorld.PlayingField.Top)
            {
                // First set the position to be correct (i.e., on the wall).
                Vector2 oldPosition = position;
                this.position.X += ((this.position.Y - Radius) - this.pongWorld.PlayingField.Top)
                    * this.velocity.X / this.velocity.Y;
                this.position.Y = this.pongWorld.PlayingField.Top + Radius;

                // Now apply the negation of the velocity.
                this.position.X = oldPosition.X;
                this.position.Y += (this.position.Y - oldPosition.Y);
                this.velocity.Y *= -1;
            }

            // Bounce off bottom
            if (this.position.Y + Radius > this.pongWorld.PlayingField.Bottom)
            {
                // First set the position to be correct (i.e., on the wall).
                Vector2 oldPosition = position;
                this.position.X += ((this.position.Y + Radius) - this.pongWorld.PlayingField.Bottom)
                    * this.velocity.X / this.velocity.Y;
                this.position.Y = this.pongWorld.PlayingField.Bottom - Radius;

                // Now apply the negation of the velocity.
                this.position.X = oldPosition.X;
                this.position.Y += (this.position.Y - oldPosition.Y);
                this.velocity.Y *= -1;
            }
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            Drawer.Draw(
                TextureStatic.Get("ball"),
                this.position,
                null,
                Color.White,
                0f,
                this.drawOrigin,
                1f,
                SpriteEffects.None,
                0.9f);
        }
    }
}
