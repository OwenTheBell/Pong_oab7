using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pong.GameElements;
using Microsoft.Xna.Framework;
using Pong.Menus;
using Pong.Clock;
using Pong.Menus.MenuDelegates;
using Pong.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Pong.Inputs;

namespace Pong.Screens
{
    public class MainScreen : GameScreen
    {
        private long MainTime;

        private MainMenu menu;

        private Vector2 textDrawPosition;

        private Vector2 textDrawOrigin;

        public MainScreen()
            : base()
        {
            this.MainTime = DateTime.Now.Ticks;
            this.menu = new MainMenu(
                new Vector2(1000,330),
                new MenuAction[]
                {
                    new MenuAction(ActionType.GoBack, new PlayGameDelegate()),
                },
                75
            );

            this.textDrawPosition = new Vector2(1000, 125);
            this.textDrawOrigin = Drawer.font.MeasureString("Menu") / 2f;
        }

        public override void Update()
        {
            base.Update();
            this.menu.Update();
        }

        public override void Draw()
        {
            Drawer.Draw(
                TextureStatic.Get("background"),
                Drawer.FullScreenRectangle,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0f
                );

            Drawer.DrawString(
                "Main Menu",
                this.textDrawPosition,
                Color.Black,
                0f,
                this.textDrawOrigin,
                0.35f,
                SpriteEffects.None,
                1f);

            this.menu.Draw();
        }
    }
}
