using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pong.PaddleControllers;
using System.IO;

namespace Pong.GameElements
{
    /// <summary>
    /// Used for setting global game settings.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Gets a value indicating whether [the paddles should be rounded].
        /// </summary>
        /// <value><c>true</c> if [use rounded paddles]; otherwise, <c>false</c>.</value>
        public static bool UseRoundedPaddles { get { return true; } }

        /// <summary>
        /// Gets the ball speed.
        /// </summary>
        /// <value>The ball speed.</value>
        public static float BallSpeed { get { return 40; } }

        /// <summary>
        /// Gets the max paddle speed.
        /// </summary>
        /// <value>The max paddle speed.</value>
        public static float MaxPaddleSpeed { get { return 5; } }

        /// <summary>
        /// Gets the left paddle AI.
        /// </summary>
        /// <value>The left paddle AI.</value>
        public static AIType LeftPaddleAI { get { return AIType.LeadPursuit; } }

        /// <summary>
        /// Gets the right paddle AI.
        /// </summary>
        /// <value>The right paddle AI.</value>
        public static AIType RightPaddleAI { get { return AIType.Human; } }        
    }
}
