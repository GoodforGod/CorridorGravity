using System; 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorridorGravity.GameLogic;
using Microsoft.Xna.Framework.Content;
using CorridorGravity.GameLogic.AnimatedEntity;

namespace CorridorGravity.GameLogic
{
    class Boss : Entity
    {
        private static Color TintColor = Color.White;

        public override float X { get; set; }
        public override float Y { get; set; }

        public float PlayerX { get; set; }
        public float PlayerY { get; set; }

        public override Texture2D EntitySprite { get; }
        public Animation CurrentAnimation { get; set; }
        private Enemy AnimationsPack;

        private bool SingleAnimationFlag;
        private int SingleAnimationType = -1;

        public const int ENTITY_HEIGHT = 72;
        public const int ENTITY_WIDTH = 44;
        private const int LEVEL_OFFSET_HEIGHT = 20;
        private const int DISTANCE_BETWEEN_ENTITIES = 105;

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

        private int LevelHeight { get; set; }
        private int LevelWidth { get; set; } 
        private int LevelDirection = 1;        // 1 - Correct direction, -1 - inverse
        private int LevelDimention = 0;        // 0 - Ground=Ground, 
                                                // 1 - RightWall=Ground, 
                                                // 2 - Top=Ground,
                                                // 3 - LeftWall=Ground.

        public Boss(ContentManager content, int levelHeight, int levelWidth, bool EntityDirection)
        {
            EntitySprite = content.Load<Texture2D>("magolor-soul-white");
            ConstractCommonParts(levelHeight, levelWidth, EntityDirection);
        }

        public Boss(ContentManager content, string contentName, int levelHeight, int levelWidth, bool EntityDirection)
        {
            EntitySprite = content.Load<Texture2D>(contentName);
            ConstractCommonParts(levelHeight, levelWidth, EntityDirection);
        }

        private void ConstractCommonParts(int levelHeight, int levelWidth, bool EntityDirection)
        {
            LevelHeight = levelHeight - LEVEL_OFFSET_HEIGHT;
            LevelWidth = levelWidth;

            AnimationsPack = new Enemy();
            CurrentAnimation = AnimationsPack.Idle;
            Y = -LevelHeight + 100;
            X = LevelWidth - 100;
            IsAlive = true;
            this.EntityDirection = EntityDirection;
        }

        public bool GetEnemyStrikeStatus()
        {
            if (SingleAnimationType == 0 || SingleAnimationType == 2)
                return true;
            else return false;
        }

        public int GetEntityWidth() { return ENTITY_WIDTH; }

        public int GetEntityHeight() { return ENTITY_HEIGHT; }

        public void SetLevelDirection(int LEVEL_DIRECTION) { this.LevelDirection = LEVEL_DIRECTION; }

        public void SetLevelDimention(int LEVEL_DIMENTION) { this.LevelDimention = LEVEL_DIMENTION; }

        public void SetPlayerCoordinates(float X, float Y) { PlayerX = X; PlayerY = Y; }

        private void IncreaseVelocityRight()                                // If right key down & speed not max, then accelerate
        {
            if (IsGrounded())
            {
                switch (LevelDimention)
                {
                    case 0:
                    case 2:
                        if (VelocityAxisX < VELOCITY_LIMIT_X_AXIS)
                            VelocityAxisX += ACCELERATION_X_AXIS;
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY > -VELOCITY_LIMIT_X_AXIS)
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
                        if (VelocityAxisX < VELOCITY_AIR_LIMIT_X_AXIS)
                            VelocityAxisX += ACCELERATION_AIR_X_AXIS;
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY > -VELOCITY_AIR_LIMIT_X_AXIS)
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
                        if (VelocityAxisX > -VELOCITY_LIMIT_X_AXIS)
                            VelocityAxisX -= ACCELERATION_X_AXIS;
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY < VELOCITY_LIMIT_X_AXIS)
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
                        if (VelocityAxisX > -VELOCITY_AIR_LIMIT_X_AXIS)
                            VelocityAxisX -= ACCELERATION_AIR_X_AXIS;
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY < VELOCITY_AIR_LIMIT_X_AXIS)
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

        private void SetAirAnimationType(int AnimationType)
        {
            var dimentionVelocity = 0f;
            SingleAnimationFlag = true;

            switch (LevelDimention)
            {
                case 0: dimentionVelocity = VelocityAxisY; break;
                case 1: dimentionVelocity = VelocityAxisX; break;
                case 2: dimentionVelocity = -VelocityAxisY; break;
                case 3: dimentionVelocity = -VelocityAxisX; break;
                default: break;
            }
            if (dimentionVelocity < 0)
                SingleAnimationType = 2;
            else if (!IsGrounded())
                SingleAnimationType = 2;
            else SingleAnimationType = AnimationType;
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

            if (!SingleAnimationFlag)
            {
                switch (LevelDimention)
                {
                    case 0:
                    case 2: between = PlayerX - X; break;
                    case 1:
                    case 3: between = Y - PlayerY; break;
                    default: break;
                }

                if (EntityDirection)
                {
                    if (between > 0 && between < DISTANCE_BETWEEN_ENTITIES)
                    {
                        switch (LevelDimention)
                        {
                            case 0: VelocityAxisY -= VELOCITY_LIMIT_X_AXIS; break;
                            case 1: VelocityAxisX -= VELOCITY_LIMIT_X_AXIS; break;
                            case 2: VelocityAxisY += VELOCITY_LIMIT_X_AXIS; break;
                            case 3: VelocityAxisX += VELOCITY_LIMIT_X_AXIS; break;
                            default: break;
                        }
                        SetAirAnimationType(0);
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
                    SetAirAnimationType(0);
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
                case 0: CoordinatesInFrameGround(); break;
                case 1: CoordinatesInFrameWall(); break;
                case 2: CoordinatesInFrameGround(); break;
                case 3: CoordinatesInFrameWall(); break;
                default: CoordinatesInFrameGround(); break;
            }
        }

        private void UpdateAnimationBasedOnVelocity()                           // Update current animation
        {
            if (SingleAnimationFlag)
            {
                switch (SingleAnimationType)
                {
                    case 0: CurrentAnimation = AnimationsPack.StrikeOne; break;
                    case 2: CurrentAnimation = AnimationsPack.JumpStrike; break;
                    case 3: CurrentAnimation = AnimationsPack.Celebrate; break;
                    case 4: CurrentAnimation = AnimationsPack.Dead; break;
                    default: break;
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
            if (X - ENTITY_HEIGHT < 0)
            {                                        // X position fall out frame
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
                EntityDirection = false;
                Y = VelocityAxisY = 0;
            }
            else if (Y + ENTITY_WIDTH >= LevelHeight)
            {         // Y position fall out frame 
                EntityDirection = true;
                Y = LevelHeight - ENTITY_WIDTH;         // Velocity = 0;
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
            else if (X + ENTITY_WIDTH > LevelWidth)
            {
                EntityDirection = false;
                X = LevelWidth - ENTITY_WIDTH;
                VelocityAxisX = 0;
            }

            if (Y < 0)
                Y = VelocityAxisY = 0;
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

        private void InverseAxisDirections()
        {
            LevelDirection = -LevelDirection;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                UpdateVelocityBasedOnDirection();

                UpdateCoordinatesBasedOnVelocity(gameTime);
            }
            else if (SingleAnimationType != 4)
            {
                SingleAnimationFlag = true;
                SingleAnimationType = 4;
            }
            else IsDead = true;

            UpdateAnimationBasedOnVelocity();

            if (!SingleAnimationFlag)
                CurrentAnimation.UpdateCycleAnimation(gameTime);
            else
            {
                SingleAnimationFlag = CurrentAnimation.UpdateSingleAnimationIsEnded(gameTime);
                if (!SingleAnimationFlag)
                    SingleAnimationType = -1;
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

            if (IsDead)
                batcher.Draw(EntitySprite, new Vector2(X, Y), new Rectangle(0, 0, 1, 1), TintColor);
            else if (SingleAnimationType == 4)
                batcher.Draw(EntitySprite, new Vector2(X, Y), CurrentAnimation.CurrentRectangle, TintColor,
                                            rotationAngle, new Vector2(1, 1), 1f, effectsApplyed, .0f);
            else if (IsAlive)
                batcher.Draw(EntitySprite, new Vector2(X, Y), CurrentAnimation.CurrentRectangle, TintColor,
                                            rotationAngle, new Vector2(1, 1), 1f, effectsApplyed, .0f);
        }

        public override void Touch()
        {

        }
    }
}