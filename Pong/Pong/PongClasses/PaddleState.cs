using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong.PongClasses
{
    /// <summary>
    /// Used for saving information about the paddle at the
    /// time that the ball reaches its side of the court.
    /// </summary>
    public class PaddleState
    {
        /// <summary>
        /// Gets the position of the paddle at this state..
        /// </summary>
        /// <value>The position of the paddle at this state.</value>
        public float paddlePosition { get; private set; }

        /// <summary>
        /// Gets the ball angle at this state.
        /// </summary>
        /// <value>The ball angle at this state.</value>
        public float ballAngle { get; private set; }

        /// <summary>
        /// Gets the ball position.
        /// </summary>
        /// <value>The ball position.</value>
        public float ballPosition { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="PaddleState"/> successfully hit the ball.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaddleState"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="position">The position.</param>
        /// <param name="ballAngle">The ball angle.</param>
        public PaddleState(bool success, float position, float ballAngle, float ballPosition)
        {
            this.Success = success;
            this.paddlePosition = position;
            this.ballAngle = ballAngle;
            this.ballPosition = ballPosition;
        }
    }
}