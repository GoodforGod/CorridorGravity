using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorridorGravity.GameLogic;
using Microsoft.Xna.Framework.Content;
using CorridorGravity.GameLogic.AnimatedEntity;
using System;

namespace CorridorGravity.GameLogic
{
    class BossEntity : Entity
    {
        private static Color TintColor = Color.White;
        public DateTime DeadTime { get; set; }
        public DateTime StrikeTimeStart { get; set; }

        public override float X { get; set; }
        public override float Y { get; set; }

        private float TransparentPower = 1f;
        private float TransparentInc = 0.005f;

        private int StrikeTimeInSeconds = 4;
        private int OnceAnimationType = -1;
        public bool EntityDirection;

        private int LevelHeight { get; set; }
        private int LevelWidth { get; set; }

        public bool IsSpawned { get; set; }
        public bool IsReadyToSpawn { get; set; }
        public bool IsOnceAnimated { get; set; }
        public bool IsAlive { get; set; }
        public bool IsDead { get; set; }
        public bool IsAttacked { get; set; }

        public int LevelDimention { get; set; }
        public int LevelDirection { get; set; }    // 1 - Correct direction, -1 - inverse

        private Random SpellRandomizer;
        public override Texture2D EntitySprite { get; }
        public Animation CurrentAnimation { get; set; }
        private Magic MagicAnimationPack;
        private Boss AnimationsPack;

        public BossEntity(ContentManager content, string contentName, int levelHeight, int levelWidth)
        {
            LevelHeight = levelHeight / 2;
            LevelWidth = levelWidth / 2;

            MagicAnimationPack = new Magic(content);
            AnimationsPack = new Boss();
            EntitySprite = content.Load<Texture2D>(contentName);
            SpellRandomizer = new Random(Guid.NewGuid().GetHashCode());

            LevelDimention = 0;
            IsAlive = true; 
        }

        public Rectangle GetBossPosition()
        {
            return new Rectangle((int)X, (int)Y, CurrentAnimation.CurrentRectangle.Width, CurrentAnimation.CurrentRectangle.Height);
        }

        public Rectangle GetBossPosition(float X, float Y)
        {
            return new Rectangle((int)X, (int)Y, CurrentAnimation.CurrentRectangle.Width, CurrentAnimation.CurrentRectangle.Height);
        }

        public void UpdateAndRelocate(float X, float Y, int OnceAnimationType, bool EntityDirection)                           // Update current animation
        {
            StrikeTimeStart = DateTime.Now;

            this.EntityDirection = EntityDirection;
            this.X = X;
            this.Y = Y;
            IsAlive = true;

            IsOnceAnimated = true;
            this.OnceAnimationType = OnceAnimationType;

            switch (this.OnceAnimationType)
            {
                case 0: CurrentAnimation = AnimationsPack.Idle; break;
                case 1: CurrentAnimation = AnimationsPack.Walk; break;
                case 2: CurrentAnimation = AnimationsPack.Jump; break;
                case 3: CurrentAnimation = AnimationsPack.Dead; break;
                default: break;
            }
        }

        private void SummonFist()
        {
            var groundCoordinate = 0;

            switch (LevelDimention)
            {
                case 0: break;
                case 1: break;
                case 2: break;
                case 3: break;
                default: break;
            }
        }

        private void SummonRune()
        {
            var groundCoordinate = 0;

            switch (LevelDimention)
            {
                case 0:  break;
                case 1:  break;
                case 2:  break;
                case 3: break;
                default: break;
            }
        }

        private void CollapseGravity()
        {
            var randomGravityDistortion = SpellRandomizer.Next(0,3);

            switch (randomGravityDistortion)
            {
                case 0: LevelDimention = 0; break;
                case 1: LevelDimention = 1; break;
                case 2: LevelDimention = 2; break;
                case 3: LevelDimention = 3; break;
                default: break;
            }
        }

        private void SelectRandomSpell()
        {
            switch (SpellRandomizer.Next(0, 2))
            {
                case 0: CollapseGravity();  break;
                case 1: SummonRune();       break;
                case 2: SummonFist();       break;
                default:                    break;
            }
        }
        
        public override void Update(GameTime gameTime)
        { 
            if (!IsDead)
            {
                IsOnceAnimated = CurrentAnimation.UpdateSingleAnimationIsEnded(gameTime);

                if (IsSpawned)
                {
                    if(!IsAttacked) 
                        SelectRandomSpell();
                     
                    if (IsAlive && (DateTime.Now - StrikeTimeStart).TotalSeconds > StrikeTimeInSeconds)
                        IsAlive = false;

                    if (TransparentPower < 0.1f)
                    {
                        IsSpawned = false;
                        IsDead = true;
                        DeadTime = DateTime.Now;
                    }
                }
                else
                {
                    if (TransparentPower < 1f)
                        TransparentPower += TransparentInc;
                    else IsSpawned = true;
                }
            }
        }

        public override void Draw(SpriteBatch batcher)
        {
            SpriteEffects effectsApplyed = SpriteEffects.None;
            var rotationAngle = .0f;

            switch (LevelDimention)                             // Choose sprite flip, depend on the current dimention
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

            if (!IsDead)
            {
                batcher.Draw(EntitySprite, new Vector2(X, Y), CurrentAnimation.CurrentRectangle, TintColor * TransparentPower,
                                            rotationAngle, new Vector2(1, 1), .7f, effectsApplyed, .0f);
                if (!IsAlive) 
                    TransparentPower -= TransparentInc; 
            } 
        }
    }
}
