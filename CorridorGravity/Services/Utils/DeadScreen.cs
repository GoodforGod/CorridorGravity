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
    class DeadScreen : Screen
    {
        SpriteFont Font;
        Rectangle DSource;
        Rectangle SSource;

        int Score;
        bool IsDead;

        public override void Initialize(GraphicsDevice graphdev, Game game, params object[] param)
        {
            base.Initialize(graphdev, game);

            this.Score = (int)param[0];
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            this.DSource = SLocator.PLManager.TSources[TTypes.DEAD];
            this.SSource = SLocator.PLManager.TSources[TTypes.SCORE];
            this.Font    = SLocator.PLManager.FLoad(SLocator.PLManager.FONT);
        }

        public override void Update(GameTime gameTime)
        {
            if(IsFaded && !IsDead)
            {
                IsDead = true;
                SLocator.AManager.Play(SongTypes.DEAD);
            }
        }

        public override void Draw(SpriteBatch batcher, GameTime gameTime)
        {
            batcher.Begin();

            var coord = new Vector2(FWidth/3, FHeight/3);

            batcher.Draw(TLabels,
                         Vector2.Zero,
                         FSource,
                         Color.Black,
                         .0f,
                         Vector2.Zero,
                         14f,
                         SpriteEffects.None,
                         .0f);

            // Dead
            batcher.Draw(TLabels,
                            coord,
                            DSource,
                            Color.White,
                            .0f,
                            Vector2.Zero,
                            0.8f,
                            SpriteEffects.None,
                            .0f);

            // Score
            batcher.Draw(TLabels, 
                            new Vector2(coord.X, coord.Y + 100),
                            SSource,
                            Color.White, 
                            .0f, 
                            Vector2.Zero, 
                            0.8f, 
                            SpriteEffects.None, 
                            .0f);

            // Score count
            batcher.DrawString(Font,
                           Score.ToString(),
                           new Vector2(coord.X, coord.Y + 100),
                           Color.DarkRed);

            batcher.End();

            base.Draw(batcher, gameTime);
        }
    }
}
