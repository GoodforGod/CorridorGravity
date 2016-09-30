using System;  
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorridorGravity.GameLogic;
using Microsoft.Xna.Framework.Content;
using CorridorGravity.GameLogic.AnimatedEntity;

namespace CorridorGravity.GameLogic
{
    class PlayerEntity : Entity
    {
        private static Color TintColor = Color.White;

        public override float X { get; set; }
        public override float Y { get; set; }

        public SpriteFont ScoreFont { get; }
        public override Texture2D EntitySprite { get; }
        public Animation CurrentAnimation { get; set; }
        private Bob AnimationsPack;
        public DateTime LastHitTime;

        public const int ENTITY_HEIGHT = 90;
        public const int ENTITY_WIDTH = 44;
        private const int LEVEL_OFFSET_HEIGHT = 20;
        private const int SCORE_PER_ENEMY = 10;

        private const float JUMP_POWER = 350f;
        private const float VELOCITY_AIR_LIMIT_X_AXIS = 40f;
        private const float VELOCITY_LIMIT_X_AXIS = 235f;
        private const float VELOCITY_LIMIT_Y_AXIS = 180f;
        private const float ACCELERATION_AIR_X_AXIS = 3f;
        private const float ACCELERATION_X_AXIS = 85f;
        private const float GRAVITY_POWER = -9.8f;
        private const float ROTATION_ANGLE_LIMIT = 1.570f;

        public bool IsAlive { get; set; }
        public bool IsDead { get; set; }
        private bool IsOnceAnimated { get; set; }
        public bool IsOnSrike { get; set; }
         
        public float RotationAngleInc = 0.157f;
        public float RotationAngle = .0f;
        public long ScoreCount { get; set; }
        public int HealthCount { get; set; }
        private int DoubleJumpFlag = 0;
        private int OnceAnimationType = -1; 
        private float SlowDownLimit = 0.001f;
        private float VelocityAxisY = 0;
        private float VelocityAxisX = 0;
        private int accum = -1;
         
        public bool EntityDirection { get; set; }   // Direction of animation, false - Animation direction right, true - left
        private int LevelHeight { get; set; }
        private int LevelWidth { get; set; } 
        public int LevelDirection { get; set; }    // 1 - Correct direction, -1 - inverse
        public int LevelDimention { get; set; }    // 0 - Ground=Ground, 
                                                    // 1 - RightWall=Ground, 
                                                    // 2 - Top=Ground,
                                                    // 3 - LeftWall=Ground. 

        public PlayerEntity(ContentManager content, int levelHeight, int levelWidth)
        {
            ScoreFont = content.Load<SpriteFont>("score-font");
            EntitySprite = content.Load<Texture2D>("player-2-white-1");
            ConstractCommonParts(levelHeight, levelWidth);
        }

        public PlayerEntity(ContentManager content, string contentName, int levelHeight, int levelWidth)
        {
            ScoreFont = content.Load<SpriteFont>("score-font");
            EntitySprite = content.Load<Texture2D>(contentName);
            ConstractCommonParts(levelHeight, levelWidth);
        }

        private void ConstractCommonParts(int levelHeight, int levelWidth)
        {
            LevelHeight = levelHeight - LEVEL_OFFSET_HEIGHT;
            LevelWidth = levelWidth;
            LevelDirection = 1;
            LevelDimention = 0;
            LastHitTime = new DateTime();

            Y = LevelHeight - ENTITY_HEIGHT - 1;
            X = LevelWidth / 2;
            ScoreCount = 0;
            HealthCount = 6;
            IsAlive = true;
            AnimationsPack = new Bob();
            CurrentAnimation = AnimationsPack.Celebrate;
            OnceAnimationType = 3;
            IsOnceAnimated = true;
        }

        public override void Init()
        { 

        }
         
        public bool GetPLayerStrikeStatus()
        {
            if (OnceAnimationType == 0 || OnceAnimationType == 1 || OnceAnimationType == 2)
                return true;
            else return false;
        }

        public int GetScorePerEnemy() { return SCORE_PER_ENEMY; }

        public int GetEntityWidth()  { return CurrentAnimation.CurrentRectangle.Width; }

        public int GetEntityHeight() { return CurrentAnimation.CurrentRectangle.Height;  }

        private void IncreaseVelocityRight(int limit)                // If right key down & speed not max, then accelerate
        {
            if (IsGrounded())
            {
                switch (LevelDimention)
                {
                    case 0:
                    case 2:
                        if (VelocityAxisX < VELOCITY_LIMIT_X_AXIS - limit)
                            VelocityAxisX += ACCELERATION_X_AXIS;
                        EntityDirection = false;
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY > -VELOCITY_LIMIT_X_AXIS + limit)
                            VelocityAxisY -= ACCELERATION_X_AXIS;
                        EntityDirection = true;
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
                        if (VelocityAxisX < VELOCITY_AIR_LIMIT_X_AXIS - limit)
                            VelocityAxisX += ACCELERATION_AIR_X_AXIS;
                        EntityDirection = false;
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY > -VELOCITY_AIR_LIMIT_X_AXIS + limit)
                            VelocityAxisY -= ACCELERATION_AIR_X_AXIS;
                        EntityDirection = true;
                        break;
                    default: break;
                }
            }
        }

        private void IncreaseVelocityLeft(int limit)                 // If left key down & speed not max, then accelerate
        {
            if (IsGrounded())
            {
                switch (LevelDimention)
                {
                    case 0:
                    case 2:
                        if (VelocityAxisX > -VELOCITY_LIMIT_X_AXIS + limit)
                            VelocityAxisX -= ACCELERATION_X_AXIS;
                        EntityDirection = true;
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY < VELOCITY_LIMIT_X_AXIS - limit)
                            VelocityAxisY += ACCELERATION_X_AXIS;
                        EntityDirection = false;
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
                        if (VelocityAxisX > -VELOCITY_AIR_LIMIT_X_AXIS + limit)
                            VelocityAxisX -= ACCELERATION_AIR_X_AXIS;
                        EntityDirection = true;
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY < VELOCITY_AIR_LIMIT_X_AXIS - limit)
                            VelocityAxisY += ACCELERATION_AIR_X_AXIS;
                        EntityDirection = false;
                        break;

                    default: break;
                }
            }
        }

        private void SlowVelocityRight(int limit)                    //Slow down acceleration
        {
            if ((LevelDimention == 0 || LevelDimention == 2) && VelocityAxisX > SlowDownLimit)
            {
                if(VelocityAxisX > limit)
                    VelocityAxisX -= ACCELERATION_X_AXIS;
                if (VelocityAxisX < 0)
                    VelocityAxisX = 0;
                EntityDirection = false;
            }
            else if (VelocityAxisY < -SlowDownLimit)
            {
                if(VelocityAxisY < -limit)
                VelocityAxisY += ACCELERATION_X_AXIS;
                if (VelocityAxisY > 0)
                    VelocityAxisY = 0;
                EntityDirection = true;
            }
        }

        private void SlowVelocityLeft(int limit)                     // Slow down acceleration
        {
            if ((LevelDimention == 0 || LevelDimention == 2) && VelocityAxisX < -SlowDownLimit)
            {
                if(VelocityAxisX < -limit)
                    VelocityAxisX += ACCELERATION_X_AXIS;
                if (VelocityAxisX > 0)
                    VelocityAxisX = 0;
                EntityDirection = true;
            }
            else if (VelocityAxisY > SlowDownLimit)
            {
                if(VelocityAxisY > limit)
                    VelocityAxisY -= ACCELERATION_X_AXIS;
                if (VelocityAxisY < 0)
                    VelocityAxisY = 0;
                EntityDirection = false;
            }
        }

        private void SetStrikeAnimationType(int AnimationType)
        {
            var dimentionVelocity = 0f;
            IsOnceAnimated = true;
            IsOnSrike = true;

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

        public void TestGravityCollapce()
        { 
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.C) && accum == -1 && IsAlive)           // RightWall
            {
                accum = 1;
                CollapseGravity(accum);
            }
            if (state.IsKeyUp(Keys.C) && accum == 1 && IsAlive)
                accum = -1;

            if (state.IsKeyDown(Keys.Space) && accum == -1 && IsAlive)       // Floor
            {
                accum = 2;
                CollapseGravity(accum);
            }
            if (state.IsKeyUp(Keys.Space) && accum == 2 && IsAlive)
                accum = -1;

            if (state.IsKeyDown(Keys.V) && accum == -1 && IsAlive)           // LeftWall
            {
                accum = 3;
                CollapseGravity(accum);
            }
            if (state.IsKeyUp(Keys.V) && accum == 3 && IsAlive)
                accum = -1;

            if (state.IsKeyDown(Keys.Z) && accum == -1 && IsAlive)           // Ground
            {
                accum = 0;
                CollapseGravity(accum);
            }
            if (state.IsKeyUp(Keys.Z) && accum == 0)
                accum = -1;
        }

        private void UpdateVelocityBasedOnInput()              // Update velocity via input & check for gravity collapse
        {
            var rightState = false;
            var leftState = false;

            KeyboardState state = Keyboard.GetState(); 

            if (state.IsKeyUp(Keys.Q) && state.IsKeyUp(Keys.E) && !IsOnceAnimated && IsAlive && IsOnSrike) 
                IsOnSrike = false;  

            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D) && IsAlive)
                rightState = true;
            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A) && IsAlive)
                leftState = true;
            if (state.IsKeyDown(Keys.Q) && !IsOnceAnimated && IsAlive && !IsOnSrike) 
                SetStrikeAnimationType(0); 
            if (state.IsKeyDown(Keys.E) && !IsOnceAnimated && IsAlive && !IsOnSrike) 
                SetStrikeAnimationType(1);

            // Test code section BEGIN ####

            //TestGravityCollapce();

            // #########   END  ###########

            // Slow during attack
            var velocityLimit = 0;
            var slowStrikeFlag = GetPLayerStrikeStatus();

            if (slowStrikeFlag && IsGrounded())
                velocityLimit = (int)VELOCITY_LIMIT_X_AXIS - 120;

            if (rightState && IsAlive)                                                                         // Right Acceleration
            { 
                if (LevelDirection == 1)                // if InverseAxisDirections was applyed
                    IncreaseVelocityRight(velocityLimit);
                else
                    IncreaseVelocityLeft(velocityLimit);

            }
            if (leftState && IsAlive)                                                                          // Left Acceleration
            { 
                if (LevelDirection == 1)                // if InverseAxisDirections was applyed
                    IncreaseVelocityLeft(velocityLimit);
                else
                    IncreaseVelocityRight(velocityLimit);

            }
            if (IsGrounded() && (!rightState || slowStrikeFlag))                                                // Right SlowDown
            {
                if (LevelDirection == 1)
                    SlowVelocityRight(velocityLimit);
                else
                    SlowVelocityLeft(velocityLimit);
            }
            if (IsGrounded() && (!leftState || slowStrikeFlag))                                                 // Left SlowDown
            {
                if (LevelDirection == 1)
                    SlowVelocityLeft(velocityLimit);
                else
                    SlowVelocityRight(velocityLimit);
            }

            var dimentionVelocity = 0f;                         // Velocity to check, is could jump or.. "the gravity is too strong in this one"
            switch(LevelDimention)
            {
                case 0: dimentionVelocity = VelocityAxisY; break;
                case 1: dimentionVelocity = VelocityAxisX; break;
                case 2: dimentionVelocity = -VelocityAxisY; break;
                case 3: dimentionVelocity = -VelocityAxisX; break;
                default: break;
            }

            if ((state.IsKeyUp(Keys.Up) && state.IsKeyUp(Keys.W)) & DoubleJumpFlag == 1 && IsAlive)    // Slow down velocityX after jump
            {
                DoubleJumpFlag = 2;
                switch (LevelDimention)
                {
                    case 0: break;
                    case 1: break;
                    case 2: break;
                    case 3: break;
                    default: break;
                }
            }
            else if ((state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
                                                & (DoubleJumpFlag == 0 || DoubleJumpFlag == 2) 
                                                    && dimentionVelocity < 60 && IsAlive)                // Jump power
            {
                if (DoubleJumpFlag < 2)
                    DoubleJumpFlag = 1;
                else DoubleJumpFlag = 3;

                switch (LevelDimention)                                                 // If jump key down & on the ground
                {
                    case 0: VelocityAxisY = -JUMP_POWER; break;
                    case 1: VelocityAxisX = -JUMP_POWER; break;
                    case 2: VelocityAxisY = JUMP_POWER; break;
                    case 3: VelocityAxisX = JUMP_POWER; break;
                    default: break;
                }
            }
            if (!IsGrounded())                                                           // Gravity activater in air
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
            else if (IsGrounded())                                                         // On ground, failsave from 
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
                case 0: CoordinatesInFrameGround(); break;
                case 1: CoordinatesInFrameWall(); break;
                case 2: CoordinatesInFrameGround(); break;
                case 3: CoordinatesInFrameWall(); break;
                default: CoordinatesInFrameGround(); break;
            }
        }

        private void UpdateAnimationBasedOnVelocity()                           // Update current animation
        {
            if (IsOnceAnimated)
            {
                switch (OnceAnimationType)
                {
                    case 0: CurrentAnimation = AnimationsPack.StrikeOne;    break;
                    case 1: CurrentAnimation = AnimationsPack.StrikeTwo;    break;
                    case 2: CurrentAnimation = AnimationsPack.JumpStrike;   break;
                    case 3: CurrentAnimation = AnimationsPack.Celebrate;    break;
                    case 4: CurrentAnimation = AnimationsPack.Dead;         break;
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
            if (X - ENTITY_HEIGHT < 0)                                          // X position fall out frame
            {
                X = ENTITY_HEIGHT;
                VelocityAxisX = 0;
            }
            else if (X - LEVEL_OFFSET_HEIGHT > LevelWidth)
            {
                if (LevelDimention == 1)
                {
                    X = LevelWidth - ENTITY_WIDTH;
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
                Y = 0;
                VelocityAxisY = 0;
            }
            else if (Y + ENTITY_WIDTH >= LevelHeight)          // Y position fall out frame
            {
                Y = LevelHeight - ENTITY_WIDTH;         // Velocity = 0;
                VelocityAxisY = 0;
            }
        }

        private void CoordinatesInFrameGround()                                 // Case dimention is ground or floor
        {
            if (X < 0)                                          // X position fall out frame
            {
                X = 0;
                VelocityAxisX = 0;
            }
            else if (X + ENTITY_WIDTH > LevelWidth)
            {
                X = LevelWidth - ENTITY_WIDTH;
                VelocityAxisX = 0;
            }

            if (Y < 0)
            {
                Y = 0;
                VelocityAxisY = 0;
            }
            else if (Y + ENTITY_HEIGHT >= LevelHeight)          // Y position fall out frame
            {
                Y = LevelHeight - ENTITY_HEIGHT;         // Velocity = 0;
                VelocityAxisY = 0;
            }
        }

        private bool IsGrounded()                                               // Check if the entity is grounded
        {
            var offset = 1;

            switch (LevelDimention)
            {
                case 0:
                    if (Y + ENTITY_HEIGHT + offset >= LevelHeight)
                    {
                        DoubleJumpFlag = 0;
                        return true;
                    }
                    else return false;

                case 1:
                    if (X + LEVEL_OFFSET_HEIGHT - offset * 2 >= LevelWidth)
                    {
                        DoubleJumpFlag = 0;
                        return true;
                    }
                    else return false;

                case 2:
                    if (Y + offset <= LEVEL_OFFSET_HEIGHT)
                    {
                        DoubleJumpFlag = 0;
                        return true;
                    }
                    else return false;

                case 3:
                    if (X + offset <= LEVEL_OFFSET_HEIGHT * 5)
                    {
                        DoubleJumpFlag = 0;
                        return true;
                    }
                    else return false;

                default: return true;
            }
        } 

        private void InverseAxisDirections()
        {
            LevelDirection = -LevelDirection;
        }

        public void CollapseGravity(int calledDimention)                          // Changes the dimention (gravity and physics of the game)
        {
            //var randomGravityDistortion = new Random().Next(1, 3); 
            var randomGravityDistortion = calledDimention;

            switch (randomGravityDistortion)
            {
                case 0: LevelDimention = 0; break;
                case 1: LevelDimention = 1; break;
                case 2: LevelDimention = 2; break;
                case 3: LevelDimention = 3; break;
                default: break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (HealthCount < 1)                // Make sure that health is > 0
                IsAlive = false;

            UpdateVelocityBasedOnInput();

            UpdateCoordinatesBasedOnVelocity(gameTime);

            if(!IsAlive)                        // Is got killed, set die animation
            {
                IsOnceAnimated = true;
                OnceAnimationType = 4;
            } 

            UpdateAnimationBasedOnVelocity();

            if (!IsOnceAnimated && IsAlive)                         // If alive and not onceAnim, play cycle animation
                CurrentAnimation.UpdateCycleAnimation(gameTime);
            else
            { 
                if (!IsDead)
                    IsOnceAnimated = CurrentAnimation.UpdateSingleAnimationIsEnded(gameTime);
                if (!IsOnceAnimated)
                    OnceAnimationType = -1;
                if (!IsOnceAnimated && !IsAlive) 
                    IsDead = true; 
            }
        }

        public override void Draw(SpriteBatch batcher)
        {
            SpriteEffects effectsApplyed = SpriteEffects.None; 

            switch (LevelDimention)                             // Choose sprite flip, depend on the current dimention
            {
                case 0:
                    if (RotationAngle <= ROTATION_ANGLE_LIMIT && RotationAngle > 0)
                        RotationAngle -= RotationAngleInc;
                    else RotationAngle = 0;
                    if (EntityDirection)
                        effectsApplyed = SpriteEffects.FlipHorizontally;
                    else effectsApplyed = SpriteEffects.None;
                    break;

                case 1:
                    if (RotationAngle >= 0 && RotationAngle < ROTATION_ANGLE_LIMIT)
                        RotationAngle += RotationAngleInc;
                    else RotationAngle = ROTATION_ANGLE_LIMIT;
                    if (EntityDirection)
                        effectsApplyed = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    else effectsApplyed = SpriteEffects.FlipVertically;
                    break;

                case 2:
                    if (RotationAngle <= ROTATION_ANGLE_LIMIT && RotationAngle > 0)
                        RotationAngle -= RotationAngleInc;
                    else RotationAngle = 0;
                    if (EntityDirection)
                        effectsApplyed = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    else effectsApplyed = SpriteEffects.FlipVertically;
                    break;

                case 3:
                    if (RotationAngle >= 0 && RotationAngle < ROTATION_ANGLE_LIMIT)
                        RotationAngle += RotationAngleInc;
                    else RotationAngle = ROTATION_ANGLE_LIMIT;
                    if (EntityDirection)
                        effectsApplyed = SpriteEffects.FlipHorizontally;
                    else effectsApplyed = SpriteEffects.None;
                    break;

                default: break;
            }

            var StrikeCoordX = X;
            var StrikeCoordY = Y; 
            var StepForHealthBar = 20;

            if (IsDead)
                batcher.Draw(EntitySprite, new Vector2(StrikeCoordX, StrikeCoordY), new Rectangle(430, 130, 78, 90), TintColor,
                                            RotationAngle, new Vector2(1, 1), 1f, effectsApplyed, .0f);
            else
            { 
                if (CurrentAnimation == AnimationsPack.StrikeTwo)
                {
                    switch (LevelDimention)
                    {
                        case 0: StrikeCoordY = Y - LEVEL_OFFSET_HEIGHT;   break;
                        case 1: StrikeCoordX = X - LEVEL_OFFSET_HEIGHT;   break;
                        case 2: StrikeCoordY = Y + LEVEL_OFFSET_HEIGHT/2; break;
                        case 3: StrikeCoordX = X + LEVEL_OFFSET_HEIGHT;   break; 
                        default: break;
                    }
                }
                if ((CurrentAnimation == AnimationsPack.StrikeOne || CurrentAnimation == AnimationsPack.StrikeTwo) && EntityDirection)
                {
                    switch (LevelDimention)
                    {
                        case 0: StrikeCoordX = X - (CurrentAnimation.CurrentRectangle.Width - 50); break;
                        case 1: StrikeCoordY = Y - (CurrentAnimation.CurrentRectangle.Width - 50); break;
                        case 2: StrikeCoordX = X - (CurrentAnimation.CurrentRectangle.Width - 50); break;
                        case 3: StrikeCoordY = Y - (CurrentAnimation.CurrentRectangle.Width - 50); break;
                        default: break;
                    }
                }
                // Entity draw
                batcher.Draw(EntitySprite, new Vector2(StrikeCoordX, StrikeCoordY), CurrentAnimation.CurrentRectangle, TintColor,
                                            RotationAngle, new Vector2(1, 1), 1f, effectsApplyed, .0f);
            }
            // Portrait draw
            batcher.Draw(EntitySprite, new Vector2(LevelWidth - LEVEL_OFFSET_HEIGHT * 2, LevelHeight + 10), AnimationsPack.Portrait.CurrentRectangle, Color.Beige,
                            0f, new Vector2(1, 1), 1f, SpriteEffects.None, .0f);
            
            // Health bar draw
            for(int i = 0; i < HealthCount; i++)
            { 
                batcher.Draw(EntitySprite, new Vector2(StepForHealthBar, LevelHeight), AnimationsPack.Health.CurrentRectangle, Color.Beige,
                 0f, new Vector2(1, 1), 1f, SpriteEffects.None, .0f);
                StepForHealthBar += 44;
            }

        } 
    }
} 