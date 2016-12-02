using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using CorridorGravity.Animations;

namespace CorridorGravity.Entities
{
    public enum MagicState
    {
        FIST,
        PORTAL,
        POOL,
        GRAVITY,
        FIRE,
        SMOKE,
        DEAD
    }

    class Magic : Entity, IAnimated<MagicState>
    {
        public AnimationManager<MagicState> AniManager      { get; set; }
        public GlideAnimation               GlideAnimation  { get; set; }

        DateTime Cooldown;
        double DelayTime;
        float Alpha;
        
        /// <summary>
        /// Is entity using glie nimation or not
        /// </summary>
        bool IsGlided;

        /// <summary>
        /// Is entity animated or static
        /// </summary>
        bool IsAnimated;

        /// <summary>
        /// Glide animation offset
        /// </summary>
        Vector2 GPosition;
        MagicState MState { get; set; }

        public Magic(World world, Vector2 position, params object[] param) : base(world, position)
        {
            this.MState = (MagicState)param[0];
            this.GState = (GroundState)param[1];
            this.Velocity.X = (float)param[2];
            this.Alpha = (float)param[3];

            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();

            this.AniManager = new AnimationManager<MagicState>();
            Load(this.GetType().Name);

            if (MState != MagicState.GRAVITY && MState != MagicState.PORTAL)
                BoundingBox = AniManager.CSource;

            if (MState == MagicState.FIST)
            {
                this.GlideAnimation = new GlideAnimation(5);
                IsGlided = true;

                if (Velocity.X > 0)
                    this.CEffect = SpriteEffects.FlipHorizontally;
            }

            if (AniManager.Animations.ContainsKey(MState))
                IsAnimated = true;

            this.GPosition = Vector2.Zero;
            this.Cooldown = DateTime.Now;
            this.DelayTime = 5;
        }

        public void Load(string name)
        {
            this.AniManager.Load(name);
        }

        public override void Touch(Entity other)
        {
            var temp = DateTime.Now;

            if (other is Player && other.AState != AnimationState.FIRE_SPECIAL && (temp - Cooldown).TotalSeconds > 2)
            {
                Cooldown = temp;
                base.Touch(other);
            }
        }

        public override void Update(GameTime gameTime)
        {
            bool temp = true;

            if (IsAnimated)
                temp = AniManager.UpdateOnce(MState, gameTime);

            if(IsGlided)
                GlideAnimation.Update(gameTime);

            // make actions depend on magic type
            switch (MState)
            {
                case MagicState.FIST:
                    switch (GState)
                    {
                        case GroundState.BOTTOM:
                        case GroundState.TOP:
                            Position.X += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                            GPosition.Y = GlideAnimation.COffset;
                            BoundingBox.Y = (int)Position.Y;
                            BoundingBox.X = (int)Position.X;

                            if (Position.X > WCore.LLimits.Width || Position.X < WCore.LLimits.X)
                                Kill(this);
                            break;

                        case GroundState.RIGHT: 
                        case GroundState.LEFT:
                            Position.Y += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                            GPosition.X = GlideAnimation.COffset;
                            BoundingBox.X = (int)Position.X;
                            BoundingBox.Y = (int)Position.Y;

                            if (Position.Y > WCore.LLimits.Height || Position.Y < WCore.LLimits.Y)
                                Kill(this);
                            break;

                        default: break;
                    }
                    break;

                case MagicState.DEAD:
                    Alpha -= 0.03f;
                    if (!temp)
                        Kill(this);
                    break;

                case MagicState.PORTAL:
                    if (EState == EntityState.NONE && AniManager.CTimeIntoAnimation.TotalSeconds > AniManager.CDuration.TotalSeconds / 2)
                    {
                        EState = EntityState.PASSIVE;
                        WCore.AddToSpawnQueue(typeof(Enemy).FullName, Position);
                    }
                    if (!temp)
                        Kill(this);
                    break;
                default:
                    if (!temp)
                        Kill(this);
                    break;
            }
        }

        public override void Draw(SpriteBatch batcher, GameTime gameTime)
        {
            batcher.Draw(Sprite,
                            Position + GPosition,
                            AniManager.CSource,
                            TColor * Alpha,
                            Angle,
                            Origin,
                            Scale,
                            CEffect,
                            .0f);
        }
    }
}
