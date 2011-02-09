using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pong.PongClasses;

namespace Pong.PaddleControllers
{
    /// <summary>
    /// Moves the paddle randomly.
    /// </summary>
    public class RandomPaddleController : IPaddleController
    {
        /// <summary>
        /// Gets or sets the pong world.
        /// </summary>
        /// <value>The pong world.</value>
        public PongWorld pongWorld { get; set; }

        /// <summary>
        /// Gets or sets the paddle.
        /// </summary>
        /// <value>The paddle.</value>
        public Paddle paddle { get; set; }

        /// <summary>
        /// The velocity of the paddle.
        /// </summary>
        private float paddleVelocity;

        /// <summary>
        /// The acceloration of the paddle.
        /// </summary>
        private float paddleAcceloration;

        /// <summary>
        /// The jerk (third position derivative) of the paddle.
        /// </summary>
        private float paddleJerk;

        /// <summary>
        /// The random number generator.
        /// </summary>
        private Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomPaddleController"/> class.
        /// </summary>
        public RandomPaddleController(PongWorld world, Paddle paddle)
        {
            this.pongWorld = world;
            this.paddle = paddle;
            this.paddleVelocity = 0;
            this.paddleAcceloration = 0;
            this.paddleJerk = 0;
            this.random = new Random();
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            this.paddle.GoTo(this.paddle.GetPosition() + this.paddleVelocity);
            this.paddleVelocity += this.paddleAcceloration;
            this.paddleAcceloration = this.paddleJerk;
            this.paddleJerk = random.Next(2) == 0 ? 1 : -1;

            if (this.paddle.GetPosition() >= this.pongWorld.PlayingField.Bottom - Paddle.Height / 2f)
            {
                this.paddle.GoTo(this.pongWorld.PlayingField.Bottom - Paddle.Height / 2f - 10);
                this.paddleAcceloration = 0;
                this.paddleVelocity = 0;
            }
            if (this.paddle.GetPosition() <= this.pongWorld.PlayingField.Top + Paddle.Height / 2f)
            {
                this.paddle.GoTo(this.pongWorld.PlayingField.Top + Paddle.Height / 2f + 10);
                this.paddleAcceloration = 0;
                this.paddleVelocity = 0;
            }
        }
    }
}
