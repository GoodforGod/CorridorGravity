using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using CorridorGravity.Animations;
using CorridorGravity.Entities;

namespace CorridorGravity.Controllers
{
    /// <summary>
    /// Controller which provides methods and logic to handle entities represented in the game
    /// </summary>
    class EntityController
    {
        /// <summary>
        /// States avoid while applying fire animation/action
        /// </summary>
        protected virtual AnimationState ResetAvoidS
        { get {
                return AnimationState.FIRE
                    | AnimationState.FIRE_OTHER
                    | AnimationState.FIRE_SPECIAL
                    | AnimationState.FIRE_AIR;
            } }

        /// <summary>
        /// States avoid while applying idle animation/action
        /// </summary>
        protected virtual AnimationState IdleAvoidS
            { get {
                return AnimationState.WALK
                    | AnimationState.FIRE_AIR;
            } }

        /// <summary>
        /// States avoid while applying gravity animation/action
        /// </summary>
        protected virtual AnimationState GravityAvoidS
            { get {
                return AnimationState.FIRE
                    | AnimationState.FIRE_OTHER
                    | AnimationState.FIRE_SPECIAL
                    | AnimationState.FIRE_AIR;
            } }

        /// <summary>
        /// States avoid while applying move animation/action
        /// </summary>
        protected virtual AnimationState MoveAvoidS
        { get {
                return AnimationState.IDLE;
            } }

        /// <summary>
        /// States avoid while applying jump animation/action
        /// </summary>
        protected virtual AnimationState JumpAvoidS
        { get {
                return AnimationState.NONE;
            } }

        /// <summary>
        /// Main handler, user to handle all controller logic algorithm
        /// </summary>
        public virtual void Handle(Entity entity, GameTime gameTime) { }

        /// <summary>
        /// Handles entities move action
        /// </summary>
        protected virtual void HMove(Entity entity)
        {
            var inrease = entity.VInrease * entity.Direction;

            if (!entity.IsGrounded)
                inrease /= 5;

            switch(entity.GState)
            {
                case GroundState.TOP:
                case GroundState.BOTTOM:
                    if (Math.Abs(entity.Velocity.X) < entity.VLimit)
                        entity.Velocity.X += inrease; break;
                case GroundState.RIGHT:
                case GroundState.LEFT:
                    if (Math.Abs(entity.Velocity.Y) < entity.VLimit)
                        entity.Velocity.Y += inrease; break;
                default: break;
            }

            if (MoveAvoidS.HasFlag(entity.AState))
                entity.AState = AnimationState.WALK;
        }

        /// <summary>
        /// Handles entities jump action
        /// </summary>
        protected virtual void HJump(Entity entity)
        {
            switch(entity.GState)
            {
                case GroundState.TOP:       entity.Velocity.Y = entity.VInrease * entity.JRate;   break;
                case GroundState.LEFT:      entity.Velocity.X = entity.VInrease * entity.JRate;   break;
                case GroundState.RIGHT:     entity.Velocity.X = -entity.VInrease * entity.JRate;  break;
                case GroundState.BOTTOM:    entity.Velocity.Y = -entity.VInrease * entity.JRate;  break;
                default: break;
            }

            if (JumpAvoidS.HasFlag(entity.AState))
                entity.AState = AnimationState.JUMP;
        }

        /// <summary>
        /// Handles entities fire/attack animation state and actions
        /// </summary>
        protected virtual void HFire(Entity entity, AnimationState state)
        {
            switch(state)
            {
                case AnimationState.FIRE:           entity.AState = AnimationState.FIRE;          break;
                case AnimationState.FIRE_OTHER:     entity.AState = AnimationState.FIRE_OTHER;    break;
                case AnimationState.FIRE_SPECIAL:   entity.AState = AnimationState.FIRE_SPECIAL;  break;
                case AnimationState.FIRE_AIR:       entity.AState = AnimationState.FIRE_AIR;      break;
                default:                            entity.AState = AnimationState.FIRE;          break;
            }

            entity.EState = EntityState.ACTIVE;
        }

        /// <summary>
        /// Slows entity if it isn't should idle
        /// </summary>
        protected virtual void HIdle(Entity entity)
        {
            if (IdleAvoidS.HasFlag(entity.AState))
                return;

            var prev = entity.Velocity.X;

            switch (entity.GState)
            {
                case GroundState.TOP: 
                case GroundState.BOTTOM:
                    if (entity.Velocity.X > 0)
                        entity.Velocity.X -= entity.VInrease;
                    if (entity.Velocity.X < 0)
                        entity.Velocity.X += entity.VInrease;
                    if (prev > 0 && entity.Velocity.X < 0)
                        entity.Velocity.X = 0;
                    break;
                case GroundState.RIGHT: 
                case GroundState.LEFT:
                    prev = entity.Velocity.Y;
                    if (entity.Velocity.Y > 0)
                        entity.Velocity.Y -= entity.VInrease;
                    if (entity.Velocity.Y < 0)
                        entity.Velocity.Y += entity.VInrease;
                    if (prev > 0 && entity.Velocity.Y < 0)
                        entity.Velocity.Y = 0;
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Changes entity position <see cref="Entity.Position"/> via velocity <see cref="Entity.Velocity"/>
        /// </summary>
        protected virtual void HPosition(Entity entity, GameTime gameTime)
        {
            entity.Position.X += entity.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            entity.Position.Y += entity.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Check if we are running out of the screen on axis X
        /// </summary>
        protected virtual void HHorizontalFall(Entity entity)
        {
            switch (entity.GState)
            {
                case GroundState.TOP:
                case GroundState.BOTTOM:
                    if (!entity.IsOnScreen)
                    {
                        if (entity.Position.X < entity.BoundingBox.Width)
                            entity.Position.X = entity.WCore.LLimits.X;
                        else entity.Position.X = entity.WCore.LLimits.Width - entity.BoundingBox.Width;
                    }
                    break;

                case GroundState.LEFT:
                case GroundState.RIGHT:
                    if (!entity.IsOnScreen)
                    {
                        if (entity.Position.Y < entity.BoundingBox.Width)
                            entity.Position.Y = entity.WCore.LLimits.Y;
                        else entity.Position.Y = entity.WCore.LLimits.Height - entity.BoundingBox.Width;
                    }
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Check if we are not falling under the world limit on axis Y
        /// </summary>
        protected virtual void HVerticalFall(Entity entity)
        {
            switch (entity.GState)
            {
                case GroundState.LEFT:
                    if (entity.Position.X < entity.GetGround)
                        entity.Position.X = entity.GetGround; break;
                case GroundState.TOP:
                    if (entity.Position.Y < entity.GetGround)
                        entity.Position.Y = entity.GetGround; break;
                case GroundState.RIGHT:
                    if (entity.Position.X > entity.GetGround)
                        entity.Position.X = entity.GetGround; break;
                case GroundState.BOTTOM:
                    if (entity.Position.Y > entity.GetGround)
                        entity.Position.Y = entity.GetGround; break;
                default: break;
            }
        }

        /// <summary>
        /// Use gravity force if not on the ground
        /// </summary>
        protected virtual void HGravity(Entity entity)
        {
            if (entity.IsGrounded)
                return;

            switch(entity.GState)
            {
                case GroundState.TOP:       entity.Velocity.Y -= entity.WCore.Gravity;  break;
                case GroundState.LEFT:      entity.Velocity.X -= entity.WCore.Gravity;  break;
                case GroundState.RIGHT:     entity.Velocity.X += entity.WCore.Gravity;  break;
                case GroundState.BOTTOM:    entity.Velocity.Y += entity.WCore.Gravity;  break;
                default:                                                                break;
            }

            if (!GravityAvoidS.HasFlag(entity.AState))
                entity.AState = AnimationState.FALL;
        }
    }
}
