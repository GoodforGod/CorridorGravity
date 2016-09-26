﻿using System;
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

namespace CorridorGravity.GameLogic
{
    class EnemyEntity : Entity
    {
        private static Color TintColor = Color.White;

        public override float X { get; set; }
        public override float Y { get; set; }
        public override Texture2D EntitySprite { get; }
        private Animation CurrentAnimation { get; set; }
        private Bob AnimationsPack;
        private bool SingleAnimationFlag;
        private int SingleAnimationType;
        private int EntityHeight = 90;
        private int EntityWidth = 44;

        private int DoubleJumpFlag = 0;
        private const float JUMP_POWER = 450f;
        private const float VELOCITY_AIR_LIMIT_X_AXIS = 40f;
        private const float VELOCITY_LIMIT_X_AXIS = 260f;
        private const float VELOCITY_LIMIT_Y_AXIS = 180f;
        private const float ACCELERATION_AIR_X_AXIS = 3f;
        private const float ACCELERATION_X_AXIS = 85f;
        private const float GRAVITY = -9.8f;

        private float SlowDownLimit = 0.001f;
        private float VelocityAxisY = 0;
        private float VelocityAxisX = 0;
        private int accum = -1;

        private int LEVEL_HEIGHT { get; set; }
        private int LEVEL_WIDTH { get; set; }
        private int LEVEL_OFFSET_HEIGHT = 20;
        private int LEVEL_DIRECTION = 1;    // 1 - Correct direction, -1 - inverse
        private int LEVEL_DIMENTION = 0;    // 0 - Ground=Ground, 
                                            // 1 - RightWall=Ground, 
                                            // 2 - Top=Ground,
                                            // 3 - LeftWall=Ground.

        private bool PLAYER_DIRECTION;      // Direction of animation, false - Animation direction right, true - left

        public EnemyEntity(ContentManager content, int levelHeight, int levelWidth)
        {
            EntitySprite = content.Load<Texture2D>("player-2-white-1");
            ConstractCommonParts(levelHeight, levelWidth);
        }

        public EnemyEntity(ContentManager content, string contentName, int levelHeight, int levelWidth)
        {
            EntitySprite = content.Load<Texture2D>(contentName);
            ConstractCommonParts(levelHeight, levelWidth);
        }
        
        private void ConstractCommonParts(int levelHeight, int levelWidth)
        { 
            LEVEL_HEIGHT = levelHeight - LEVEL_OFFSET_HEIGHT;
            LEVEL_WIDTH = levelWidth;
        }

        public override void Init()
        {
            AnimationsPack = new Bob();
            CurrentAnimation = AnimationsPack.Idle;
            Y = -LEVEL_HEIGHT + 100;
            X = LEVEL_WIDTH - 100;
        }

        public void SetLevelDirection(int LEVEL_DIRECTION) { this.LEVEL_DIRECTION = LEVEL_DIRECTION; }

        public void SetLevelDimention(int LEVEL_DIMENTION) { this.LEVEL_DIMENTION = LEVEL_DIMENTION; }

        private void IncreaseVelocityRight()                // If right key down & speed not max, then accelerate
        {
            if (IsGrounded())
            {
                switch (LEVEL_DIMENTION)
                {
                    case 0:
                    case 2:
                        if (VelocityAxisX < VELOCITY_LIMIT_X_AXIS)
                            VelocityAxisX += ACCELERATION_X_AXIS;
                        PLAYER_DIRECTION = false;
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY > -VELOCITY_LIMIT_X_AXIS)
                            VelocityAxisY -= ACCELERATION_X_AXIS;
                        PLAYER_DIRECTION = true;
                        break;
                    default: break;
                }

            }
            else
            {
                switch (LEVEL_DIMENTION)
                {
                    case 0:
                    case 2:
                        if (VelocityAxisX < VELOCITY_AIR_LIMIT_X_AXIS)
                            VelocityAxisX += ACCELERATION_AIR_X_AXIS;
                        PLAYER_DIRECTION = false;
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY > -VELOCITY_AIR_LIMIT_X_AXIS)
                            VelocityAxisY -= ACCELERATION_AIR_X_AXIS;
                        PLAYER_DIRECTION = true;
                        break;
                    default: break;
                }
            }
        }

        private void IncreaseVelocityLeft()                 // If left key down & speed not max, then accelerate
        {
            if (IsGrounded())
            {
                switch (LEVEL_DIMENTION)
                {
                    case 0:
                    case 2:
                        if (VelocityAxisX > -VELOCITY_LIMIT_X_AXIS)
                            VelocityAxisX -= ACCELERATION_X_AXIS;
                        PLAYER_DIRECTION = true;
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY < VELOCITY_LIMIT_X_AXIS)
                            VelocityAxisY += ACCELERATION_X_AXIS;
                        PLAYER_DIRECTION = false;
                        break;
                    default: break;
                }

            }
            else
            {
                switch (LEVEL_DIMENTION)
                {
                    case 0:
                    case 2:
                        if (VelocityAxisX > -VELOCITY_AIR_LIMIT_X_AXIS)
                            VelocityAxisX -= ACCELERATION_AIR_X_AXIS;
                        PLAYER_DIRECTION = true;
                        break;
                    case 1:
                    case 3:
                        if (VelocityAxisY < VELOCITY_AIR_LIMIT_X_AXIS)
                            VelocityAxisY += ACCELERATION_AIR_X_AXIS;
                        PLAYER_DIRECTION = false;
                        break;

                    default: break;
                }
            }
        }

        private void SlowVelocityRight()                    //Slow down acceleration
        {
            if ((LEVEL_DIMENTION == 0 || LEVEL_DIMENTION == 2) && VelocityAxisX > SlowDownLimit)
            {
                VelocityAxisX -= ACCELERATION_X_AXIS;
                if (VelocityAxisX < 0)
                    VelocityAxisX = 0;
                PLAYER_DIRECTION = false;
            }
            else if (VelocityAxisY < -SlowDownLimit)
            {
                VelocityAxisY += ACCELERATION_X_AXIS;
                if (VelocityAxisY > 0)
                    VelocityAxisY = 0;
                PLAYER_DIRECTION = true;
            }
        }

        private void SlowVelocityLeft()                     // Slow down acceleration
        {
            if ((LEVEL_DIMENTION == 0 || LEVEL_DIMENTION == 2) && VelocityAxisX < -SlowDownLimit)
            {
                VelocityAxisX += ACCELERATION_X_AXIS;
                if (VelocityAxisX > 0)
                    VelocityAxisX = 0;
                PLAYER_DIRECTION = true;
            }
            else if (VelocityAxisY > SlowDownLimit)
            {
                VelocityAxisY -= ACCELERATION_X_AXIS;
                if (VelocityAxisY < 0)
                    VelocityAxisY = 0;
                PLAYER_DIRECTION = false;
            }
        }

        private void SingleAnimationTypeInAir(int AnimationType)
        {
            var dimentionVelocity = 0f;
            SingleAnimationFlag = true;

            switch (LEVEL_DIMENTION)
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

        private void UpdateVelocityFromInput()              // Update velocity via input & check for gravity collapse
        {
            var rightState = false;
            var leftState = false;

            /*
            KeyboardState state = Keyboard.GetState();
            //MouseState mouseState = Mouse.GetState();

            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                rightState = true;
            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
                leftState = true;
            if (state.IsKeyDown(Keys.Q) && !SingleAnimationFlag)
                SingleAnimationTypeInAir(0);
            if (state.IsKeyDown(Keys.E) && !SingleAnimationFlag)
                SingleAnimationTypeInAir(1);

            // Test code section BEGIN ############################

            if (state.IsKeyDown(Keys.C) && accum == -1)           // RightWall
            {
                accum = 1;
                GravityCollapse(accum);
            }
            if (state.IsKeyUp(Keys.C) && accum == 1)
                accum = -1;

            if (state.IsKeyDown(Keys.Space) && accum == -1)       // Floor
            {
                accum = 2;
                GravityCollapse(accum);
            }
            if (state.IsKeyUp(Keys.Space) && accum == 2)
                accum = -1;

            if (state.IsKeyDown(Keys.V) && accum == -1)           // LeftWall
            {
                accum = 3;
                GravityCollapse(accum);
            }
            if (state.IsKeyUp(Keys.V) && accum == 3)
                accum = -1;

            if (state.IsKeyDown(Keys.Z) && accum == -1)           // Ground
            {
                accum = 0;
                GravityCollapse(accum);
            }
            if (state.IsKeyUp(Keys.Z) && accum == 0)
                accum = -1;

            // END ################################################
            */
            if (rightState)                                                                         // Right Acceleration
            {
                if (LEVEL_DIRECTION == 1)                // if InverseAxisDirections was applyed
                    IncreaseVelocityRight();
                else
                    IncreaseVelocityLeft();
            }
            if (leftState)                                                                          // Left Acceleration
            {
                if (LEVEL_DIRECTION == 1)                // if InverseAxisDirections was applyed
                    IncreaseVelocityLeft();
                else
                    IncreaseVelocityRight();
            }
            if (IsGrounded() && !rightState)                                                // Right SlowDown
            {
                if (LEVEL_DIRECTION == 1)
                    SlowVelocityRight();
                else
                    SlowVelocityLeft();
            }
            if (IsGrounded() && !leftState)                                                 // Left SlowDown
            {
                if (LEVEL_DIRECTION == 1)
                    SlowVelocityLeft();
                else
                    SlowVelocityRight();
            }
            /*
            if ((state.IsKeyUp(Keys.Up) && state.IsKeyUp(Keys.W)) & DoubleJumpFlag > 1)             // Slow down after jump
            {
                switch (LEVEL_DIMENTION)
                {
                    case 0: break;
                    case 1: break;
                    case 2: break;
                    case 3: break;
                    default: break;
                }
            }
            else if ((state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W)) & DoubleJumpFlag < 2)    // Jump power
            {
                DoubleJumpFlag = 1;
                switch (LEVEL_DIMENTION)                         // If jump key down & on the ground
                {
                    case 0: VelocityAxisY = -JUMP_POWER; break;
                    case 1: VelocityAxisX = -JUMP_POWER; break;
                    case 2: VelocityAxisY = JUMP_POWER; break;
                    case 3: VelocityAxisX = JUMP_POWER; break;
                    default: break;
                }
            }
            */
            if (!IsGrounded())                                                                   // Gravity activater in air
            {
                switch (LEVEL_DIMENTION)
                {
                    case 0: VelocityAxisY -= GRAVITY; break;
                    case 1: VelocityAxisX -= GRAVITY; break;
                    case 2: VelocityAxisY += GRAVITY; break;
                    case 3: VelocityAxisX += GRAVITY; break;
                    default: break;
                }
            }
            else if (IsGrounded())                                                             // On ground, failsave from 
            {
                switch (LEVEL_DIMENTION)
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

            switch (LEVEL_DIMENTION)                    // Restore player coordinates, if out of frame, depends on Dimention
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
            if (SingleAnimationFlag)
            {
                switch (SingleAnimationType)
                {
                    case 0: CurrentAnimation = AnimationsPack.StrikeOne;    break;
                    case 1: CurrentAnimation = AnimationsPack.StrikeTwo;    break;
                    case 2: CurrentAnimation = AnimationsPack.JumpStrike;   break;
                    case 3: CurrentAnimation = AnimationsPack.Celebrate;    break;
                    default:                                                break;
                }
            }
            else if (VelocityAxisX != 0 || VelocityAxisY != 0)
            {
                var dimentionVelocity = VelocityAxisX;
                if (LEVEL_DIMENTION == 0 || LEVEL_DIMENTION == 2)
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
            if (X - EntityHeight < 0)                                          // X position fall out frame
            {
                X = EntityHeight;
                VelocityAxisX = 0;
            }
            else if (X - LEVEL_OFFSET_HEIGHT > LEVEL_WIDTH)
            {
                if (LEVEL_DIMENTION == 1)
                {
                    X = LEVEL_WIDTH - EntityWidth;
                    VelocityAxisX = 0;
                }
                else
                {
                    X = LEVEL_WIDTH;
                    VelocityAxisX = 0;
                }
            }

            if (Y < 0)
            {
                Y = 0;
                VelocityAxisY = 0;
            }
            else if (Y + EntityWidth >= LEVEL_HEIGHT)          // Y position fall out frame
            {
                Y = LEVEL_HEIGHT - EntityWidth;         // Velocity = 0;
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
            else if (X + EntityWidth > LEVEL_WIDTH)
            {
                X = LEVEL_WIDTH - EntityWidth;
                VelocityAxisX = 0;
            }

            if (Y < 0)
            {
                Y = 0;
                VelocityAxisY = 0;
            }
            else if (Y + EntityHeight >= LEVEL_HEIGHT)          // Y position fall out frame
            {
                Y = LEVEL_HEIGHT - EntityHeight;         // Velocity = 0;
                VelocityAxisY = 0;
            }
        }

        private bool IsGrounded()                                               // Check if the entity is grounded
        {
            var offset = 1;

            switch (LEVEL_DIMENTION)
            {
                case 0:
                    if (Y + EntityHeight + offset >= LEVEL_HEIGHT)
                    {
                        DoubleJumpFlag = 0;
                        return true;
                    }
                    else return false;

                case 1:
                    if (X + LEVEL_OFFSET_HEIGHT - offset * 2 >= LEVEL_WIDTH)
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

        private void SwapLevelDimentions()                                      // Swaps levels height and width dimentions
        {
            // Not Used
            var buffer = LEVEL_HEIGHT;
            LEVEL_HEIGHT = LEVEL_WIDTH;
            LEVEL_WIDTH = LEVEL_HEIGHT;
        }

        private void InverseAxisDirections()
        {
            LEVEL_DIRECTION = -LEVEL_DIRECTION;
        }

        public void GravityCollapse(int calledDimention)                          // Changes the dimention (gravity and physics of the game)
        {
            //var randomGravityDistortion = new Random().Next(1, 3); 
            var randomGravityDistortion = calledDimention;

            switch (randomGravityDistortion)
            {
                case 0: LEVEL_DIMENTION = 0; break;
                case 1: LEVEL_DIMENTION = 1; break;
                case 2: LEVEL_DIMENTION = 2; break;
                case 3: LEVEL_DIMENTION = 3; break;
                default: break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            UpdateVelocityFromInput();

            UpdateCoordinatesBasedOnVelocity(gameTime);

            UpdateAnimationBasedOnVelocity();

            if (!SingleAnimationFlag)
                CurrentAnimation.UpdateCycleAnimation(gameTime);
            else
                SingleAnimationFlag = CurrentAnimation.UpdateSingleAnimation(gameTime);
        }

        public override void Draw(SpriteBatch batcher)
        {
            SpriteEffects effectsApplyed = SpriteEffects.None;

            var rotationAngle = .0f;

            switch (LEVEL_DIMENTION)                             // Choose sprite flip, depend on the current dimention
            {
                case 0:
                    rotationAngle = .0f;
                    if (PLAYER_DIRECTION)
                        effectsApplyed = SpriteEffects.FlipHorizontally;
                    else effectsApplyed = SpriteEffects.None;
                    break;

                case 1:
                    rotationAngle = 1.571f;
                    if (PLAYER_DIRECTION)
                        effectsApplyed = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    else effectsApplyed = SpriteEffects.FlipVertically;
                    break;

                case 2:
                    rotationAngle = .0f;
                    if (PLAYER_DIRECTION)
                        effectsApplyed = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    else effectsApplyed = SpriteEffects.FlipVertically;
                    break;

                case 3:
                    rotationAngle = 1.571f;
                    if (PLAYER_DIRECTION)
                        effectsApplyed = SpriteEffects.FlipHorizontally;
                    else effectsApplyed = SpriteEffects.None;
                    break;

                default: break;
            }

            batcher.Draw(EntitySprite, new Vector2(X, Y), CurrentAnimation.CurrentRectangle, TintColor,
                                        rotationAngle, new Vector2(1, 1), 1f, effectsApplyed, .0f);
        }

        public override void Touch()
        {

        }
    }
}