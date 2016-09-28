using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorridorGravity.GameLogic;
using System.Collections.Generic;
using System;

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

        private int RespawnTimeInSeconds;
        private Random RandomMagic = new Random();

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
         
        PlayerEntity Player;
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
            Magic_1.UpdateAndRelocate(206, 188, 0);
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
            Player = new PlayerEntity(Content, ENTITY_PLAYER, World.LevelHeight - FRAME_SCORE_OFFSET, World.LevelWidth);
            Player.Init(); 
        }

        private void CreateEnemy(float X, float Y, bool direction)
        {
            EnemyList.Add(new EnemyEntity(Content, ENTITY_ENEMY, World.LevelHeight - FRAME_SCORE_OFFSET, World.LevelWidth, direction, X, Y));
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

            CreateEnemy(World.LevelHeight/2, World.LevelWidth/2, true);

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

        private void RelocatePortal(MagicEntity magic)
        {
            var newCoordX = RandomMagic.Next(100, FRAME_WIDTH - 100);
            var newCoordY = RandomMagic.Next(100, FRAME_HEIGHT - 100);
            magic.UpdateAndRelocate(newCoordX, newCoordY, 0);
        }

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
                    if (CheckOnTouch(new Rectangle((int)Player.X, (int)Player.Y,
                                                        Player.GetEntityWidth(), Player.GetEntityHeight()),
                                                  new Rectangle((int)enemy.X, (int)enemy.Y,
                                                        enemy.GetEntityWidth(), enemy.GetEntityHeight())))
                    {
                        if (Player.GetPLayerStrikeStatus())               // Kill enemy, if player attacked and was collision
                        {
                            enemy.IsAlive = false;
                            Player.ScoreCount += Player.GetScorePerEnemy();
                        }
                        else if (enemy.GetEnemyStrikeStatus() && Player.EntityDirection == enemy.EntityDirection)
                        {
                            if (!enemy.IsAttacked && Player.IsAlive)         // If player not attacked, but collided, player minuse health
                            {
                                enemy.IsAttacked = true;
                                Player.HealthCount--;
                            }
                        }
                    }
                    else
                        enemy.IsAttacked = false;               // Make sure that enemy attack once per collision
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

            // Check if active, if not, pause game
            if (IsActive)
            {
                // Check for pause input  
                if (Keyboard.GetState().IsKeyDown(Keys.P) && ButtonFlag)
                {
                    ButtonFlag = false;
                    IsPaused = !IsPaused;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.P))
                    ButtonFlag = true;


                // If not paused
                if (!IsPaused && IsActive)
                {
                    // Magic Updates
                    foreach (var magic in MagicList)
                    {
                        magic.Update(gameTime);
                        if (!magic.IsAlive)
                        {
                            if (!magic.IsDead)                                                              // Start respawn timer, if dead
                            {
                                magic.DeadTime = DateTime.Now;
                                magic.IsDead = true;
                                RespawnTimeInSeconds = RandomMagic.Next(1, 9);
                            }
                            else if ((DateTime.Now - magic.DeadTime).TotalSeconds > RespawnTimeInSeconds)   // Relocate when time is up    
                            {
                                magic.IsDead = false;
                                RelocatePortal(magic);
                            }
                        }
                        else magic.LevelDimention = Player.LevelDimention;

                        // Portal spawns enemies
                        if (magic.IsReadyToSpawn && !magic.IsSpawned && magic.IsAlive)
                        {
                            magic.IsSpawned = true;
                            //CreateEnemy(magic.X - 30, magic.Y + 20, true);
                        }
                    }

                    // Characters Updates
                    Player.Update(gameTime);

                    //Enemy Update 
                    foreach (var enemy in EnemyList)
                    {
                        if (!enemy.IsDead)
                        {
                            enemy.SetLevelDimention(Player.LevelDimention);
                            enemy.SetLevelDirection(Player.LevelDirection);
                            enemy.PlayerX = Player.X;
                            enemy.PlayerY = Player.Y;
                            enemy.Update(gameTime);
                        }
                    }

                    // Chekc for collision
                    CollideAllEntities();

                }
            }
            else IsPaused = true;

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

            // If window is active
            if (IsActive)
            {
                // Background
                World.Draw(spriteBatch);

                //Enviroment
                foreach (var env in EnvList)
                    env.Draw(spriteBatch);

                //Characters
                Player.Draw(spriteBatch);

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
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
