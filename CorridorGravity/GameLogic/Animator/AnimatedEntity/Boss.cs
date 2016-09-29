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
        public const double INTERVAL_STEADY = .25;
        public const double INTERVAL_TENTLE = .35; 

        public Rectangle EyeWhitePart { get; }
        public Rectangle EyeRedPart { get; }
        public Rectangle EyeBlackPart { get; }

        public Boss()
        {
            EyeWhitePart = new Rectangle(489, 133, 52, 52);
            EyeRedPart = new Rectangle(500, 94, 29, 28);
            EyeBlackPart = new Rectangle(509, 80, 10, 10);

            Idle = new Animation();
            Walk = new Animation();
            Jump = new Animation();
            Intro = new Animation();
            Dead = new Animation();
            StrikeOne = new Animation();
            StrikeTwo = new Animation();
            JumpStrike = new Animation();
            Celebrate = new Animation();

            // Idle (each 200x232) start in 0x0
            Idle.AddFrame(new Rectangle(218, 0, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(218, 2, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(218, 4, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(218, 6, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(0, 8, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(0, 10, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(0, 8, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(218, 6, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(218, 4, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(218, 2, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(218, 0, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));

            // Walk (each 200x232) start in 218x250
            Walk.AddFrame(new Rectangle(218, 250, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(218, 252, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(218, 254, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(218, 256, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(7, 258, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(7, 260, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(7, 258, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(218, 256, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(218, 254, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(218, 252, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(218, 250, 200, 232), TimeSpan.FromSeconds(INTERVAL_DEFAULT));

            // Fist appear (each 68x88) start in 727x346
            StrikeOne.AddFrame(new Rectangle(727, 346, 68, 88), TimeSpan.FromSeconds(INTERVAL_STEADY));
            StrikeOne.AddFrame(new Rectangle(727, 349, 68, 88), TimeSpan.FromSeconds(INTERVAL_STEADY));
            StrikeOne.AddFrame(new Rectangle(727, 352, 68, 88), TimeSpan.FromSeconds(INTERVAL_STEADY));
            StrikeOne.AddFrame(new Rectangle(727, 355, 68, 88), TimeSpan.FromSeconds(INTERVAL_STEADY));
            StrikeOne.AddFrame(new Rectangle(727, 358, 68, 88), TimeSpan.FromSeconds(INTERVAL_STEADY));

            // Fist Attack (each 68x88) start in 642x346
            StrikeOne.AddFrame(new Rectangle(642, 346, 68, 88), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(642, 349, 68, 88), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(642, 352, 68, 88), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(642, 355, 68, 88), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(642, 358, 68, 88), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(642, 355, 68, 88), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(642, 352, 68, 88), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(642, 349, 68, 88), TimeSpan.FromSeconds(INTERVAL_ATTACK));

            // Wave (each 100x142) start in 478x500
            Jump.AddFrame(new Rectangle(478, 500, 118, 142), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            Jump.AddFrame(new Rectangle(613, 500, 118, 142), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            Jump.AddFrame(new Rectangle(754, 500, 118, 142), TimeSpan.FromSeconds(INTERVAL_ATTACK)); 

            // Tentcle  (each 156x116) start in 728x16
            Celebrate.AddFrame(new Rectangle(728, 16, 156, 116), TimeSpan.FromSeconds(INTERVAL_TENTLE));
            Celebrate.AddFrame(new Rectangle(728, 131, 156, 116), TimeSpan.FromSeconds(INTERVAL_TENTLE));
            Celebrate.AddFrame(new Rectangle(728, 231, 156, 116), TimeSpan.FromSeconds(INTERVAL_TENTLE));
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
