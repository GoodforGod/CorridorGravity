using System;
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
    class PlayerEntity : Entity
    {
        private static Color TintColor = Color.White;

        public override float X { get; set; } 
        public override float Y { get; set; }
        public override Texture2D EntitySprite { get; } 
        private Animation CurrentAnimation { get; set; }
        private Bob SpriteBob;
        private int EntityHeight = 90;
        private int EntityWidth = 44;

        private const float JUMP_POWER = 550f;
        private const float AIR_LIMIT_X_AXIS = 10f;
        private const float VELOCITY_LIMIT_X_AXIS = 260f;
        private const float VELOCITY_LIMIT_Y_AXIS = 180f;
        private const float ACCELERATION_X_AXIS = 85f;
        private const float GRAVITY = -9.8f;

        private float VelocityAxisY = 0;
        private float VelocityAxisX = 0;

        private int LEVEL_HEIGHT { get; set; }
        private int LEVEL_WIDTH { get; set; }
        private int LEVEL_OFFSET_HEIGHT = 20;
        private int LEVEL_DIMENTION = 0;    // 0 - Ground=Ground, 
                                            // 1 - RightWall=Ground, 
                                            // 2 - Top=Ground,
                                            // 3 - LeftWall=Ground.

        private bool PLAYER_DIRECTION;      // Direction of animation, false - Animation direction right, true - left

        public PlayerEntity(ContentManager content, int levelHeight, int levelWidth)  {
            EntitySprite = content.Load<Texture2D>("player-2-white-1");
            LEVEL_HEIGHT = levelHeight - LEVEL_OFFSET_HEIGHT;
            LEVEL_WIDTH = levelWidth;
        }

        public PlayerEntity(ContentManager content, string contentName, int levelHeight, int levelWidth) {
            EntitySprite = content.Load<Texture2D>(contentName);
            LEVEL_HEIGHT = levelHeight - LEVEL_OFFSET_HEIGHT;
            LEVEL_WIDTH = levelWidth;
        }

        public override void Init() {
            SpriteBob = new Bob();
            CurrentAnimation = SpriteBob.Idle;
            Y = -LEVEL_HEIGHT + 100;
            X = LEVEL_WIDTH/2;
        }

        private void UpdateVelocityFromInput()              // Update velocity via input & check for gravity collapse
        {
            var rightState = false;
            var leftState = false;

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                rightState = true;        
            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
                leftState = true;

            if (rightState && VelocityAxisX < VELOCITY_LIMIT_X_AXIS)
            {
                if (IsGrounded())
                    VelocityAxisX += ACCELERATION_X_AXIS;       // If right key down & speed not max, then accelerate
                else VelocityAxisX += ACCELERATION_X_AXIS / AIR_LIMIT_X_AXIS;
                PLAYER_DIRECTION = false;
            }
            if (leftState && VelocityAxisX > -VELOCITY_LIMIT_X_AXIS)
            {
                if (IsGrounded())
                    VelocityAxisX -= ACCELERATION_X_AXIS;       // If left key down & speed not max, then accelerate
                else VelocityAxisX -= ACCELERATION_X_AXIS / AIR_LIMIT_X_AXIS;
                PLAYER_DIRECTION = true;
            }
            if (VelocityAxisX > 0.001f && IsGrounded() && rightState == false)
            {
                VelocityAxisX -= ACCELERATION_X_AXIS;       // Slow down 
                if (VelocityAxisX < 0)
                    VelocityAxisX = 0;
                PLAYER_DIRECTION = false;
            }
            else if (VelocityAxisX < -0.001f && IsGrounded() && leftState == false)
            {
                VelocityAxisX += ACCELERATION_X_AXIS;       // Slow down
                if (VelocityAxisX > 0)
                    VelocityAxisX = 0;
                PLAYER_DIRECTION = true;
            }

            if ((state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W)) & IsGrounded())
                VelocityAxisY = -JUMP_POWER;                        // If jump key down & on the ground
            else if (!IsGrounded())
                VelocityAxisY -= GRAVITY;                           // In the air, then gravity keep going
            else VelocityAxisY = 0;
        }

        private void UpdateCoordinatesBasedOnVelocity(GameTime gameTime)        // Updates entitys coordinates
        {
            X += VelocityAxisX * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Y += VelocityAxisY * (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (LEVEL_DIMENTION)                    // Restore player coordinates, if out of frame, depends on Dimention
            {
                case 0: CoordinatesInFrameGround();
                    break;

                case 1: CoordinatesInFrameWall();
                    break;

                case 2: CoordinatesInFrameGround();
                    break;

                case 3: CoordinatesInFrameWall();
                    break;

                default: CoordinatesInFrameGround();
                    break;
            }
        }

        private void UpdateAnimationBasedOnVelocity()
        {
            if (VelocityAxisX != 0 || VelocityAxisY != 0)
            {
                if (VelocityAxisY > 0)
                    CurrentAnimation = SpriteBob.Jump;
                else if (VelocityAxisY < 0)
                    CurrentAnimation = SpriteBob.Jump;
                else if (IsGrounded())
                    CurrentAnimation = SpriteBob.Walk;
            }
            else if(CurrentAnimation != SpriteBob.Idle)
            {
                CurrentAnimation = SpriteBob.Idle;
            }
        }

        private void CoordinatesInFrameWall()       // Case dimention is one of the walls
        {
            if (X < 0)                                          // X position fall out frame
                X = 0;
            else if (X + EntityWidth > LEVEL_HEIGHT)
                X = LEVEL_HEIGHT - EntityWidth;

            if (Y < 0)
                Y = 0;
            else if (Y + EntityHeight >= LEVEL_WIDTH)          // Y position fall out frame
                Y = LEVEL_WIDTH - EntityHeight;         // Velocity = 0;
        }

        private void CoordinatesInFrameGround()     // Case dimention is ground or floor
        {
            if (X < 0)                                          // X position fall out frame
                X = 0;
            else if (X + EntityWidth > LEVEL_WIDTH)
                X = LEVEL_WIDTH - EntityWidth;

            if (Y < 0)
                Y = 0;
            else if (Y + EntityHeight >= LEVEL_HEIGHT)          // Y position fall out frame
                Y = LEVEL_HEIGHT - EntityHeight;         // Velocity = 0;
        }

        private bool IsGrounded()                   // Check if the entity is grounded
        {
            if (Y + EntityHeight + 1 >= LEVEL_HEIGHT)
                return true;
            else return false;
        }

        private void SwapLevelDimentions()          // Swaps levels height and width dimentions
        {
            var buffer = LEVEL_HEIGHT;
            LEVEL_HEIGHT = LEVEL_WIDTH;
            LEVEL_WIDTH = LEVEL_HEIGHT;
        }

        private void SwapAxisDirections()           
        {

        }

        public void GravityCollapse()               // Changes the gravity and physics of the game, via one of four dimentions
        {
            var randomGravityDistortion = new Random().Next(1, 3);

            switch (LEVEL_DIMENTION)
            {
                case 0:
                    break;

                case 1:
                    SwapLevelDimentions();
                    break;

                case 2:
                    break;

                case 3:
                    SwapLevelDimentions();
                    break;

                default:
                    break;
            }


        }

        public override void Update(GameTime gameTime)
        {
            UpdateVelocityFromInput();

            UpdateCoordinatesBasedOnVelocity(gameTime);

            UpdateAnimationBasedOnVelocity();

            CurrentAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch batcher)
        {
            switch(LEVEL_DIMENTION)
            {
                case 0:
                    if (PLAYER_DIRECTION)
                        batcher.Draw(EntitySprite, new Vector2(X, Y), CurrentAnimation.CurrentRectangle, TintColor,
                                        .0f, new Vector2(1, 1), 1f, SpriteEffects.FlipHorizontally, .0f);
                    else batcher.Draw(EntitySprite, new Vector2(X, Y), CurrentAnimation.CurrentRectangle, TintColor,
                                        .0f, new Vector2(1, 1), 1f, SpriteEffects.None, .0f);
                    break;

                case 1:
                    break;

                case 2:
                    if (PLAYER_DIRECTION)
                        batcher.Draw(EntitySprite, new Vector2(X, Y), CurrentAnimation.CurrentRectangle, TintColor,
                                        .0f, new Vector2(1, 1), 1f, SpriteEffects.FlipHorizontally & SpriteEffects.FlipVertically, .0f);
                    else batcher.Draw(EntitySprite, new Vector2(X, Y), CurrentAnimation.CurrentRectangle, TintColor,
                                        .0f, new Vector2(1, 1), 1f, SpriteEffects.None & SpriteEffects.FlipVertically, .0f);
                    break;

                case 3:
                    break;

                default:
                    break;
            }
        }

        public override void Touch()
        {

        }
    }
}
