using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pong.GameElements;

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
            GameWorld.screens.Play(new GameScreen());
        }
    }
}
