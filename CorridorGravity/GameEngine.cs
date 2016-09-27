using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorridorGravity.GameLogic;
using System.Collections.Generic;

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
        private const int FRAME_SCORE_OFFSET = 40;
        private bool IsPaused { get; set; }
        private bool ButtonFlag { get; set; }

        private int SpawnTime = 16;

        private const string ENTITY_PLAYER = "player-2-white-1";
        private const string ENTITY_WORLD = "world-background-score";
        private const string ENTITY_ENEMY = "skeleton";
        private const string ENTITY_BOSS = "magolor-soul-white";
        private const string ENTITY_MAGIC = "magic-white";
        private const string ENTITY_PILL = "pill-white";

        List<MagicEntity> MagicList;
        List<EnviromentEntity> EnvList;
        List<EnemyEntity> EnemyList;
        List<EnemyEntity> KillList;

        // For now player one, soooooooo... Wait for multiplayer..

        EnviromentEntity Pill_1;
        EnviromentEntity Pill_2;
         
        PlayerEntity FirstPlayer;
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
        
        private void InitWorld()
        {
            World = new WorldEntity(Content, ENTITY_WORLD, FRAME_WIDTH, FRAME_HEIGHT);
            World.Init();

            MagicEntity Magic_1 = new MagicEntity(Content, ENTITY_MAGIC, FRAME_HEIGHT, FRAME_WIDTH);
            Magic_1.UpdateAnimationBasedOnBoss(206, 188, 0);
            MagicList.Add(Magic_1);
        }
        
        private void InitEnviroment()
        { 
            Pill_1 = new EnviromentEntity(Content, ENTITY_PILL, FRAME_WIDTH, FRAME_HEIGHT);
            Pill_2 = new EnviromentEntity(Content, ENTITY_PILL, FRAME_WIDTH, FRAME_HEIGHT);
            Pill_1.Init(206, 268, true);
            Pill_2.Init(562, 268, false);

            EnvList.Add(Pill_1);
            EnvList.Add(Pill_2);
        }

        private void InitCharacter()
        {
            FirstPlayer = new PlayerEntity(Content, ENTITY_PLAYER, World.WORLD_HEIGHT - FRAME_SCORE_OFFSET, World.WORLD_WIDTH);
            FirstPlayer.Init(); 
        }

        private void CreateEnemy(float X, float Y, bool direction)
        {
            EnemyList.Add(new EnemyEntity(Content, ENTITY_ENEMY, World.WORLD_HEIGHT - FRAME_SCORE_OFFSET, World.WORLD_WIDTH, direction, X, Y));
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;

            MagicList = new List<MagicEntity>();
            EnvList = new List<EnviromentEntity>();
            EnemyList = new List<EnemyEntity>();
            KillList = new List<EnemyEntity>();

            InitWorld();

            InitEnviroment();

            InitCharacter();

            CreateEnemy(World.WORLD_HEIGHT/2, World.WORLD_WIDTH/2, true);

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
        /// 

        private bool CheckOnTouch(Rectangle character, Rectangle enemy)
        {  
            bool collisionX = (character.X + character.Width >= enemy.X) &&             // X axis
                (enemy.X + enemy.Width >= character.X);

            bool collisionY = (character.Y + character.Height >= enemy.Y) &&    // Y axis    
                (enemy.Y + enemy.Height >= character.Y);

            return collisionX && collisionY;
        }

        private void CollideAllEntities()
        {
            foreach (var enemy in EnemyList)                // Check collision and take actions
            {
                if (enemy.IsAlive)
                    if (CheckOnTouch(new Rectangle((int)FirstPlayer.X, (int)FirstPlayer.Y,
                                                        FirstPlayer.GetEntityWidth(), FirstPlayer.GetEntityHeight()),
                                                  new Rectangle((int)enemy.X, (int)enemy.Y,
                                                        enemy.GetEntityWidth(), enemy.GetEntityHeight())))
                    {
                        if (FirstPlayer.GetPLayerStrikeStatus())
                            enemy.IsAlive = false;
                        else if (enemy.GetEnemyStrikeStatus() && FirstPlayer.EntityDirection == enemy.EntityDirection)
                        {
                            if (!enemy.IsAttacked && FirstPlayer.IsAlive)
                            {
                                enemy.IsAttacked = true;
                                FirstPlayer.HealthCount--;
                            }
                        }
                    }
                    else
                        enemy.IsAttacked = false; 
            }


            foreach (var enemy in EnemyList)                    // Clean dead entities
            {
                if (enemy.IsDead) 
                    KillList.Add(enemy); 
            }

            EnemyList.RemoveAll(e => KillList.Contains(e));
            KillList.Clear();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Check for pause input  
            if (Keyboard.GetState().IsKeyDown(Keys.P) && ButtonFlag)
            {
                ButtonFlag = false;
                IsPaused = !IsPaused;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.P))
                ButtonFlag = true;

            // Magic Updates
            foreach(var magic in MagicList)
            {
                magic.Update(gameTime);
                if(magic.IsReadyToSpawn && !magic.IsSpawned)
                {
                    magic.IsSpawned = true;
                    CreateEnemy(magic.X - 30, magic.Y + 20, true);
                }
            }

            // If not paused
            if (!IsPaused)
            {
                // Characters Updates
                FirstPlayer.Update(gameTime);

                //Enemy Update 
                foreach (var enemy in EnemyList)
                {
                    if (!enemy.IsDead)
                    {
                        enemy.SetLevelDimention(FirstPlayer.LEVEL_DIMENTION);
                        enemy.SetLevelDirection(FirstPlayer.LEVEL_DIRECTION);
                        enemy.SetPlayerCoordinates(FirstPlayer.X, FirstPlayer.Y);
                        enemy.Update(gameTime);
                    }
                } 

                CollideAllEntities();

            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Azure);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend); 

            // Background
            World.Draw(spriteBatch);

            //Enviroment
            foreach (var env in EnvList)
                env.Draw(spriteBatch);

            //Characters
            FirstPlayer.Draw(spriteBatch);

            //Magic
            foreach (var magic in MagicList)
                magic.Draw(spriteBatch);

            //Enemies
            foreach (var enemy in EnemyList)
                if (!enemy.IsDead)
                    enemy.Draw(spriteBatch);

            // Pause
            if (IsPaused)
            {
                var rect = new Texture2D(GraphicsDevice, 1, 1);
                rect.SetData(new[] { Color.Black });
                spriteBatch.Draw(rect, new Vector2(0, 0), new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT),
                    Color.Black * 0.3f, 0f, new Vector2(1, 1), 5f, SpriteEffects.None, .5f);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
