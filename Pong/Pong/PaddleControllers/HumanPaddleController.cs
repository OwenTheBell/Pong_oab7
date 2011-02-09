using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pong.PongClasses;
using Pong.GameElements;
using Pong.Inputs;

namespace Pong.PaddleControllers
{
    /// <summary>
    /// Used to let a human control the paddle.
    /// </summary>
    public class HumanPaddleController : IPaddleController
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
        /// Initializes a new instance of the <see cref="HumanPaddleController"/> class.
        /// </summary>
        public HumanPaddleController(PongWorld pongWorld, Paddle paddle)
        {
            this.pongWorld = pongWorld;
            this.paddle = paddle;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            // Detect if up or down is pressed on the keyboard/controller.
            // If so, move the paddle accordingly. Note that ...ContainsFloat
            // for MoveVertical will return 0 if the joystick is in the rest position
            // or up or down is now
            this.paddle.GoTo(this.paddle.GetPosition() - 100 * GameWorld.controller.ContainsFloat(ActionType.MoveVertical));
        }
    }
}
