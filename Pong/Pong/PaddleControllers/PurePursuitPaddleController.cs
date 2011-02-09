using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pong.PongClasses;

namespace Pong.PaddleControllers
{
    /// <summary>
    /// This implements "pure pursuit". In other words, the paddle tries to 
    /// be always aligned with the ball.
    /// </summary>
    public class PurePursuitPaddleController : IPaddleController
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
        /// Initializes a new instance of the <see cref="PurePursuitPaddleController"/> class.
        /// </summary>
        /// <param name="pongWorld">The pong world.</param>
        /// <param name="paddle">The paddle.</param>
        public PurePursuitPaddleController(PongWorld pongWorld, Paddle paddle)
        {
            this.paddle = paddle;
            this.pongWorld = pongWorld;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            // Just go to the ball's position....always.
            this.paddle.GoTo(this.pongWorld.ball.position.Y);
        }
    }
}
