using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Pong.Menus.MenuDelegates;
using Pong.GameElements;
using Pong.Inputs;

namespace Pong.Menus
{
    class MainMenu : Menu
    {
        public MainMenu(Vector2 position, MenuAction[] actions, float spacing)
            : base(position, actions)
        {
            MenuEntry play = new MenuEntry(
                "Play",
                new MenuAction[] 
                { 
                    new MenuAction(ActionType.Select, new PlayGameDelegate()) 
                },
                position);

            MenuEntry quit = new MenuEntry(
                "Quit",
                new MenuAction[] 
                { 
                    new MenuAction(ActionType.Select, new QuitGameDeleage())
                },
                position + new Vector2(0, spacing));

            play.UpperMenu = quit;
            play.LowerMenu = quit;

            quit.UpperMenu = play;
            quit.LowerMenu = play;

            this.Add(play);
            this.Add(quit);
        }
    }
}
