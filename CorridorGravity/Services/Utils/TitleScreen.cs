using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CorridorGravity.Services.Utils
{
    class TitleScreen : Screen
    {
        Rectangle       IntroName;
        Rectangle       IntroPress;
        Texture2D       TitleBackground;

        Vector2 NamePosition;
        Vector2 PressPosition;

        bool IsReady;

        public override void Initialize(GraphicsDevice graphdev, Game game, params object[] param)
        {
            base.Initialize(graphdev, game);

            this.NamePosition = new Vector2(FWidth / 4, FHeight / 5);
            this.PressPosition = new Vector2(FWidth / 2.5f, FHeight - 100);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            this.TitleBackground = Content.Load<Texture2D>(SLocator.PLManager.TRoot + SLocator.PLManager.TList[TTypes.INTRO]);
            this.IntroName = SLocator.PLManager.TSources[TTypes.INTRO_NAME];
            this.IntroPress = SLocator.PLManager.TSources[TTypes.PRESS];

            this.SLocator.AManager.Play(SongTypes.INTRO);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            // Start fading out 
            if (state.IsKeyDown(Keys.Space) && PState.IsKeyUp(Keys.Space) && IsFaded)
            {
                StartFade();
                IsReady = true; 
            }

            // switch to game screen when pressed
            if (IsReady && IsFaded)
                SManager.SwitchScreen(ScreenTypes.GAME);
            else if (IsReady && !IsFaded)
                SLocator.AManager.Mute(FadeRatio);

            this.PState = state;
        }

        public override void Draw(SpriteBatch batcher, GameTime gameTime)
        {
            batcher.Begin();

            batcher.Draw(TitleBackground, Vector2.Zero, Color.White);
            batcher.Draw(TLabels, NamePosition, IntroName, Color.White);
            batcher.Draw(TLabels, PressPosition, IntroPress, Color.White);

            // Fade effect
            base.Draw(batcher, gameTime);

            batcher.End();
        }
    }
}
