using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pong.GameElements;
using Pong.Screens;

namespace Pong.Menus.MenuDelegates
{
    public class PlayGameDelegate : IMenuDelegate
    {

        public PlayGameDelegate()
            : base()
        {
        }

        public void Run()
        {
            if (GameWorld.screens.Count > 0)
            {
                GameWorld.screens[GameWorld.screens.Count - 1].Disposed = true;
            }
            GameWorld.screens.Play(new PongScreen());
        }
    }
}
