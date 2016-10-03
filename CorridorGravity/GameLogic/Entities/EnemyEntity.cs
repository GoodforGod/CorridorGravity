﻿using System; 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorridorGravity.GameLogic;
using Microsoft.Xna.Framework.Content;
using CorridorGravity.GameLogic.AnimatedEntity;

namespace CorridorGravity.GameLogic
{
    class EnemyEntity : Entity
    {
        private static Color TintColor = Color.White;
        public DateTime LastHitTime = DateTime.Now;

        public override float X { get; set; }
        public override float Y { get; set; }

        public float PlayerX { get; set; }
        public float PlayerY { get; set; }

        public override Texture2D EntitySprite { get; }
        public Animation CurrentAnimation { get; set; }
        public Enemy AnimationsPack { get; }
        private Magic MagicAnimationPack;

        private bool IsOnceAnimated;
        private int OnceAnimationType = -1; 
        private float TransparentPower = 1f;

        public int EntityHeight { get; }
        public int EntityWidth { get; }
        private const int LEVEL_OFFSET_HEIGHT = 20;
        private const int DISTANCE_BETWEEN_ENTITIES = 105;
        private const int SCORE_VELOCITY_LIMIT = 10;

        private const float JUMP_POWER = 450f;
        private const float VELOCITY_AIR_LIMIT_X_AXIS = 40f;
        private const float VELOCITY_LIMIT_X_AXIS = 205f;
        private const float VELOCITY_LIMIT_Y_AXIS = 180f;
        private const float ACCELERATION_AIR_X_AXIS = 3f;
        private const float ACCELERATION_X_AXIS = 85f;
        private const float GRAVITY_POWER = -9.8f; 

        private float SlowDownLimit = 0.001f;
        private float VelocityAxisY = 0;
        private float VelocityAxisX = 0; 

        public bool EntityDirection { get; set; }           // Direction of animation, false - Animation direction right, true - left
        public bool IsAlive { get; set; }
        public bool IsDead { get; set; }
        public bool IsAttacked { get; set; }
        public float RotationAngle = .0f;
        public int PlayerScore { get; set; }

        private int LevelHeight { get; }
        private int LevelWidth { get; }
        public int LevelDirection { get; set; }       // 1 - Correct direction, -1 - inverse
        public int LevelDimention { get; set; }       // 0 - Ground=Ground, 
                                                      // 1 - RightWall=Ground, 
                                                      // 2 - Top=Ground,
                                                      // 3 - LeftWall=Ground.

        public EnemyEntity(ContentManager content, string contentName, int levelHeight, int levelWidth, bool EntityDirection, float X, float Y)
        {
            EntitySprite = content.Load<Texture2D>(contentName);
            MagicAnimationPack = new Magic(content);
            AnimationsPack = new Enemy();
            CurrentAnimation = AnimationsPack.Idle;

            LevelHeight = levelHeight - LEVEL_OFFSET_HEIGHT;
            LevelWidth = levelWidth;
            LevelDimention = 0;
            LevelDirection = 1;

            EntityHeight = 72;
            EntityWidth = 44;
            PlayerScore = 1;
            IsAlive = true;
            this.Y = Y;
            this.X = X;
            this.EntityDirection = EntityDirection;
        }

        public bool GetEnemyStrikeStatus()
        {
            if (OnceAnimationType == 0 || OnceAnimationType == 2)
                return true;
            else return false;
        }

        private void IncreaseVelocityRight()                                // If right key down & speed not max, then accelerate
        {
            if (IsGrounded())
            {
                switch (LevelDimention)
                {
                    case 0:
                    case 2:
                        if (VelocityAxisX < VELOCITY_LIMIT_X_AXIS + PlayerScore / SCORE_VELOCITY_LIMIT)
                            VelocityAxisX += ACCELERATION_X_AXIS; 
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY > -VELOCITY_LIMIT_X_AXIS - PlayerScore / SCORE_VELOCITY_LIMIT)
                            VelocityAxisY -= ACCELERATION_X_AXIS; 
                        break;
                    default: break;
                }

            }
            else
            {
                switch (LevelDimention)
                {
                    case 0:
                    case 2:
                        if (VelocityAxisX < VELOCITY_AIR_LIMIT_X_AXIS + PlayerScore / SCORE_VELOCITY_LIMIT)
                            VelocityAxisX += ACCELERATION_AIR_X_AXIS; 
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY > -VELOCITY_AIR_LIMIT_X_AXIS - PlayerScore / SCORE_VELOCITY_LIMIT)
                            VelocityAxisY -= ACCELERATION_AIR_X_AXIS; 
                        break;
                    default: break;
                }
            }
        }

        private void IncreaseVelocityLeft()                                 // If left key down & speed not max, then accelerate
        {
            if (IsGrounded())
            {
                switch (LevelDimention)
                {
                    case 0:
                    case 2:
                        if (VelocityAxisX > -VELOCITY_LIMIT_X_AXIS - PlayerScore / SCORE_VELOCITY_LIMIT)
                            VelocityAxisX -= ACCELERATION_X_AXIS; 
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY < VELOCITY_LIMIT_X_AXIS + PlayerScore / SCORE_VELOCITY_LIMIT)
                            VelocityAxisY += ACCELERATION_X_AXIS; 
                        break;
                    default: break;
                }

            }
            else
            {
                switch (LevelDimention)
                {
                    case 0:
                    case 2:
                        if (VelocityAxisX > -VELOCITY_AIR_LIMIT_X_AXIS - PlayerScore / SCORE_VELOCITY_LIMIT)
                            VelocityAxisX -= ACCELERATION_AIR_X_AXIS; 
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY < VELOCITY_AIR_LIMIT_X_AXIS + PlayerScore / SCORE_VELOCITY_LIMIT)
                            VelocityAxisY += ACCELERATION_AIR_X_AXIS; 
                        break;

                    default: break;
                }
            }
        }

        private void SlowVelocityRight()                                        //Slow down acceleration
        {
            if ((LevelDimention == 0 || LevelDimention == 2) && VelocityAxisX > SlowDownLimit)
            {
                VelocityAxisX -= ACCELERATION_X_AXIS;
                if (VelocityAxisX < 0)
                    VelocityAxisX = 0; 
            }
            else if (VelocityAxisY < -SlowDownLimit)
            {
                VelocityAxisY += ACCELERATION_X_AXIS;
                if (VelocityAxisY > 0)
                    VelocityAxisY = 0; 
            }
        }

        private void SlowVelocityLeft()                                         // Slow down acceleration
        {
            if ((LevelDimention == 0 || LevelDimention == 2) && VelocityAxisX < -SlowDownLimit)
            {
                VelocityAxisX += ACCELERATION_X_AXIS;
                if (VelocityAxisX > 0)
                    VelocityAxisX = 0; 
            }
            else if (VelocityAxisY > SlowDownLimit)
            {
                VelocityAxisY -= ACCELERATION_X_AXIS;
                if (VelocityAxisY < 0)
                    VelocityAxisY = 0; 
            }
        }

        private void SetStrikeAnimationType(int AnimationType)
        {
            var dimentionVelocity = 0f;
            IsOnceAnimated = true; 

            switch (LevelDimention)
            {
                case 0: dimentionVelocity = VelocityAxisY; break;
                case 1: dimentionVelocity = VelocityAxisX; break;
                case 2: dimentionVelocity = -VelocityAxisY; break;
                case 3: dimentionVelocity = -VelocityAxisX; break;
                default: break;
            }
            if (dimentionVelocity < 0)
                OnceAnimationType = 2;
            else if (!IsGrounded())
                OnceAnimationType = 2;
            else OnceAnimationType = AnimationType;
        }

        private void UpdateVelocityBasedOnDirection()                           // Update velocity via input & check for gravity collapse
        {
            var rightState = false;
            var leftState = false; 
            var between = 0f;

            if (EntityDirection)
                rightState = true; 
            else
                leftState = true;

            if (!IsOnceAnimated)
            {
                switch (LevelDimention)
                {
                    case 0:
                    case 2: between = PlayerX - X;  break; 
                    case 1:
                    case 3: between = Y - PlayerY;  break;
                    default:                        break;
                }

                if (EntityDirection)
                {
                    if (between > 0 && between < DISTANCE_BETWEEN_ENTITIES)
                    { 
                        switch(LevelDimention)
                        {
                            case 0: VelocityAxisY -= VELOCITY_LIMIT_X_AXIS;  break;
                            case 1: VelocityAxisX -= VELOCITY_LIMIT_X_AXIS;  break;
                            case 2: VelocityAxisY += VELOCITY_LIMIT_X_AXIS;  break;
                            case 3: VelocityAxisX += VELOCITY_LIMIT_X_AXIS;  break;
                            default:                                         break;
                        }
                        SetStrikeAnimationType(0); 
                    }
                }
                else if (between < 0 && between > -DISTANCE_BETWEEN_ENTITIES)
                {
                    switch (LevelDimention)
                    {
                        case 0: VelocityAxisY -= VELOCITY_LIMIT_X_AXIS; break;
                        case 1: VelocityAxisX -= VELOCITY_LIMIT_X_AXIS; break;
                        case 2: VelocityAxisY += VELOCITY_LIMIT_X_AXIS; break;
                        case 3: VelocityAxisX += VELOCITY_LIMIT_X_AXIS; break;
                        default: break;
                    }
                    SetStrikeAnimationType(0); 
                }
            }
            
            if (rightState)                                                                     // Right Acceleration
            {
                if (LevelDirection == 1)       // if InverseAxisDirections was applyed
                    IncreaseVelocityRight();
                else
                    IncreaseVelocityLeft();
            }

            if (leftState)                                                                       // Left Acceleration
            {
                if (LevelDirection == 1)       // if InverseAxisDirections was applyed
                    IncreaseVelocityLeft();
                else
                    IncreaseVelocityRight();
            }

            if (IsGrounded() && !rightState)                                                     // Right SlowDown
            {
                if (LevelDirection == 1)
                    SlowVelocityRight();
                else
                    SlowVelocityLeft();
            }

            if (IsGrounded() && !leftState)                                                      // Left SlowDown
            {
                if (LevelDirection == 1)
                    SlowVelocityLeft();
                else
                    SlowVelocityRight();
            } 

            if (!IsGrounded())                                                                   // Gravity activater in air
            {
                switch (LevelDimention)
                {
                    case 0: VelocityAxisY -= GRAVITY_POWER; break;
                    case 1: VelocityAxisX -= GRAVITY_POWER; break;
                    case 2: VelocityAxisY += GRAVITY_POWER; break;
                    case 3: VelocityAxisX += GRAVITY_POWER; break;
                    default: break;
                }
            }
            else if (IsGrounded())                                                             // On ground, failsave from 
            {
                switch (LevelDimention)
                {
                    case 0:
                        if (VelocityAxisY > 0)                     // If jump key down & on the ground
                            VelocityAxisY = 0;
                        break;
                    case 1:
                        if (VelocityAxisX > 0)                     // If jump key down & on the ground
                            VelocityAxisX = 0;
                        break;
                    case 2:
                        if (VelocityAxisY < 0)                     // If jump key down & on the ground
                            VelocityAxisY = 0;
                        break;
                    case 3:
                        if (VelocityAxisX < 0)                     // If jump key down & on the ground
                            VelocityAxisX = 0;
                        break;
                    default: break;
                }
            }
        }

        private void UpdateCoordinatesBasedOnVelocity(GameTime gameTime)        // Updates entitys coordinates
        {
            X += VelocityAxisX * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Y += VelocityAxisY * (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (LevelDimention)                    // Restore player coordinates, if out of frame, depends on Dimention
            {
                case 0: CoordinatesInFrameGround();     break;
                case 1: CoordinatesInFrameWall();       break;
                case 2: CoordinatesInFrameGround();     break;
                case 3: CoordinatesInFrameWall();       break;
                default: CoordinatesInFrameGround();    break;
            }
        }

        private void UpdateAnimationBasedOnVelocity()                           // Update current animation
        {
            if (IsOnceAnimated)
            {
                switch (OnceAnimationType)
                {
                    case 0: CurrentAnimation = AnimationsPack.StrikeOne;    break; 
                    case 2: CurrentAnimation = AnimationsPack.JumpStrike;   break;
                    case 3: CurrentAnimation = AnimationsPack.Celebrate;    break;
                    case 4: CurrentAnimation = MagicAnimationPack.Dead;     break;
                    default:                                                break;
                }
            }
            else if (VelocityAxisX != 0 || VelocityAxisY != 0)
            {
                var dimentionVelocity = VelocityAxisX;
                if (LevelDimention == 0 || LevelDimention == 2)
                    dimentionVelocity = VelocityAxisY;

                if (dimentionVelocity > 0)
                    CurrentAnimation = AnimationsPack.Jump;
                else if (dimentionVelocity < 0)
                    CurrentAnimation = AnimationsPack.Jump;
                else if (IsGrounded())
                    CurrentAnimation = AnimationsPack.Walk;
            }
            else if (CurrentAnimation != AnimationsPack.Idle)
                CurrentAnimation = AnimationsPack.Idle;
        }

        private void CoordinatesInFrameWall()                                   // Case dimention is one of the walls
        {
            if (X - EntityHeight < 0)
            {                                        // X position fall out frame
                X = EntityHeight;
                VelocityAxisX = 0;
            }
            else if (X - LEVEL_OFFSET_HEIGHT > LevelWidth)
            {
                if (LevelDimention == 1)
                {
                    X = LevelWidth - EntityWidth;
                    VelocityAxisX = 0;
                }
                else
                {
                    X = LevelWidth;
                    VelocityAxisX = 0;
                }
            }

            if (Y < 0)
            {
                EntityDirection = false;
                Y = VelocityAxisY = 0;
            }
            else if (Y + EntityWidth >= LevelHeight)
            {         // Y position fall out frame 
                EntityDirection = true;
                Y = LevelHeight - EntityWidth;         // Velocity = 0;
                VelocityAxisY = 0;
            }
        }

        private void CoordinatesInFrameGround()                                 // Case dimention is ground or floor
        {
            if (X < 0)                                          // X position fall out frame
            {
                EntityDirection = true;
                X = VelocityAxisX = 0; 
            }
            else if (X + EntityWidth > LevelWidth)
            {
                EntityDirection = false;
                X = LevelWidth - EntityWidth;
                VelocityAxisX = 0;
            }

            if (Y < 0) 
                Y = VelocityAxisY = 0;  
            else if (Y + EntityHeight >= LevelHeight)          // Y position fall out frame
            {
                Y = LevelHeight - EntityHeight;         // Velocity = 0;
                VelocityAxisY = 0;
            }
        }

        private bool IsGrounded()                                               // Check if the entity is grounded
        {
            var offset = 1;

            switch (LevelDimention)
            {
                case 0:
                    if (Y + EntityHeight + offset >= LevelHeight) 
                        return true; 
                    else return false; 

                case 1:
                    if (X + LEVEL_OFFSET_HEIGHT - offset * 2 >= LevelWidth) 
                        return true; 
                    else return false;

                case 2:
                    if (Y + offset <= LEVEL_OFFSET_HEIGHT) 
                        return true; 
                    else return false;

                case 3:
                    if (X + offset <= LEVEL_OFFSET_HEIGHT * 4) 
                        return true; 
                    else return false; 
                default: return true;
            }
        }  

        public override void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                UpdateVelocityBasedOnDirection();

                UpdateCoordinatesBasedOnVelocity(gameTime);
            }
            else if (OnceAnimationType != 4)
            {
                IsOnceAnimated = true;
                OnceAnimationType = 4;
            } 

            UpdateAnimationBasedOnVelocity();

            if (!IsOnceAnimated)
                CurrentAnimation.UpdateCycleAnimation(gameTime); 
            else
            {
                if (!IsAlive && TransparentPower > 0.1f)
                    TransparentPower -= .01f;
                if (!IsDead)
                    IsOnceAnimated = CurrentAnimation.UpdateSingleAnimationIsEnded(gameTime);
                if (!IsOnceAnimated)
                    OnceAnimationType = -1;
                if (!IsAlive && OnceAnimationType == -1)
                    IsDead = true;
            }  
        }

        private SpriteEffects AdjustSpriteEffect()
        {
            switch (LevelDimention)                             // Choose sprite flip, depend on the current dimention
            {
                case 0:
                    if (!EntityDirection)
                        return SpriteEffects.FlipHorizontally;
                    else return SpriteEffects.None; 

                case 1:
                    if (EntityDirection)
                        return SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    else return SpriteEffects.FlipVertically; 

                case 2:
                    if (!EntityDirection)
                        return SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    else return SpriteEffects.FlipVertically; 

                case 3:
                    if (EntityDirection)
                        return SpriteEffects.FlipHorizontally;
                    else return SpriteEffects.None; 

                default: return SpriteEffects.None;
            }
        }

        public override void Draw(SpriteBatch batcher)
        {
            SpriteEffects effectsApplyed = AdjustSpriteEffect();
            
            if (IsDead) 
                batcher.Draw(EntitySprite, new Vector2(X, Y), new Rectangle(0, 0, 1, 1), TintColor); 
            else if (OnceAnimationType == 4) 
                batcher.Draw(MagicAnimationPack.MagicSprite, new Vector2(X, Y), CurrentAnimation.CurrentRectangle, 
                                 TintColor * TransparentPower, RotationAngle, new Vector2(1, 1), 1f, effectsApplyed, .0f); 
            else if (IsAlive)
                batcher.Draw(EntitySprite, new Vector2(X, Y), CurrentAnimation.CurrentRectangle, TintColor,
                                            RotationAngle, new Vector2(1, 1), 1f, effectsApplyed, .0f); 
        } 
    }
}