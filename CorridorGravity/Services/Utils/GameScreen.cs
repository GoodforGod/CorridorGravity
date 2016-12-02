using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using CorridorGravity.Entities;

namespace CorridorGravity.Services.Utils
{
    class GameScreen : Screen
    {
        SpriteFont      Font        { get; set; }
        Rectangle       HSource     { get; set; }
        Rectangle       SSource     { get; set; }
        Rectangle       PSource     { get; set; }
        World           WCore       { get; set; }

        bool            IsPaused    { get; set; }
        bool            IsSoundOver { get; set; }
        bool            IsSongStart { get; set; }

        public override void Initialize(GraphicsDevice graphdev, Game game, params object[] param)
        {
            base.Initialize(graphdev, game);

            this.SLocator = ServiceLocator.Instance;
            this.WCore = new World(CoreGame, GraphDev.Viewport);

            this.SLocator.AManager.Stop();
            this.SLocator.AManager.MaxVolume(1f);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            this.PSource = SLocator.PLManager.TSources[TTypes.PAUSE];
            this.SSource = SLocator.PLManager.TSources[TTypes.SCORE];
            this.HSource = SLocator.PLManager.TSources[TTypes.HEALTH];

            this.Font    = SLocator.PLManager.FLoad(SLocator.PLManager.FONT);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            this.WCore.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            // Pause/Unpause and pause/resume music
            if (state.IsKeyDown(Keys.P) && PState.IsKeyUp(Keys.P))
            {
                IsPaused = !IsPaused;
                SLocator.AManager.PauseOrResume();
            }

            // Check of we faded an start sound effect play
            if(IsFaded && !IsSoundOver)
            {
                IsSoundOver = true;
                SLocator.AManager.Play(SongTypes.LAUGH);
            }

            if(IsFaded && !IsSongStart && !SLocator.AManager.IsSoundPlaying)
            {
                IsSongStart = true;
                SLocator.AManager.Play(SongTypes.INGAME);
            }

            this.PState = state;

            // If not paused, update world
            if (!IsPaused)
                WCore.Update(gameTime);

            //If player is dead start fade out
            if (!WCore.IsPlayerAlive)
                StartFade();

            // switch to dead screen if faded
            if(!WCore.IsPlayerAlive && IsFaded)
                SManager.SwitchScreen(ScreenTypes.DEAD, WCore.PSCore);
        }

        protected void DrawUI(SpriteBatch batcher)
        {
            var offset = 40;

            // Draw heath bar
            var scoreCoord = new Vector2(FWidth - 260, FHeight - 45);
            for (int i = 0; i < WCore.PHealth; i++)
                batcher.Draw(TLabels, new Vector2(20 + offset * i, scoreCoord.Y), HSource, Color.White);

            // Score label
            batcher.Draw(TLabels, scoreCoord, SSource, Color.White, .0f, Vector2.Zero, 0.5f, SpriteEffects.None, .0f);

            // Scores count
            batcher.DrawString(Font, 
                WCore.PSCore.ToString(), 
                new Vector2(scoreCoord.X + SSource.Width/2 + 20, scoreCoord.Y - 10), 
                Color.DarkRed);
        }

        protected void DrawPause(SpriteBatch batcher)
        {
            batcher.Draw(TLabels,
                           Vector2.Zero,
                           FSource,
                           Color.Black * 0.4f,
                           .0f,
                           Vector2.Zero,
                           14f,
                           SpriteEffects.None,
                           .0f);

            batcher.Draw(TLabels, new Vector2(FWidth / 3.5f, FHeight / 3), PSource, Color.White);
        }

        public override void Draw(SpriteBatch batcher, GameTime gameTime)
        {
            batcher.Begin();

            WCore.Draw(batcher, gameTime);

            DrawUI(batcher);

            if (IsPaused)
                DrawPause(batcher);

            //Fade effect
            base.Draw(batcher, gameTime);

            batcher.End();
        }
    }
}
