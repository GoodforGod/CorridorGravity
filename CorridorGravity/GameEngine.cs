using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorridorGravity.GameLogic;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Media;

namespace CorridorGravity
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameEngine : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont StatusFont;
        Song BackgroundSong;

        private const int FRAME_WIDTH = 1024;
        private const int FRAME_HEIGHT = 768;
        private const int FRAME_SCORE_OFFSET = 40;
        private bool IsPaused { get; set; }
        private bool ButtonFlag { get; set; }
        private bool IntroFlag = true;
        private bool DelayFlag = true;

        private float TransparentPower = 0.05f;
        private float TransparentInc = 0.01f;
        private float PortalRespawnTimeInSeconds;
        private float BossRespawnTimeInSeconds;
        private Random LocationRandomizer;
        private int NextScoreGoal = 200;
        private const int ScoreStep = 100;

        private const string ENTITY_PLAYER = "player-2-white-1";
        private const string ENTITY_ENEMY = "skeleton";
        private const string ENTITY_BOSS = "magolor-soul-white";
        private const string ENTITY_MAGIC = "magic-white";

        private const string TEXTURE_WORLD = "world-background-score";
        private const string TEXTURE_PILL = "pill-white";
        private const string TEXTURE_INTRO = "intro";
        private const string TEXTURE_PAUSE = "world-paused";
        private const string TEXTURE_DEAD = "world-dead";
        private const string TEXTURE_NAME = "intro-name";


        private const string FONT_SCORE = "score-font";
        private const string SONG_BACKGROUND = "joshuaempyre-arcade-music-loop"; 

        List<MagicEntity> MagicList;
        List<EnviromentEntity> EnvList;
        List<EnemyEntity> EnemyList;
        List<EnemyEntity> KillList;

        // For now player one, soooooooo... Wait for multiplayer.. 
        Texture2D IntroTexture;
        Texture2D DeadTexture;
        Texture2D PauseTexture;
        Texture2D NameTexture;

        DateTime DelayTime;

        PlayerEntity Player;
        WorldEntity World;
        BossEntity Boss;

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
        
        private void InitCreatures()
        {  
            MagicEntity Magic_1 = new MagicEntity(Content, ENTITY_MAGIC, FRAME_HEIGHT, FRAME_WIDTH);
            Magic_1.UpdateAndRelocate(206, 188, 0);
            MagicList.Add(Magic_1);

            Boss = new BossEntity(Content, ENTITY_BOSS, FRAME_HEIGHT, FRAME_WIDTH);
            Boss.UpdateAndRelocate(500, 500, 0, false);

            LocationRandomizer = new Random(Guid.NewGuid().GetHashCode());
        }
        
        private void InitEnviroment()
        { 
            //Pill_1 = new EnviromentEntity(Content, ENTITY_PILL, FRAME_WIDTH, FRAME_HEIGHT);
            //Pill_2 = new EnviromentEntity(Content, ENTITY_PILL, FRAME_WIDTH, FRAME_HEIGHT);
            //Pill_1.Init(206, 268, true);
            //Pill_2.Init(562, 268, false);

            //EnvList.Add(Pill_1);
            //EnvList.Add(Pill_2);
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
            //IsMouseVisible = true;
            NextScoreGoal = 200;  

            MagicList = new List<MagicEntity>();
            EnvList = new List<EnviromentEntity>();
            EnemyList = new List<EnemyEntity>();
            KillList = new List<EnemyEntity>();


            World = new WorldEntity(Content, TEXTURE_WORLD, FRAME_WIDTH, FRAME_HEIGHT);
            World.Init();

            InitCreatures();

            InitEnviroment();

            InitCharacter();

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

            StatusFont = Content.Load<SpriteFont>(FONT_SCORE);
            BackgroundSong = Content.Load<Song>(SONG_BACKGROUND);

            NameTexture = Content.Load<Texture2D>(TEXTURE_NAME);
            IntroTexture = Content.Load<Texture2D>(TEXTURE_INTRO);
            DeadTexture = Content.Load<Texture2D>(TEXTURE_DEAD);
            PauseTexture = Content.Load<Texture2D>(TEXTURE_PAUSE);

            MediaPlayer.Play(BackgroundSong);
            MediaPlayer.IsRepeating = true;
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
            var newCoordX = 0;
            var newCoordY = 0;

            var bossSummonRect = new Rectangle();

            if (Boss.SummonAnimation == null)
                bossSummonRect = new Rectangle(0, 0, 0, 0);
            else
                Boss.GetSummonPosition();

            do
            {
                newCoordX = LocationRandomizer.Next(100, FRAME_WIDTH - 100);
                newCoordY = LocationRandomizer.Next(100, FRAME_HEIGHT - 100);
            }
            while ((CheckOnTouch(magic.GetMagicPosition(newCoordX, newCoordY), bossSummonRect)));

            magic.UpdateAndRelocate(newCoordX, newCoordY, 0);
        }

        private void RelocateBoss(BossEntity boss)
        {
            var newCoordX = LocationRandomizer.Next(100, FRAME_WIDTH - 220);
            var newCoordY = LocationRandomizer.Next(80, FRAME_HEIGHT - 300);
            boss.UpdateAndRelocate(newCoordX, newCoordY, 0, false);
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
            // check collision vs boss strike 
            if (Boss.IsSummoned)
            {
                if (Boss.SummonAnimation != null && Boss.IsSummoned)
                {
                    if (CheckOnTouch(new Rectangle((int)Player.X, (int)Player.Y,
                                                            Player.GetEntityWidth(), Player.GetEntityHeight()),
                                                      new Rectangle((int)Boss.SummonX, (int)Boss.SummonY, Boss.SummonAnimation.CurrentRectangle.Width, Boss.SummonAnimation.CurrentRectangle.Height))
                                                              && Player.IsAlive
                                                                && (DateTime.Now - Player.LastHitTime).TotalSeconds > 1)
                    {
                        Player.HealthCount--;
                        Player.LastHitTime = DateTime.Now;
                    }
                }
            }


            // Check collision and take actions vs enemy strike
            foreach (var enemy in EnemyList)
            {
                if (enemy.IsAlive)
                    if (CheckOnTouch(new Rectangle((int)Player.X, (int)Player.Y,
                                                        Player.GetEntityWidth(), Player.GetEntityHeight()),
                                                  new Rectangle((int)enemy.X, (int)enemy.Y,
                                                        enemy.GetEntityWidth(), enemy.GetEntityHeight())))
                    {
                        // Kill enemy, if player attacked and was collision
                        if (Player.GetPLayerStrikeStatus())               
                        {
                            enemy.IsAlive = false;
                            Player.ScoreCount += Player.GetScorePerEnemy();
                        }
                        else if (enemy.GetEnemyStrikeStatus() && Player.IsAlive 
                            && (DateTime.Now - Player.LastHitTime).TotalSeconds > 1)
                        {
                            Player.HealthCount--;
                            Player.LastHitTime = DateTime.Now;
                        }
                    } 
            } 
            // Clean dead entities
            foreach (var enemy in EnemyList)                    
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
                if (!IntroFlag)
                {
                    // Check for pause input  
                    if (Keyboard.GetState().IsKeyDown(Keys.P) && ButtonFlag)
                    {
                        ButtonFlag = false;
                        IsPaused = !IsPaused;

                        if (IsPaused)
                            MediaPlayer.Pause();
                        else MediaPlayer.Resume();
                    }
                    if (Keyboard.GetState().IsKeyUp(Keys.P))
                        ButtonFlag = true;

                    if(DelayFlag)
                    {
                        if ((DateTime.Now - DelayTime).TotalSeconds > 6)
                            DelayFlag = false;
                    }

                    // If not paused
                    if (!IsPaused && IsActive)
                    {
                        //Boss
                        if (!DelayFlag)
                        {
                            Boss.PlayerX = Player.X;
                            Boss.PlayerY = Player.Y;
                            Boss.PlayerScore = Player.ScoreCount;
                            //Boss.LevelDimention = Player.LevelDimention;
                            Boss.Update(gameTime);

                            if (Boss.IsDead)
                                Player.LevelDimention = Boss.LevelDimention;

                            if (Boss.IsDead && Boss.IsReadyToSpawn && (DateTime.Now - Boss.DeadTime).TotalSeconds > BossRespawnTimeInSeconds)   // Relocate when time is up    
                            {
                                // Spawn boss when time has come
                                Boss.IsReadyToSpawn = false;
                                Boss.IsDead = false;
                                RelocateBoss(Boss);
                            }
                            else if (!Boss.IsReadyToSpawn && Boss.IsDead)
                            {
                                // Set boss next spawn time
                                Boss.IsReadyToSpawn = true;
                                BossRespawnTimeInSeconds = LocationRandomizer.Next(1, 3);
                                BossRespawnTimeInSeconds -= Player.ScoreCount / 1000;
                                if (BossRespawnTimeInSeconds < 2)
                                    BossRespawnTimeInSeconds = 2;
                            }

                            // Magic Updates
                            foreach (var magic in MagicList)
                            {
                                magic.Update(gameTime);
                                if (!magic.IsAlive)
                                {
                                    // Start respawn timer, if dead
                                    if (!magic.IsDead)
                                    {
                                        magic.DeadTime = DateTime.Now;
                                        magic.IsDead = true;
                                        PortalRespawnTimeInSeconds = LocationRandomizer.Next(2, 9);
                                        PortalRespawnTimeInSeconds -= Player.ScoreCount / 1000;
                                        if (PortalRespawnTimeInSeconds < 1)
                                            PortalRespawnTimeInSeconds = 1;
                                    }
                                    else if ((DateTime.Now - magic.DeadTime).TotalSeconds > PortalRespawnTimeInSeconds)
                                    {
                                        // Relocate when time has come   
                                        magic.IsDead = false;
                                        RelocatePortal(magic);
                                    }
                                }
                                else magic.LevelDimention = Boss.LevelDimention;

                                // Portal spawns enemies
                                if (magic.IsReadyToSpawn && !magic.IsSpawned && magic.IsAlive)
                                {
                                    magic.IsSpawned = true;
                                    CreateEnemy(magic.X - 30, magic.Y + 20, true);
                                }
                            }
                        }
                        // Characters Updates
                        Player.Update(gameTime);

                        if (!DelayFlag)
                        {
                            //Enemy Update 
                            foreach (var enemy in EnemyList)
                            {
                                if (!enemy.IsDead)
                                {
                                    enemy.LevelDimention = Player.LevelDimention;
                                    enemy.LevelDirection = Player.LevelDirection;
                                    enemy.PlayerX = Player.X;
                                    enemy.PlayerY = Player.Y;
                                    enemy.Update(gameTime);
                                }
                            }

                            // Chekc for collision
                            CollideAllEntities();

                            //Spawn more portals
                            if (Player.ScoreCount > NextScoreGoal)
                            {
                                MagicList.Add(new MagicEntity(Content, ENTITY_MAGIC, FRAME_HEIGHT, FRAME_WIDTH));
                                NextScoreGoal += ScoreStep;
                            }
                        }

                    }
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && ButtonFlag)
                    {
                        ButtonFlag = false;
                        IntroFlag = false;
                        DelayTime = DateTime.Now;
                    }
                    if (Keyboard.GetState().IsKeyUp(Keys.Space))
                        ButtonFlag = true;
                }
            }
            else
            {
                MediaPlayer.Pause();
                IsPaused = true;
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

            // If window is active
            if (IsActive)
            {
                if (!IntroFlag)
                {
                    // Background
                    World.Draw(spriteBatch);

                    //Enviroment
                    //foreach (var env in EnvList)
                    //    env.Draw(spriteBatch);


                    //Magic
                    foreach (var magic in MagicList)
                        magic.Draw(spriteBatch);

                    //Boss
                    Boss.Draw(spriteBatch);

                    //Characters
                    Player.Draw(spriteBatch);

                    //Enemies
                    foreach (var enemy in EnemyList)
                        if (!enemy.IsDead)
                            enemy.Draw(spriteBatch);

                    // Pause
                    if (IsPaused)
                    { 
                        spriteBatch.Draw(PauseTexture, new Vector2(0, 0), new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT),
                            Color.White * 0.3f, 0f, new Vector2(1, 1), 5f, SpriteEffects.None, .5f);
                    }

                    if (Player.IsDead)
                    { 
                        spriteBatch.Draw(DeadTexture, new Vector2(0, 0), new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT), 
                            Color.White * TransparentPower, 0f, new Vector2(1, 1), 1f, SpriteEffects.None, TransparentPower);

                        if (TransparentPower < .99f)
                            TransparentPower += TransparentInc;
                    }
                }
                else
                {
                    spriteBatch.Draw(IntroTexture, new Vector2(0, 0), new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT), Color.White,
                        0f, new Vector2(1, 1), 1f, SpriteEffects.None, 1f);

                    spriteBatch.Draw(NameTexture, new Vector2(0, 0), new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT), Color.White,
                        0f, new Vector2(1, 1), 1f, SpriteEffects.None, 1f);
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
