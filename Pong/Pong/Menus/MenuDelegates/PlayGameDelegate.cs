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
            GameWorld.screens.KillAll();
            GameWorld.screens.Play(new PongScreen());
        }
    }
}
