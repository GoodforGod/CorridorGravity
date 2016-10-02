using System; 
using CorridorGravity.GameLogic.Animator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace CorridorGravity.GameLogic.AnimatedEntity
{
    class Bob : AnimationEntity
    {
        public const double INTERVAL_DEFAULT = .15;
        public const double INTERVAL_ATTACK = .10;
        public const double INTERVAL_ULTIMATE = .15;
        public const int ULTIMATE_LENGTH = 5;

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
            Portrait = new Animation();
            Health = new Animation();
            Ultimate = new Animation();

            // Idle 192x84 (each 48x84) start in 6x15
            Idle.AddFrame(new Rectangle(6, 15, 48, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(54, 15, 48, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(102, 15, 48, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(150, 15, 48, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(102, 15, 48, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Idle.AddFrame(new Rectangle(54, 15, 48, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));

            // Walk 240x82 (each 60x82) start in 3x239
            Walk.AddFrame(new Rectangle(3, 239, 44, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
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
            Dead.AddFrame(new Rectangle(294, 133, 60, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Dead.AddFrame(new Rectangle(353, 129, 74, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Dead.AddFrame(new Rectangle(430, 130, 78, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));

            // StrikeOne (first two 100x86, (each 50x86) start in 2x129), 
            // thrid (75x86 start in 107x130), 
            // fourth and fifth equal first two.
            StrikeOne.AddFrame(new Rectangle(1, 129, 50, 86), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(51, 129, 50, 86), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(107, 130, 75, 86), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(51, 129, 50, 86), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeOne.AddFrame(new Rectangle(1, 129, 50, 86), TimeSpan.FromSeconds(INTERVAL_ATTACK));

            // StrikeTwo first is Walk frame
            //  First (48x84 start in 3x368)
            // Second (80x96 start in 57x357)
            // Third (54x112 start in 140x341)
            // Forth and fifth equal second and first
            StrikeTwo.AddFrame(new Rectangle(4, 105, 44, 112), TimeSpan.FromSeconds(INTERVAL_ATTACK));  // First frame of walk anim
            StrikeTwo.AddFrame(new Rectangle(3, 341, 50, 112), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeTwo.AddFrame(new Rectangle(58, 341, 80, 112), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeTwo.AddFrame(new Rectangle(142, 341, 53, 112), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeTwo.AddFrame(new Rectangle(58, 341, 80, 112), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeTwo.AddFrame(new Rectangle(3, 341, 50, 112), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            StrikeTwo.AddFrame(new Rectangle(4, 105, 44, 112), TimeSpan.FromSeconds(INTERVAL_ATTACK));  // First frame of walk anim

            // JumpStrike First (37x64 start in 342x398)
            // Second (60x64 start in 384x399)
            // Third equal first
            JumpStrike.AddFrame(new Rectangle(342, 398, 37, 64), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            JumpStrike.AddFrame(new Rectangle(384, 399, 60, 64), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            JumpStrike.AddFrame(new Rectangle(342, 398, 37, 64), TimeSpan.FromSeconds(INTERVAL_ATTACK));

            // Celebrate (144x88 (each 36x88) start in 340x499)
            Celebrate.AddFrame(new Rectangle(340, 5001, 45, 88), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Celebrate.AddFrame(new Rectangle(389, 499, 36, 88), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Celebrate.AddFrame(new Rectangle(440, 499, 36, 88), TimeSpan.FromSeconds(INTERVAL_DEFAULT));

            Portrait.AddFrame(new Rectangle(543, 531, 23, 34), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
 
            Health.AddFrame(new Rectangle(440, 499, 36, 38), TimeSpan.FromSeconds(INTERVAL_DEFAULT));

            // Ultimate (each 64x120) start in 0x608
            Ultimate.AddFrame(new Rectangle(550, 608, 48, 120), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            Ultimate.AddFrame(new Rectangle(0, 608, 65, 120), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            Ultimate.AddFrame(new Rectangle(62, 608, 67, 120), TimeSpan.FromSeconds(INTERVAL_ATTACK));

            // Loop start
            for (int i = 0; i < ULTIMATE_LENGTH; i++)
            { 
                Ultimate.AddFrame(new Rectangle(125, 608, 78, 120), TimeSpan.FromSeconds(INTERVAL_ULTIMATE));
                Ultimate.AddFrame(new Rectangle(211, 608, 65, 120), TimeSpan.FromSeconds(INTERVAL_ULTIMATE));
                Ultimate.AddFrame(new Rectangle(307, 608, 67, 120), TimeSpan.FromSeconds(INTERVAL_ULTIMATE));
                Ultimate.AddFrame(new Rectangle(386, 608, 75, 120), TimeSpan.FromSeconds(INTERVAL_ULTIMATE));
            }
            // Loop end

            Ultimate.AddFrame(new Rectangle(476, 608, 48, 120), TimeSpan.FromSeconds(INTERVAL_ATTACK));
            Ultimate.AddFrame(new Rectangle(550, 608, 48, 120), TimeSpan.FromSeconds(INTERVAL_ATTACK));

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

        public Animation Ultimate { get; }
    }
}
