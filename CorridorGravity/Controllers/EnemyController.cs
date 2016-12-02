using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using CorridorGravity.Entities;
using CorridorGravity.Animations;

namespace CorridorGravity.Controllers
{
    /// <summary>
    /// Enemie's controller representation <see cref="EntityController"/>
    /// </summary>
    class EnemyController : EntityController
    {
        DateTime Cooldown;

        protected override AnimationState IdleAvoidS
        { get {
                return base.IdleAvoidS | AnimationState.FIRE;
            } }

        public override void Handle(Entity entity, GameTime gameTime)
        {
            if (!base.ResetAvoidS.HasFlag(entity.AState))
                entity.AState = AnimationState.IDLE;

            HMove(entity);

            if (entity.EState == EntityState.READY)
            {
                HJump(entity);
                HFire(entity, AnimationState.FIRE);
            }

            HIdle(entity);

            HGravity(entity);

            HPosition(entity, gameTime);

            HVerticalFall(entity);

            HHorizontalFall(entity);

            var col = DateTime.Now;

            if (!entity.IsOnScreen && (col - Cooldown).TotalSeconds > 2)
            {
                Cooldown = col;
                entity.Velocity = Vector2.Zero;
                entity.Direction = -entity.Direction;
                HMove(entity);
            }
        }
    }
}
