using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CorridorGravity.Services;
using CorridorGravity.Animations;

namespace CorridorGravity.Entities
{
    /// <summary>
    /// What part of the dimention is ground now
    /// </summary>
    public enum GroundState
    {
        BOTTOM,
        RIGHT,
        LEFT,
        TOP
    }

    /// <summary>
    /// Represents world level
    /// </summary>
    class Level
    {
        public Rectangle Limits { get; private set; }

        Texture2D LSprite;
        Texture2D FSprite;

        AnimationManager<AnimationState> AniManager;

        int Width;
        int Height;

        const int Offset = 12;

        public Level(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            Initialize();
        }

        public void Initialize()
        {
            this.Limits = new Rectangle(Offset, Offset, Width - Offset, Height - Offset * 4);

            this.AniManager = new AnimationManager<AnimationState>();
            this.AniManager.Load(this.GetType().Name);

            this.LSprite = ServiceLocator.Instance.PLManager.TLoad(ServiceLocator.Instance.PLManager.TList[TTypes.BACKGROUND]);
            this.FSprite = ServiceLocator.Instance.PLManager.TLoad(ServiceLocator.Instance.PLManager.TList[TTypes.MAGIC]);
        }

        public void Update(GameTime gameTime)
        {
            //this.AniManager.UpdateSingleAnim(AnimationState.IDLE, gameTime);
        }

        public void Draw(SpriteBatch batcher)
        {
            batcher.Draw(LSprite, Vector2.Zero, Color.White);
        }
    }
}
