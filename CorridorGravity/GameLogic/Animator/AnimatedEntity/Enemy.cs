using System; 
using CorridorGravity.GameLogic.Animator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace CorridorGravity.GameLogic.AnimatedEntity
{
    class Enemy : AnimationEntity
    {
        public const double INTERVAL_DEFAULT = .15;
        public const double INTERVAL_ATTACK = .25;

        public Enemy()
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

            // Idle (each 48x60) start in 212x99 
            Idle.AddFrame(new Rectangle(122, 418, 42, 64), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(66, 416, 42, 64), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(122, 418, 42, 64), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            //Idle.AddFrame(new Rectangle(212, 94, 50, 66), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            //Idle.AddFrame(new Rectangle(272, 94, 50, 66), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            //Idle.AddFrame(new Rectangle(332, 94, 50, 66), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            //Idle.AddFrame(new Rectangle(396, 94, 50, 66), TimeSpan.FromSeconds(INTERVAL_DEFAULT));

            // Walk (each 42x62) start in 9x338
            Walk.AddFrame(new Rectangle(9, 338, 42, 62), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(63, 338, 42, 62), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(117, 338, 42, 62), TimeSpan.FromSeconds(INTERVAL_DEFAULT));

            // Jump (each 42x62) start in 7x175
            Jump.AddFrame(new Rectangle(7, 175, 42, 64), TimeSpan.FromSeconds(INTERVAL_DEFAULT));

            // Intro equal Celebrate

            // Dead (each 68x60) start in 196x403
            Dead.AddFrame(new Rectangle(196, 403, 68, 60), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Dead.AddFrame(new Rectangle(285, 403, 68, 60), TimeSpan.FromSeconds(INTERVAL_DEFAULT));

            // StrikeOne
            StrikeOne.AddFrame(new Rectangle(0, 170, 54, 68), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(65, 170, 54, 68), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(0, 170, 54, 68), TimeSpan.FromSeconds(INTERVAL_ATTACK));

            // StrikeTwo Firs 

            // JumpStrike equal StrikeOne
            JumpStrike.AddFrame(new Rectangle(0, 170, 54, 68), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            JumpStrike.AddFrame(new Rectangle(65, 170, 54, 68), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            JumpStrike.AddFrame(new Rectangle(0, 170, 54, 68), TimeSpan.FromSeconds(INTERVAL_ATTACK));

            // Celebrate (144x88 (each 36x88) start in 340x499)
            Celebrate.AddFrame(new Rectangle(122, 418, 42, 64), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Celebrate.AddFrame(new Rectangle(66, 416, 42, 64), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Celebrate.AddFrame(new Rectangle(122, 418, 42, 64), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
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
