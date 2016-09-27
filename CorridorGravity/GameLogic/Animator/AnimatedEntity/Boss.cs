using System;
using CorridorGravity.GameLogic.Animator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorridorGravity.GameLogic.AnimatedEntity
{
    class Boss : AnimationEntity
    {
        public const double INTERVAL_DEFAULT = .15;
        public const double INTERVAL_ATTACK = .25;

        public Boss()
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

            // Idle 192x84 (each 48x60) start in 212x99
            Idle.AddFrame(new Rectangle(212, 99, 48, 60), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(276, 99, 48, 60), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(340, 99, 48, 60), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(404, 99, 48, 60), TimeSpan.FromSeconds(INTERVAL_DEFAULT));

            // Walk 240x82 (each 48x60) start in 212x99
            Walk.AddFrame(new Rectangle(212, 99, 44, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(50, 239, 44, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(99, 239, 44, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(149, 239, 44, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(199, 239, 44, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));

            // Jump 164x92 (each 41x92) start in 205x7
            //Jump.AddFrame(new Rectangle(206, 9, 44, 90), TimeSpan.FromSeconds(INTERVAL));
            Jump.AddFrame(new Rectangle(254, 9, 36, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Jump.AddFrame(new Rectangle(296, 9, 32, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Jump.AddFrame(new Rectangle(332, 9, 36, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Jump.AddFrame(new Rectangle(296, 9, 32, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Jump.AddFrame(new Rectangle(254, 9, 36, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            //Jump.AddFrame(new Rectangle(206, 9, 44, 90), TimeSpan.FromSeconds(INTERVAL));

            // Intro equal Celebrate

            // Dead 212x92 (each 73x92) start in 295x131
            Dead.AddFrame(new Rectangle(295, 131, 73, 92), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Dead.AddFrame(new Rectangle(368, 131, 73, 92), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Dead.AddFrame(new Rectangle(441, 131, 73, 92), TimeSpan.FromSeconds(INTERVAL_DEFAULT));

            // StrikeOne (first two 100x86, (each 50x86) start in 2x129), 
            // thrid (75x86 start in 107x130), 
            // fourth and fifth equal first two.
            StrikeOne.AddFrame(new Rectangle(1, 129, 50, 86), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(51, 129, 50, 86), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(107, 130, 75, 86), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(51, 129, 50, 86), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(1, 129, 50, 86), TimeSpan.FromSeconds(INTERVAL_ATTACK));

            // StrikeTwo First (48x84 start in 3x368)
            // Second (80x96 start in 57x357)
            // Third (54x112 start in 140x341)
            // Forth and fifth equal second and first
            StrikeTwo.AddFrame(new Rectangle(3, 368, 48, 84), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeTwo.AddFrame(new Rectangle(57, 357, 48, 84), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeTwo.AddFrame(new Rectangle(140, 341, 54, 112), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeTwo.AddFrame(new Rectangle(57, 357, 48, 84), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeTwo.AddFrame(new Rectangle(3, 368, 48, 84), TimeSpan.FromSeconds(INTERVAL_ATTACK));

            // JumpStrike First (37x64 start in 342x398)
            // Second (60x64 start in 384x399)
            // Third equal first
            JumpStrike.AddFrame(new Rectangle(342, 398, 37, 64), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            JumpStrike.AddFrame(new Rectangle(384, 399, 60, 64), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            JumpStrike.AddFrame(new Rectangle(342, 398, 37, 64), TimeSpan.FromSeconds(INTERVAL_ATTACK));

            // Celebrate (144x88 (each 36x88) start in 340x499)
            Celebrate.AddFrame(new Rectangle(340, 499, 36, 88), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Celebrate.AddFrame(new Rectangle(376, 499, 36, 88), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Celebrate.AddFrame(new Rectangle(422, 499, 36, 88), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
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

        public override Animation Portrait { get; }

        public override Animation Health { get; }
    }
}
