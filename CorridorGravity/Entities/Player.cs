using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using CorridorGravity.Animations;

namespace CorridorGravity.Entities
{
    class Player : Entity, IAnimated<AnimationState>
    {
        public AnimationManager<AnimationState> AniManager { get; set; }

        public int Score { get; set; }

        Vector2 Offset;

        public override GroundState GState
        {
            get { return base.GState; }

            set
            {
                base.GState = value;
                RecalculateOffset();
            }
        }

        public Player(World world, Vector2 position, params object[] param) : base(world, position, param)
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();

            this.Health = 6;

            this.AniManager = new AnimationManager<AnimationState>();
            Load(this.GetType().Name);
            this.BoundingBox = new Rectangle((int)Position.X, 
                                                (int)Position.Y, 
                                                AniManager.CSource.Width, 
                                                AniManager.CSource.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void RecalculateOffset()
        {
            switch(GState)
            {
                case GroundState.BOTTOM:
                    break;
                case GroundState.RIGHT:
                    break;
                case GroundState.LEFT:
                    break;
                case GroundState.TOP:
                    break;
                default: break;
            }
        }

        public void Load(string name)
        {
            this.AniManager.Load(name);
        }

        public override void Kill(Entity killer)
        {
            EState = EntityState.DYING;
        }

        public override void Touch(Entity other)
        {
            if (EState == EntityState.ACTIVE && other is Enemy)
            {
                Score += 10;
                base.Touch(other);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (EState != EntityState.DEAD && EState != EntityState.DYING)
                Handler.Handle(this, gameTime);

            this.BoundingBox.X = (int)Position.X;
            this.BoundingBox.Y = (int)Position.Y;

            bool temp = true;

            if (EState != EntityState.DEAD)
                temp = AniManager.UpdateOnce(AState, gameTime);
            if (EState == EntityState.DYING && !temp)
                EState = EntityState.DEAD;

            if(!temp) 
            {
                AState = AnimationState.IDLE;
                EState = EntityState.PASSIVE;
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