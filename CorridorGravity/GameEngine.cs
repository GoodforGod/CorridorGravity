using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorridorGravity.GameLogic;

namespace CorridorGravity
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameEngine : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private const int FRAME_WIDTH = 1024;
        private const int FRAME_HEIGHT = 768;

        private const string ENTITY_TYPE = "player-2-white-1";

        EnemyEntity Enemy;
        PlayerEntity FirstCharacter;
        WorldEntity World;

        public GameEngine()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = FRAME_WIDTH;
            graphics.PreferredBackBufferHeight = FRAME_HEIGHT;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
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
            this.IsMouseVisible = true;

            World = new WorldEntity(Content, "world-background", FRAME_WIDTH, FRAME_HEIGHT);
            World.Init();
            
            FirstCharacter = new PlayerEntity(Content, ENTITY_TYPE, World.WORLD_HEIGHT, World.WORLD_WIDTH);
            FirstCharacter.Init();

            Enemy = new EnemyEntity(Content, ENTITY_TYPE, World.WORLD_HEIGHT, World.WORLD_WIDTH);
            Enemy.Init();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here

            // Characters Updates
            FirstCharacter.Update(gameTime);
            Enemy.SetLevelDimention(FirstCharacter.GetLevelDimention());
            Enemy.SetLevelDirection(FirstCharacter.GetLevelDirection());
            Enemy.Update(gameTime);
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Azure);
            spriteBatch.Begin();
            // TODO: Add your drawing code here

            // Background
            World.Draw(spriteBatch);

            //Characters
            FirstCharacter.Draw(spriteBatch);

            //
            Enemy.Draw(spriteBatch);

            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
