using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pong.PongClasses;

namespace Pong.PaddleControllers
{
    /// <summary>
    /// The interface for controlling a paddle.
    /// </summary>
    public interface IPaddleController
    {
        /// <summary>
        /// Updates this instance.
        /// </summary>
        void Update();

        /// <summary>
        /// Gets or sets the paddle.
        /// </summary>
        /// <value>The paddle.</value>
        Paddle paddle { get; set; }

        /// <summary>
        /// Gets or sets the pong world.
        /// </summary>
        /// <value>The pong world.</value>
        PongWorld pongWorld { get; set; }
    }
}
