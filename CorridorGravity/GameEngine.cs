using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorridorGravity.GameLogic;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace CorridorGravity
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameEngine : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private bool IsPaused { get; set; }
        private bool ButtonFlag { get; set; }
        private bool IntroFlag = true;
        private bool DelayFlag = true;
        private bool SongFlag = false;
        private bool DeadSongFlag = false;

        private float TransparentPower = 0.05f;
        private float TransparentInc = 0.01f;
        private float PortalRespawnTimeInSeconds;
        private float BossRespawnTimeInSeconds;
        private long NextScoreGoal = 60;
        private const int ScoreStep = 50;

        private const float MinBossSpawnTime = 0.5f;
        private const float MinPortalSpawnTime = 0.5f;
         
        private const int FRAME_WIDTH = 1024;
        private const int FRAME_HEIGHT = 768;
        private const int FRAME_SCORE_OFFSET = 40;

        private const string ENTITY_PLAYER = "player-2-white-1";
        private const string ENTITY_ENEMY = "skeleton";
        private const string ENTITY_BOSS = "magolor-soul-white";
        private const string ENTITY_MAGIC = "magic-white";

        private const string TEXTURE_WORLD = "world-background-score-1";
        private const string TEXTURE_PILL = "pill-white";
        private const string TEXTURE_INTRO = "intro-1";
        private const string TEXTURE_PAUSE = "world-paused";
        private const string TEXTURE_DEAD = "world-dead";
        private const string TEXTURE_NAME = "intro-name-1";
        private const string TEXTURE_SCORE = "score";

        private const string FONT_SCORE = "score-font";
        private const string SONG_BACKGROUND = "joshuaempyre-arcade-music-loop";
        private const string SONG_INTRO = "xdimebagx-atmosphere-horror-loop";
        private const string SONG_LAUGH = "klankbeeld-laugh";
        private const string SONG_DEAD = "rocotilos-game-over-evil";
         
        SpriteFont StatusFont;
        Song BackgroundSong;
        Song IntroSong;
        Song LaughSong;
        SoundEffect DeadSong;

        List<MagicEntity> MagicList;
        List<EnviromentEntity> EnvList;
        List<EnemyEntity> EnemyList;
        List<EnemyEntity> KillList;

        // For now player one, soooooooo... Wait for multiplayer.. 
        Texture2D IntroTexture;
        Texture2D DeadTexture;
        Texture2D PauseTexture;
        Texture2D NameTexture;
        Texture2D ScoreTexture;

        DateTime DelayTime;
        Random PortalRandomizer;
        Random BossRandomizer;

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
         
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //IsMouseVisible = true; 

            MagicList = new List<MagicEntity>();
            EnvList = new List<EnviromentEntity>();
            EnemyList = new List<EnemyEntity>();
            KillList = new List<EnemyEntity>();
             
            World = new WorldEntity(Content, TEXTURE_WORLD, FRAME_WIDTH, FRAME_HEIGHT);
            World.Init();

            InitCharacter();

            InitCreatures();

            InitEnviroment(); 

            base.Initialize();
        }

        // Find new random algorithm, part of it
        public static uint BitRotate(uint x)
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

        private void InitCreatures()
        {  
            MagicEntity Magic_1 = new MagicEntity(Content, ENTITY_MAGIC, FRAME_HEIGHT, FRAME_WIDTH);
            Magic_1.UpdateAndRelocate(206, 188, 0);
            MagicList.Add(Magic_1);

            Boss = new BossEntity(Content, ENTITY_BOSS, FRAME_HEIGHT, FRAME_WIDTH);
            Boss.UpdateAndRelocate(500, 300, 0, false, Player.ScoreCount);

            PortalRandomizer = new Random((int)GenerateRandomCoordinate(DateTime.Now.Second, DateTime.Now.Millisecond));
            BossRandomizer = new Random(((int)GenerateRandomCoordinate(DateTime.Now.Second, DateTime.Now.Millisecond) * DateTime.Now.Millisecond));
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
            IntroSong = Content.Load<Song>(SONG_INTRO);
            LaughSong = Content.Load<Song>(SONG_LAUGH);
            DeadSong = Content.Load<SoundEffect>(SONG_DEAD);

            ScoreTexture = Content.Load<Texture2D>(TEXTURE_SCORE);
            NameTexture = Content.Load<Texture2D>(TEXTURE_NAME);
            IntroTexture = Content.Load<Texture2D>(TEXTURE_INTRO);
            DeadTexture = Content.Load<Texture2D>(TEXTURE_DEAD);
            PauseTexture = Content.Load<Texture2D>(TEXTURE_PAUSE);

            MediaPlayer.Play(IntroSong);
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

            if (Boss.SummonRuneAnimation == null)
                bossSummonRect = new Rectangle(0, 0, 0, 0);
            else
                bossSummonRect = Boss.GetBossSummonPosition();

            do
            {
                newCoordX = PortalRandomizer.Next(40, FRAME_WIDTH - 80);
                newCoordY = PortalRandomizer.Next(20, FRAME_HEIGHT - 80);
            }
            while ((CheckOnTouch(magic.GetMagicPosition(newCoordX, newCoordY), bossSummonRect)));

            magic.UpdateAndRelocate(newCoordX, newCoordY, 0);
        }

        private void RelocateBoss(BossEntity boss)
        {
            var newCoordX = BossRandomizer.Next(100, FRAME_WIDTH - 220);
            var newCoordY = BossRandomizer.Next(80, FRAME_HEIGHT - 300);

            boss.UpdateAndRelocate(newCoordX, newCoordY, 0, false, Player.ScoreCount);
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
                if ((Boss.IsFistSummoned || Boss.IsRuneSummoned) && Boss.IsSummoned)
                {
                    foreach (var spell in Boss.SpellList)
                    {
                        var collideRectangle = new Rectangle();
                        if (spell.SpellType == 0)
                            collideRectangle = new Rectangle((int)spell.X, (int)spell.Y, 
                                                        Boss.SummonFistAnimation.CurrentRectangle.Width, 
                                                            Boss.SummonFistAnimation.CurrentRectangle.Height);
                        else
                            collideRectangle = new Rectangle((int)spell.X, (int)spell.Y,
                                                        Boss.SummonRuneAnimation.CurrentRectangle.Width,
                                                            Boss.SummonRuneAnimation.CurrentRectangle.Height);

                        if (Player.IsAlive && (DateTime.Now - spell.LastHitTime).TotalSeconds > 1 
                                        &&  CheckOnTouch(new Rectangle((int)Player.X, (int)Player.Y, Player.GetEntityWidth(), Player.GetEntityHeight()),
                                                collideRectangle))
                        {
                            Player.HealthCount--;
                            spell.LastHitTime = DateTime.Now;
                        }
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
                            && (DateTime.Now - enemy.LastHitTime).TotalSeconds > 1)
                        {
                            Player.HealthCount--;
                            enemy.LastHitTime = DateTime.Now;
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

                    // Delay before fight
                    if(DelayFlag)
                    {
                        if ((DateTime.Now - DelayTime).TotalSeconds > 6)
                            DelayFlag = false; 
                    }
                    else if(SongFlag)
                    {
                        SongFlag = false;
                        MediaPlayer.Volume = 0f;
                        MediaPlayer.Play(BackgroundSong);
                        MediaPlayer.IsRepeating = true;
                    }
                    else if (MediaPlayer.Volume < 1f && !SongFlag && !Player.IsDead) 
                        MediaPlayer.Volume += 0.003f;
                    
                    // Shut volume down when dead
                    if (Player.IsDead && MediaPlayer.Volume > 0 && !DeadSongFlag)
                    {
                        MediaPlayer.Volume -= 0.01f;
                        if (MediaPlayer.Volume < 0)
                            MediaPlayer.Volume = 0;
                    } 
                    else if(Player.IsDead && !DeadSongFlag)
                    {
                        DeadSongFlag = true;
                        MediaPlayer.IsRepeating = false;
                        MediaPlayer.Stop();
                        MediaPlayer.Volume = 1f; 
                        DeadSong.Play(); 
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
                                BossRespawnTimeInSeconds = BossRandomizer.Next(1, 3);
                                BossRespawnTimeInSeconds -= Player.ScoreCount / 500;
                                if (BossRespawnTimeInSeconds < MinBossSpawnTime)
                                    BossRespawnTimeInSeconds = MinBossSpawnTime;
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
                                        PortalRespawnTimeInSeconds = BossRandomizer.Next(2, 7);
                                        PortalRespawnTimeInSeconds -= Player.ScoreCount / 300;
                                        if (PortalRespawnTimeInSeconds < MinPortalSpawnTime)
                                            PortalRespawnTimeInSeconds = MinPortalSpawnTime;
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
                                    var directionBoll = true; 
                                    if (BossRandomizer.Next(0, 10) > 5)
                                        directionBoll = false;
                                    CreateEnemy(magic.X - 30, magic.Y + 20, directionBoll);
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
                                    enemy.RotationAngle = Player.RotationAngle;
                                    enemy.LevelDimention = Player.LevelDimention;
                                    enemy.LevelDirection = Player.LevelDirection;
                                    enemy.PlayerX = Player.X;
                                    enemy.PlayerY = Player.Y;
                                    enemy.PlayerScore = Player.ScoreCount;
                                    enemy.Update(gameTime);
                                }
                            }

                            // Check for collision
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
                        if (!SongFlag)
                        {
                            SongFlag = true;
                            MediaPlayer.Play(LaughSong);
                            MediaPlayer.Volume = 1f;
                            MediaPlayer.IsRepeating = false;
                        }
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

                    // Draw players scores
                    spriteBatch.Draw(ScoreTexture, new Vector2(FRAME_WIDTH - 300, FRAME_HEIGHT - 55),
                                    new Rectangle(0, 0, 299, 85), Color.White,
                                        0f, new Vector2(1, 1), 0.5f, SpriteEffects.None, 1f);

                    spriteBatch.DrawString(StatusFont, Player.ScoreCount.ToString(),
                                        new Vector2(FRAME_WIDTH - 150, FRAME_HEIGHT - 52), Color.Red,
                                                0f, new Vector2(1, 1), 1f, SpriteEffects.None, 0f);

                    // Pause
                    if (IsPaused && !Player.IsDead) 
                        spriteBatch.Draw(PauseTexture, new Vector2(0, 0), new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT),
                            Color.White * 0.5f, 0f, new Vector2(1, 1), 1f, SpriteEffects.None, 0.5f);

                    // Draw dead title and score
                    if (Player.IsDead)
                    { 
                        spriteBatch.Draw(DeadTexture, new Vector2(0, 0), new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT), 
                                Color.White * TransparentPower, 0f, new Vector2(1, 1), 1f, SpriteEffects.None, TransparentPower);
                         
                        spriteBatch.DrawString(StatusFont, Player.ScoreCount.ToString(), 
                                        new Vector2(FRAME_WIDTH / 2, FRAME_HEIGHT / 2 + 120), Color.Red * TransparentPower, 
                                                0f, new Vector2(1,1), 1f, SpriteEffects.None, 0f);

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
