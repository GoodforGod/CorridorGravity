using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using CorridorGravity.Animations;

namespace CorridorGravity.Entities
{
    class Enemy : Entity, IAnimated<AnimationState>
    {
        public AnimationManager<AnimationState> AniManager { get; set; }

        public override int JRate { get { return 15; } }

        DateTime Cooldown;

        public Enemy(World world, Vector2 position, params object[] param) : base(world, position, param)
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();

            this.AniManager = new AnimationManager<AnimationState>();
            Load(this.GetType().Name);
            this.BoundingBox = new Rectangle((int)Position.X, 
                                                (int)Position.Y, 
                                                AniManager.CSource.Width, 
                                                AniManager.CSource.Height);

            this.Cooldown = DateTime.Now;
        }

        public void Load(string name)
        {
            this.AniManager.Load(name);
        }

        public override void Touch(Entity other)
        {
            var temp = DateTime.Now;

            if (other is Player && other.EState != EntityState.ACTIVE && (temp - Cooldown).TotalSeconds > 2)
            {
                EState = EntityState.READY;
                Cooldown = temp;
                base.Touch(other);
            }
        }

        public override void Update(GameTime gameTime)
        {
            Handler.Handle(this, gameTime);

            this.BoundingBox.X = (int)Position.X;
            this.BoundingBox.Y = (int)Position.Y;

            var temp = AniManager.UpdateOnce(AState, gameTime);

            if (EState == EntityState.ACTIVE && !temp)
            {
                EState = EntityState.NONE;
                AState = AnimationState.WALK;
            }
        }

        public override void Draw(SpriteBatch batcher, GameTime gameTime)
        {
            batcher.Draw(Sprite,
                            Position,
                            AniManager.CSource,
                            TColor,
                            Angle,
                            Origin,
                            Scale,
                            CEffect,
                            .0f);
        }
    }
}