using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pong.GameElements;

namespace Pong.Menus.MenuDelegates
{
    /// <summary>
    /// A delegate used for exiting the game.
    /// </summary>
    public class QuitGameDeleage : IMenuDelegate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuitGameDeleage"/> class.
        /// </summary>
        public QuitGameDeleage()
        {
        }

        /// <summary>
        /// Runs this instance, performing the action once,
        /// and thereafter exiting the method.
        /// </summary>
        public void Run()
        {
            GameWorld.ExitGame();
        }
    }
}
