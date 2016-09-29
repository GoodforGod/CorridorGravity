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

        public float SummonX { get; set; }
        public float SummonY { get; set; }

        public override float X { get; set; }
        public override float Y { get; set; }

        public float PlayerX { get; set; }
        public float PlayerY { get; set; }

        private float TransparentPower = 0.01f;
        private float TransparentInc = 0.005f; 
        private float SummonVelocity = 4.8f;

        private int StrikeTimeInSeconds = 10;
        private int OnceAnimationType = -1;
        public bool EntityDirection;

        private int LevelHeight { get; set; }
        private int LevelWidth { get; set; }

        public bool IntroFlag { get; set; }
        public bool IsSpawned { get; set; }
        public bool IsReadyToSpawn { get; set; }
        public bool IsOnceAnimated { get; set; }
        public bool IsAlive { get; set; }
        public bool IsDead { get; set; }
        public bool IsAttacked { get; set; }
        public bool IsSummonReady{ get; set; }
        public bool IsSummoned { get; set; }
        public bool SummonDirection { get; set; }

        public long PlayerScore = 1;
        public int FutureDimention { get; set; }
        public int LevelDimention { get; set; }
        public int LevelDirection { get; set; }    // 1 - Correct direction, -1 - inverse

        private Random SpellRandomizer;
        public override Texture2D EntitySprite { get; }
        public Animation CurrentAnimation { get; set; }
        public Animation SummonAnimation { get; set; }
        private Magic MagicAnimationPack;
        private Boss AnimationsPack;

        public BossEntity(ContentManager content, string contentName, int levelHeight, int levelWidth)
        {
            LevelHeight = levelHeight;
            LevelWidth = levelWidth;

            MagicAnimationPack = new Magic(content);
            AnimationsPack = new Boss();
            EntitySprite = content.Load<Texture2D>(contentName);
            SpellRandomizer = new Random(Guid.NewGuid().GetHashCode());

            LevelDimention = 0;
            IsAlive = true; 
        }

        public Rectangle GetSummonPosition()
        {
            return new Rectangle((int)SummonX, (int)SummonY, SummonAnimation.CurrentRectangle.Width, SummonAnimation.CurrentRectangle.Height);
        }

        public Rectangle GetSummonPosition(float X, float Y)
        {
            return new Rectangle((int)X, (int)Y, SummonAnimation.CurrentRectangle.Width, SummonAnimation.CurrentRectangle.Height);
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
            IsSummoned = true;
            SummonAnimation = AnimationsPack.StrikeOne;

            if (SpellRandomizer.Next(1, 2) == 1)
                SummonVelocity = -SummonVelocity; 

            switch (LevelDimention)
            {
                case 0: 
                    SummonY = LevelHeight - 130;
                    if (SummonVelocity > 0)
                    {
                        SummonX = 0;
                        SummonDirection = false;
                    }
                    else
                    {
                        SummonX = LevelWidth;
                        SummonDirection = true; 
                    }
                    break;
                case 1:
                    SummonX = LevelWidth - 10;
                    if (SummonVelocity > 0)
                    {
                        SummonY = 0;
                        SummonDirection = false;
                    }
                    else
                    {
                        SummonY = LevelHeight;
                        SummonDirection = true;
                    }
                    break;
                case 2:
                    SummonY = 20;
                    if (SummonVelocity > 0)
                    {
                        SummonX = 0;
                        SummonDirection = false;
                    }
                    else
                    {
                        SummonX = LevelWidth;
                        SummonDirection = true;
                    }
                    break;
                case 3:
                    SummonX = 60;
                    if (SummonVelocity > 0)
                    {
                        SummonY = 0;
                        SummonDirection = false;
                    }
                    else
                    {
                        SummonY = LevelHeight;
                        SummonDirection = true;
                    }
                    break;
                default: break;
            }
        }

        private void SummonRune()
        { 
            IsSummoned = true;
            SummonAnimation = MagicAnimationPack.Walk;

            switch (LevelDimention)
            {
                case 0: SummonX = SpellRandomizer.Next(20, LevelWidth - 60); SummonY = LevelHeight - 130; break;
                case 1: SummonX = LevelWidth - 10; SummonY = SpellRandomizer.Next(20, LevelWidth - 60); break;
                case 2: SummonX = SpellRandomizer.Next(20, LevelWidth - 60); SummonY = 20; break;
                case 3: SummonX = 60; SummonY = SpellRandomizer.Next(20, LevelWidth - 60); break;
                default: break;
            }
        }

        private void CollapseGravity()
        {
            var randomGravityDistortion = SpellRandomizer.Next(0,3);

            IsSummoned = false;

            switch (randomGravityDistortion)
            {
                case 0: FutureDimention = 0; break;
                case 1: FutureDimention = 1; break;
                case 2: FutureDimention = 2; break;
                case 3: FutureDimention = 3; break;
                default: break;
            }
        }

        private void SelectRandomSpell()
        {
            IsAttacked = true;
            switch (SpellRandomizer.Next(0, 9))
            {
                case 0: 
                case 3: 
                case 5: CollapseGravity();  break;
                case 1: 
                case 4:
                case 7: 
                case 9: SummonRune();       break;
                case 2: 
                case 6:
                case 8: SummonFist();       break; 
                default:                    break;
            }
        }
        
        private void UpdateSummonFistVelocity()
        {
            var playerScoreInc = PlayerScore;

            if (SummonVelocity < 0)
                playerScoreInc = -PlayerScore;

            switch (LevelDimention)
            {
                case 0:
                case 2:
                    if (SummonVelocity > 0)
                    {
                        if (SummonX < LevelWidth)
                            SummonX += SummonVelocity + playerScoreInc / 100;
                        else
                            IsSummoned = false;
                    }
                    else
                    {
                        if (SummonX > 0)
                            SummonX += SummonVelocity + playerScoreInc / 100;
                        else
                            IsSummoned = false;
                    }
                    break;
                case 1:
                case 3:
                    if (SummonVelocity > 0)
                    {
                        if (SummonY < LevelHeight)
                            SummonY += SummonVelocity + playerScoreInc / 100;
                        else
                            IsSummoned = false;
                    }
                    else
                    {
                        if (SummonY > 0)
                            SummonY += SummonVelocity + playerScoreInc / 100;
                        else
                            IsSummoned = false;
                    }
                    break;
                default: break;
            }
        }

        public override void Update(GameTime gameTime)
        { 
            if (!IsDead)
            {
                // Update body animation
                IsOnceAnimated = CurrentAnimation.UpdateSingleAnimationIsEnded(gameTime);

                //Update summon/strike animation
                if (SummonAnimation == MagicAnimationPack.Walk && IsSummoned)
                    IsSummoned = SummonAnimation.UpdateSingleAnimationIsEnded(gameTime);
                else if(SummonAnimation == AnimationsPack.StrikeOne)
                {
                    SummonAnimation.UpdateSingleAnimationIsEnded(gameTime);
                    UpdateSummonFistVelocity();
                }

                if (IsSpawned)
                {
                    if(!IsAttacked) 
                        SelectRandomSpell();
                     
                    if (IsAlive && !IsSummoned && (DateTime.Now - StrikeTimeStart).TotalSeconds > StrikeTimeInSeconds)
                        IsAlive = false;

                    if (TransparentPower < 0.1f)
                    {
                        LevelDimention = FutureDimention;
                        IsSpawned = false;
                        IsAttacked = false;
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
                case 0: rotationAngle = .0f;                                                    break; 
                case 1: rotationAngle = 1.571f;  effectsApplyed = SpriteEffects.FlipVertically; break; 
                case 2: rotationAngle = .0f;  effectsApplyed = SpriteEffects.FlipVertically;    break; 
                case 3: rotationAngle = 1.571f;                                                 break; 
                default: break;
            }

            if (!IsDead)
            {
                var eyeWhiteX = 0f;
                var eyeWhiteY = 0f;
                var eyeRedX = 0f;
                var eyeRedY = 0f;
                var eyeBlackX = 0f;
                var eyeBlackY = 0f; 

                var eyeExtraX = 0f;
                var eyeExtraY = 0f;

                // Choose Eye coordinates, depend on dimention
                switch (LevelDimention)
                {
                    case 0:
                        eyeWhiteX = X + 48;
                        eyeWhiteY = Y + 80 - CurrentAnimation.CurrentRectangle.Y;
                        eyeRedX = 12;
                        eyeRedY = 15;
                        eyeBlackX = 20;
                        eyeBlackY = 23;
                        if (X - PlayerX > 0)
                        {
                            eyeBlackX--;
                            eyeExtraX = -1;
                        }
                        else if (X - PlayerX < -70)
                        {
                            eyeBlackX++;
                            eyeExtraX = 1;
                        }
                        if (Y - PlayerY > 0)
                        {
                            eyeBlackY--;
                            eyeExtraY = -1;
                        }
                        else if (Y - PlayerY < -70)
                        {
                            eyeBlackY++;
                            eyeExtraY = 1;
                        } 
                        break;

                    case 1:
                        eyeWhiteX = X - 43 - CurrentAnimation.CurrentRectangle.Y;
                        eyeWhiteY = Y + 46;
                        eyeRedX = -4;
                        eyeRedY = 14;
                        eyeBlackX = -11;
                        eyeBlackY = 21;
                        if (X - PlayerX > 50)
                        {
                            eyeBlackX--;
                            eyeExtraX = -1;
                        }
                        else if (X - PlayerX < -150)
                        {
                            eyeBlackX++;
                            eyeExtraX = 1;
                        }
                        if (Y - PlayerY > 50)
                        {
                            eyeBlackY--;
                            eyeExtraY = -1;
                        }
                        else if (Y - PlayerY < -150)
                        {
                            eyeBlackY++;
                            eyeExtraY = 1;
                        } 
                        break;

                    case 2:
                        eyeWhiteX = X + 45;
                        eyeWhiteY = Y + 43 + CurrentAnimation.CurrentRectangle.Y;
                        eyeRedX = 16;
                        eyeRedY = 6;
                        eyeBlackX = 23;
                        eyeBlackY = 12;
                        if (X - PlayerX > 50)
                        {
                            eyeBlackX--;
                            eyeExtraX = -1;
                        }
                        else if(X - PlayerX < -150)
                        {
                            eyeBlackX++;
                            eyeExtraX = 1;
                        }
                        if (Y - PlayerY > 150)
                        {
                            eyeBlackY--;
                            eyeExtraY = -1;
                        }
                        else if(Y - PlayerY < -50)
                        {
                            eyeBlackY++;
                            eyeExtraY = 1;
                        } 
                        break;

                    case 3:
                        eyeWhiteX = X - 80 + CurrentAnimation.CurrentRectangle.Y;
                        eyeWhiteY = Y + 46;
                        eyeRedX = -12;
                        eyeRedY = 14;
                        eyeBlackX = -22;
                        eyeBlackY = 21;
                        if (X - PlayerX > 150)
                        {
                            eyeBlackX--;
                            eyeExtraX = -1;
                        }
                        else if(X - PlayerX < -50)
                        {
                            eyeBlackX++;
                            eyeExtraX = 1;
                        }
                        
                        if (Y - PlayerY > 50)
                        {
                            eyeBlackY--;
                            eyeExtraY = -1;
                        }
                        else if (Y - PlayerY < -50)
                        {
                            eyeBlackY++;
                            eyeExtraY = 1;
                        } 
                        break;

                    default:
                        break;
                }

                // Boss white eye 
                batcher.Draw(EntitySprite, new Vector2(eyeWhiteX, eyeWhiteY), 
                                                        AnimationsPack.EyeWhitePart, TintColor * TransparentPower,
                                                            rotationAngle, new Vector2(1, 1), .7f, effectsApplyed, .0f);

                // Boss red eye
                batcher.Draw(EntitySprite, new Vector2(eyeWhiteX + eyeRedX + eyeExtraX, eyeWhiteY + eyeRedY + eyeExtraY), 
                                                            AnimationsPack.EyeRedPart, TintColor * TransparentPower,
                                                                rotationAngle, new Vector2(1, 1), .7f, effectsApplyed, .0f);


                // Boss black eye
                batcher.Draw(EntitySprite, new Vector2(eyeWhiteX + eyeBlackX + eyeExtraX, eyeWhiteY + eyeBlackY + eyeExtraY), 
                                                            AnimationsPack.EyeBlackPart, TintColor * TransparentPower, 
                                                                rotationAngle, new Vector2(1, 1), .7f, effectsApplyed, .0f);

                // Boss body
                if (!IntroFlag)
                    batcher.Draw(EntitySprite, new Vector2(X, Y), CurrentAnimation.CurrentRectangle, TintColor * TransparentPower,
                                                rotationAngle, new Vector2(1, 1), .7f, effectsApplyed, .0f);

                

                if(IsSummoned)
                {
                    if (SummonAnimation == MagicAnimationPack.Walk)
                        batcher.Draw(MagicAnimationPack.MagicSprite, new Vector2(SummonX, SummonY),
                                                            SummonAnimation.CurrentRectangle, TintColor * TransparentPower,
                                                                    rotationAngle, new Vector2(1, 1), .7f, effectsApplyed, .0f);
                    else
                    {
                        if (!SummonDirection)
                            effectsApplyed = effectsApplyed | SpriteEffects.FlipHorizontally;

                        batcher.Draw(EntitySprite, new Vector2(SummonX, SummonY),
                                                            SummonAnimation.CurrentRectangle, TintColor * TransparentPower,
                                                                    rotationAngle, new Vector2(1, 1), .7f, effectsApplyed, .0f);
                    } 
                }

                if (!IsAlive) 
                    TransparentPower -= TransparentInc; 
            } 
        }
    }
}
