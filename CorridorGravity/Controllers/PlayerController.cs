using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using CorridorGravity.Entities;
using CorridorGravity.Animations;

namespace CorridorGravity.Controllers
{
    /// <summary>
    /// Players's controller representation <see cref="EntityController"/>
    /// </summary>
    class PlayerController : EntityController
    {
        public override void Handle(Entity entity, GameTime gameTime)
        {
            var pressed = Keyboard.GetState().GetPressedKeys();

            if (!base.ResetAvoidS.HasFlag(entity.AState))
                entity.AState = AnimationState.IDLE;

            var tempFire = AnimationState.NONE;

            foreach(var key in pressed)
            {
                switch(key)
                {
                    case Keys.D: 
                    case Keys.Right:
                        entity.Direction = 1;
                        if (!pressed.Contains<Keys>(Keys.Left) && !pressed.Contains<Keys>(Keys.A))
                            HMove(entity);
                        break;
                    case Keys.A: 
                    case Keys.Left:
                        entity.Direction = -1;
                        if (!pressed.Contains<Keys>(Keys.Right) && !pressed.Contains<Keys>(Keys.D))
                            HMove(entity);
                        break;
                    case Keys.W:
                    case Keys.Up:
                        HJump(entity);
                        break;
                    case Keys.E:
                        if (entity.IsGrounded) tempFire = AnimationState.FIRE;
                        else tempFire = AnimationState.FIRE_AIR; break;
                    case Keys.Q:
                        if (entity.IsGrounded) tempFire = AnimationState.FIRE_OTHER;
                        else    tempFire = AnimationState.FIRE_AIR; break;
                    case Keys.Space:
                        if (entity.IsGrounded) tempFire = AnimationState.FIRE_SPECIAL;
                        else tempFire = AnimationState.FIRE_AIR; break;
                    default: break;
                }
            }

            if (tempFire != AnimationState.NONE)
                HFire(entity, tempFire);

            HIdle(entity);

            HGravity(entity);

            HPosition(entity, gameTime);

            HHorizontalFall(entity);
            HVerticalFall(entity);
        }
    }
}
