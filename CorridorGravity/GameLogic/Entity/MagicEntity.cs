using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorridorGravity.GameLogic;
using Microsoft.Xna.Framework.Content;
using CorridorGravity.GameLogic.AnimatedEntity;
using System;

namespace CorridorGravity.GameLogic
{
    class MagicEntity : Entity
    {
        private static Color TintColor = Color.White;

        public override float X { get; set; }
        public override float Y { get; set; }
          
        private int OnceAnimationType = -1;

        private int LEVEL_HEIGHT { get; set; }
        private int LEVEL_WIDTH { get; set; }

        public bool IsSpawned { get; set; }
        public bool IsReadyToSpawn { get; set; }
        public double IsOnceAnimated { get; set; } 
        public bool IsAlive { get; set; }
        public bool IsDead { get; set; }
        private float TransparentPower = 1f;

        private int LEVEL_DIMENTION { get; set; }
        private bool EntityDirection;

        public override Texture2D EntitySprite { get; }
        public Animation CurrentAnimation { get; set; }
        private Magic AnimationsPack;

        public MagicEntity(ContentManager content, int levelHeight, int levelWidth)
        {
            EntitySprite = content.Load<Texture2D>("magic-white");
            ConstractCommonParts(levelHeight, levelWidth);
        }

        public MagicEntity(ContentManager content, string contentName, int levelHeight, int levelWidth)
        {
            EntitySprite = content.Load<Texture2D>(contentName);
            ConstractCommonParts(levelHeight, levelWidth);
        }

        private void ConstractCommonParts(int levelHeight, int levelWidth)
        {
            LEVEL_HEIGHT = levelHeight / 2;
            LEVEL_WIDTH = levelWidth / 2;
            AnimationsPack = new Magic();
            LEVEL_DIMENTION = 0;
            IsAlive = true;
        } 

        public Rectangle GetEnviromentSizes()
        {
            return new Rectangle((int)X, (int)Y, EntitySprite.Width, EntitySprite.Height);
        }

        public void Init(int X, int Y, bool EntityDirection)
        {
            this.X = X;
            this.Y = Y;
            this.EntityDirection = EntityDirection;
        }

        public void UpdateAnimationBasedOnBoss(float X, float Y, int OnceAnimationType)                           // Update current animation
        {
            this.X = X;
            this.Y = Y;
            IsOnceAnimated = 0;
            this.OnceAnimationType = OnceAnimationType;
            switch (this.OnceAnimationType)
            {
                case 0: CurrentAnimation = AnimationsPack.Idle;  break;
                case 1: CurrentAnimation = AnimationsPack.Walk;  break;
                case 2: CurrentAnimation = AnimationsPack.Jump;  break;
                case 3: CurrentAnimation = AnimationsPack.Dead;  break;
                default:                                         break;
            }  
        }
        
        public override void Update(GameTime gameTime)
        { 
            if (IsOnceAnimated != -1)
                IsOnceAnimated = CurrentAnimation.UpdateSingleAnimationTimeSlapsed(gameTime);
            else
            {
                IsReadyToSpawn = false;
                IsSpawned = false;

                if (IsOnceAnimated == -1)
                    IsOnceAnimated = 0;
            }
            if(IsOnceAnimated > CurrentAnimation.Duration.TotalMilliseconds / 2)
                IsReadyToSpawn = true;
        }

        public override void Draw(SpriteBatch batcher)
        {
            SpriteEffects effectsApplyed = SpriteEffects.None;  
            var rotationAngle = .0f;
            
            switch (LEVEL_DIMENTION)                             // Choose sprite flip, depend on the current dimention
            {
                case 0:
                    rotationAngle = .0f;
                    if (!EntityDirection)
                        effectsApplyed = SpriteEffects.FlipHorizontally;
                    else effectsApplyed = SpriteEffects.None;
                    break;

                case 1:
                    rotationAngle = 1.571f;
                    if (EntityDirection)
                        effectsApplyed = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    else effectsApplyed = SpriteEffects.FlipVertically;
                    break;

                case 2:
                    rotationAngle = .0f;
                    if (!EntityDirection)
                        effectsApplyed = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    else effectsApplyed = SpriteEffects.FlipVertically;
                    break;

                case 3:
                    rotationAngle = 1.571f;
                    if (EntityDirection)
                        effectsApplyed = SpriteEffects.FlipHorizontally;
                    else effectsApplyed = SpriteEffects.None;
                    break;

                default: break;
            }

            if (IsOnceAnimated != -1)
                batcher.Draw(EntitySprite, new Vector2(X, Y), CurrentAnimation.CurrentRectangle, TintColor*TransparentPower,
                                            rotationAngle, new Vector2(1, 1), 1f, effectsApplyed, .0f);

        }

        public override void Touch()
        {
            base.Touch();
        }
    }
}
