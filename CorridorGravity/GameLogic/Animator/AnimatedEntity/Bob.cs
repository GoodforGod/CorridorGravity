using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorridorGravity.GameLogic.Animator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CorridorGravity.GameLogic.AnimatedEntity
{
    class Bob : EntityAnimation
    {
        public const double INTERVAL = .25; 

        public Bob()
        {
            Idle = new Animation();
            Walk = new Animation();
            Jump = new Animation();
            Intro = new Animation();
            Dead = new Animation();
            StrikeOne = new Animation();
            StrikeTwo = new Animation();
            JumpStrike = new Animation();
            Celebrate = new Animation();

            // Idle 192x84 (each 48x84) start in 6x15
            Idle.AddFrame(new Rectangle(6, 15, 48, 84), TimeSpan.FromSeconds(INTERVAL));
            Idle.AddFrame(new Rectangle(54, 15, 48, 84), TimeSpan.FromSeconds(INTERVAL));
            Idle.AddFrame(new Rectangle(102, 15, 48, 84), TimeSpan.FromSeconds(INTERVAL));
            Idle.AddFrame(new Rectangle(150, 15, 48, 84), TimeSpan.FromSeconds(INTERVAL));
            Idle.AddFrame(new Rectangle(102, 15, 48, 84), TimeSpan.FromSeconds(INTERVAL));
            Idle.AddFrame(new Rectangle(54, 15, 48, 84), TimeSpan.FromSeconds(INTERVAL));

            // Walk 240x82 (each 60x82) start in 3x239
            Walk.AddFrame(new Rectangle(3, 239, 60, 82), TimeSpan.FromSeconds(INTERVAL));
            Walk.AddFrame(new Rectangle(63, 239, 60, 82), TimeSpan.FromSeconds(INTERVAL));
            Walk.AddFrame(new Rectangle(123, 239, 60, 82), TimeSpan.FromSeconds(INTERVAL));
            Walk.AddFrame(new Rectangle(183, 239, 60, 82), TimeSpan.FromSeconds(INTERVAL));
            Walk.AddFrame(new Rectangle(243, 239, 60, 82), TimeSpan.FromSeconds(INTERVAL));

            // Jump 164x92 (each 41x92) start in 205x7
            Jump.AddFrame(new Rectangle(205, 7, 16, 92), TimeSpan.FromSeconds(INTERVAL));
            Jump.AddFrame(new Rectangle(246, 7, 16, 92), TimeSpan.FromSeconds(INTERVAL));
            Jump.AddFrame(new Rectangle(287, 7, 16, 92), TimeSpan.FromSeconds(INTERVAL));
            Jump.AddFrame(new Rectangle(328, 7, 16, 92), TimeSpan.FromSeconds(INTERVAL));
            Jump.AddFrame(new Rectangle(287, 7, 16, 92), TimeSpan.FromSeconds(INTERVAL));
            Jump.AddFrame(new Rectangle(246, 7, 16, 92), TimeSpan.FromSeconds(INTERVAL));
            Jump.AddFrame(new Rectangle(205, 7, 16, 92), TimeSpan.FromSeconds(INTERVAL));

            // Intro equal Celebrate

            // Dead 212x92 (each 73x92) start in 295x131
            Dead.AddFrame(new Rectangle(295, 131, 73, 92), TimeSpan.FromSeconds(INTERVAL));
            Dead.AddFrame(new Rectangle(368, 131, 73, 92), TimeSpan.FromSeconds(INTERVAL));
            Dead.AddFrame(new Rectangle(441, 131, 73, 92), TimeSpan.FromSeconds(INTERVAL));

            // StrikeOne (first two 100x86, (each 50x86) start in 2x129), 
            // thrid (75x86 start in 107x130), 
            // fourth and fifth equal first two.
            StrikeOne.AddFrame(new Rectangle(1, 129, 50, 86), TimeSpan.FromSeconds(INTERVAL));
            StrikeOne.AddFrame(new Rectangle(51, 129, 50, 86), TimeSpan.FromSeconds(INTERVAL));
            StrikeOne.AddFrame(new Rectangle(107, 130, 75, 86), TimeSpan.FromSeconds(INTERVAL));
            StrikeOne.AddFrame(new Rectangle(51, 129, 50, 86), TimeSpan.FromSeconds(INTERVAL));
            StrikeOne.AddFrame(new Rectangle(1, 129, 50, 86), TimeSpan.FromSeconds(INTERVAL));

            // StrikeTwo First (48x84 start in 3x368)
            // Second (80x96 start in 57x357)
            // Third (54x112 start in 140x341)
            // Forth and fifth equal second and first
            StrikeTwo.AddFrame(new Rectangle(3, 368, 48, 84), TimeSpan.FromSeconds(INTERVAL));
            StrikeTwo.AddFrame(new Rectangle(57, 357, 48, 84), TimeSpan.FromSeconds(INTERVAL));
            StrikeTwo.AddFrame(new Rectangle(140, 341, 54, 112), TimeSpan.FromSeconds(INTERVAL));
            StrikeTwo.AddFrame(new Rectangle(57, 357, 48, 84), TimeSpan.FromSeconds(INTERVAL));
            StrikeTwo.AddFrame(new Rectangle(3, 368, 48, 84), TimeSpan.FromSeconds(INTERVAL));

            // JumpStrike First (37x64 start in 342x398)
            // Second (60x64 start in 384x399)
            // Third equal first
            JumpStrike.AddFrame(new Rectangle(342, 398, 37, 64), TimeSpan.FromSeconds(INTERVAL));
            JumpStrike.AddFrame(new Rectangle(384, 399, 60, 64), TimeSpan.FromSeconds(INTERVAL));
            JumpStrike.AddFrame(new Rectangle(342, 398, 37, 64), TimeSpan.FromSeconds(INTERVAL));

            // Celebrate (144x88 (each 36x88) start in 340x499)
            Celebrate.AddFrame(new Rectangle(340, 499, 36, 88), TimeSpan.FromSeconds(INTERVAL));
            Celebrate.AddFrame(new Rectangle(376, 499, 36, 88), TimeSpan.FromSeconds(INTERVAL));
            Celebrate.AddFrame(new Rectangle(422, 499, 36, 88), TimeSpan.FromSeconds(INTERVAL));
        }

        public override Animation Idle { get; }

        public override Animation Walk { get; }

        public override Animation Jump { get; }

        public override Animation Intro { get; }

        public override Animation Dead { get; }

        public override Animation StrikeOne { get; }

        public override Animation StrikeTwo { get; }

        public override Animation JumpStrike { get; }

        public override Animation Celebrate { get; }
    }
}
