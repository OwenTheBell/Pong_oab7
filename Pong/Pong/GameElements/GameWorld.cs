using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Pong.Rendering;
using Pong.Clock;
using Pong.Mathematics;
using Pong.Screens;
using Pong.Inputs;
using Pong.Audio;

namespace Pong.GameElements
{
    /// <summary>
    /// This is the main public class for the game, which is the first thing initiallized,
    /// and it the ultimate controller of everything that is going on. Although this
    /// is originally made as an instanced object, all of the variables are static
    /// (which is okay, as there will only ever be 1 of these existing at a time).
    /// Thus, just say GameWorld.variable from anywhere to get easy access to that
    /// variable.
    /// </summary>
    public class GameWorld : Game
    {
        /// <summary>
        /// Gets the Graphics Device, used for getting information about the
        /// monitor or TV.
        /// </summary>
        /// <value>The graphics device.</value>
        public static GraphicsDevice graphics { get; private set; }

        /// <summary>
        /// Gets the sprite batch, which is used for rendering.
        /// </summary>
        /// <value>The sprite batch.</value>
        public static SpriteBatch spriteBatch { get; private set; }

        /// <summary>
        /// Gets the stack of screens used in the game.
        /// </summary>
        /// <value>The screens.</value>
        public static ScreenContainer screens { get; private set; }

        /// <summary>
        /// Gets the content manager, which handles all loading and unloading of content
        /// such as sprites, sound, and music in the game..
        /// </summary>
        /// <value>The content.</value>
        public static ContentManager content { get; private set; }

        /// <summary>
        /// Gets the controller for the player.
        /// </summary>
        /// <value>The controller.</value>
        public static Controller controller { get; private set; }

        /// <summary>
        /// Gets the audio. Used for loading and playing sounds and music.
        /// </summary>
        /// <value>The audio manager.</value>
        public static AudioManager audio { get; private set; }

        /// <summary>
        /// Indicates whether or not the game is exiting. This really exists
        /// so that we can ask demo players to buy the game before they exit.
        /// </summary>
        private static bool exitStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWorld"/> class.
        /// </summary>
        public GameWorld()
            : base()
        {
            GraphicsDeviceManager manager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            manager.PreferredBackBufferWidth
                = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.75f);
            manager.PreferredBackBufferHeight
                = (int)(0.5625f * manager.PreferredBackBufferWidth);
            controller = new Controller(PlayerIndex.One);
            screens = new ScreenContainer();
            exitStatus = false;
        }

        /// <summary>
        /// Called after the Game and GraphicsDevice are created, but before LoadContent.  Reference page contains code sample.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Loads the sprites and audio.
        /// </summary>
        protected override void LoadContent()
        {
            content = Content;
            graphics = GraphicsDevice;

            graphics.PresentationParameters.BackBufferWidth 
                = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PresentationParameters.BackBufferHeight 
                = (int)(0.5625f * GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);

#if !XBOX
            graphics.PresentationParameters.IsFullScreen = false;
#endif

            // Load the textures.
            TextureStatic.Load("blank", @"Art\blank");
            TextureStatic.Load("ball", @"Art\ball");
            TextureStatic.Load("paddleA", @"Art\paddleA");
            TextureStatic.Load("paddleB", @"Art\paddleB");
            TextureStatic.Load("background", @"Art\background");

            // Load the sounds and music
            audio = new AudioManager(this);
            audio.LoadSound("beep1", @"Sound\beep1");
            audio.LoadSound("beep2", @"Sound\beep2");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Drawer.Initiallize();

            //screens.Play(new HubWorld(HubWorldConfiguration.Intro));
            screens.Play(new MainScreen());
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        public static void ExitGame()
        {
            exitStatus = true;
        }

        /// <summary>
        /// Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Update.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (exitStatus)
            {
                this.Exit();
            }

            GameClock.Update();
            audio.Update(gameTime);
            controller.Update();
            screens.Update();
        }
        
        /// <summary>
        /// Reference page contains code sample.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            screens.Draw();

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
