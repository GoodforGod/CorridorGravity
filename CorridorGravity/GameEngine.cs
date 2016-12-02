using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CorridorGravity.Services;

namespace CorridorGravity
{
    public class GameEngine : Game
    {
        ScreenManager           SManager;
        ServiceLocator          SLocator;

        GraphicsDeviceManager   GraphDevManager;
        SpriteBatch             Batcher;

        /// <summary>
        /// 
        /// </summary>
        public bool NeedToExit { get; set; }

        public static readonly int FWidth = 1024;
        public static readonly int FHeight = 768;

        public GameEngine()
        {
            Content.RootDirectory = "Content";

            GraphDevManager = new GraphicsDeviceManager(this);
            GraphDevManager.PreferredBackBufferWidth = FWidth;
            GraphDevManager.PreferredBackBufferHeight = FHeight;
            GraphDevManager.ApplyChanges();
            this.Window.Position = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width/2 - FWidth/2, 
                                            GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height/2 - FHeight/2);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //IsMouseVisible = true; 
            SLocator = ServiceLocator.Instance;
            SLocator.Initialize(Content);

            SManager = ScreenManager.Instance;
            SManager.Initialize(GraphicsDevice, this);

            base.Initialize();
        }

        // Find new random algorithm, part of it
        protected static uint BitRotate(uint x)
        {
            const int bits = 16;
            return (x << bits) | (x >> (32 - bits));
        }

        // Find new random algorithm, main algo
        public static uint GenerateRandomCoordinate(long x, long y)
        {
            uint num = (uint)DateTime.Now.Millisecond;
            for (uint i = 0; i < 16; i++)
            {
                num = num * 541 + (uint)x;
                num = BitRotate(num);
                num = num * 809 + (uint)y;
                num = BitRotate(num);
                num = num * 673 + (uint)i;
                num = BitRotate(num);
            }
            return num % 4;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            // Create a new SpriteBatch, which can be used to draw textures.
            Batcher = new SpriteBatch(GraphicsDevice);

            SManager.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            SLocator.AManager.Stop();
            SLocator.UnloadContent();
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                    || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                UnloadContent();
                Exit();
            }

            SManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SManager.Draw(Batcher, gameTime);

            base.Draw(gameTime);
        }
    }
}
