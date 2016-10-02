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
        public DateTime DeadTime { get; set; }

        public override float X { get; set; }
        public override float Y { get; set; }
         
        private const float TransparentPower = 0.9f;

        private int OnceAnimationType = -1;
        private bool EntityDirection;

        private int LevelHeight { get; set; }
        private int LevelWidth { get; set; }
        private float RotationAngle { get; set; }

        public bool IsSpawned { get; set; }
        public bool IsReadyToSpawn { get; set; }
        public double IsOnceAnimated { get; set; } 
        public bool IsAlive { get; set; }
        public bool IsDead { get; set; } 

        public int LevelDimention { get; set; } 

        public override Texture2D EntitySprite { get; }
        public Animation CurrentAnimation { get; set; }
        private Magic AnimationsPack; 

        public MagicEntity(ContentManager content, string contentName, int levelHeight, int levelWidth)
        {
            CurrentAnimation = new Animation();
            LevelHeight = levelHeight / 2;
            LevelWidth = levelWidth / 2;
            AnimationsPack = new Magic(content);
            LevelDimention = 0;
            IsAlive = true;
        }
        public Rectangle GetMagicPosition()
        {
            return new Rectangle((int)X, (int)Y, CurrentAnimation.CurrentRectangle.Width, CurrentAnimation.CurrentRectangle.Height);
        }

        public Rectangle GetMagicPosition(float X, float Y)
        {
            return new Rectangle((int)X, (int)Y, CurrentAnimation.CurrentRectangle.Width, CurrentAnimation.CurrentRectangle.Height);
        }

        public void Init(int X, int Y, bool EntityDirection)
        {
            this.X = X;
            this.Y = Y;
            this.EntityDirection = EntityDirection;
        }

        public void UpdateAndRelocate(float X, float Y, int OnceAnimationType)                           // Update current animation
        {
            this.X = X;
            this.Y = Y;
            IsAlive = true;
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
            if (!IsDead)
            {
                if (IsOnceAnimated != -1)
                    IsOnceAnimated = CurrentAnimation.UpdateSingleAnimationTimeSlapsed(gameTime);
                else if (IsAlive)
                {
                    IsReadyToSpawn = false;
                    IsSpawned = false;
                    IsAlive = false;
                }
                if (IsOnceAnimated > CurrentAnimation.Duration.TotalMilliseconds / 2)
                    IsReadyToSpawn = true;
            }
        }

        private SpriteEffects AdjustSpriteEffect()
        {
            switch (LevelDimention)                             // Choose sprite flip, depend on the current dimention
            {
                case 0:
                    RotationAngle = .0f;
                    if (!EntityDirection)
                        return SpriteEffects.FlipHorizontally;
                    else return SpriteEffects.None; 

                case 1:
                    RotationAngle = 1.571f;
                    if (EntityDirection)
                        return SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    else return SpriteEffects.FlipVertically; 

                case 2:
                    RotationAngle = .0f;
                    if (!EntityDirection)
                        return SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    else return SpriteEffects.FlipVertically; 

                case 3:
                    RotationAngle = 1.571f;
                    if (EntityDirection)
                        return SpriteEffects.FlipHorizontally;
                    else return SpriteEffects.None; 

                default: return SpriteEffects.None; 
            }
        }

        public override void Draw(SpriteBatch batcher)
        {
            SpriteEffects effectsApplyed = AdjustSpriteEffect();

            if (IsOnceAnimated != -1)
                batcher.Draw(AnimationsPack.MagicSprite, new Vector2(X, Y), CurrentAnimation.CurrentRectangle, TintColor*TransparentPower,
                                            RotationAngle, new Vector2(1, 1), 1f, effectsApplyed, .0f); 
        } 
    }
}
