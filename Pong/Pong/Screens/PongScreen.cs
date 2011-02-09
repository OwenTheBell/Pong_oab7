using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pong.GameElements;
using Pong.PongClasses;
using Microsoft.Xna.Framework;

namespace Pong.Screens
{
    /// <summary>
    /// Used for displaying the state of the game.
    /// </summary>
    public class PongScreen : GameScreen
    {
        /// <summary>
        /// The pong world.
        /// </summary>
        private PongWorld pongWorld;

        /// <summary>
        /// Initializes a new instance of the <see cref="PongScreen"/> class.
        /// </summary>
        public PongScreen()
            : base()
        {
            int leftBuffer = 200;
            int topBuffer = 40;
            this.pongWorld = new PongWorld(
                new Rectangle(leftBuffer, topBuffer, 1920 - leftBuffer * 2, 1080 - topBuffer * 2), 
                Settings.BallSpeed,
                Settings.UseRoundedPaddles,
                Settings.LeftPaddleAI, 
                Settings.RightPaddleAI);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            base.Update();

            this.pongWorld.Update();
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            this.pongWorld.Draw();
        }
    }
}
