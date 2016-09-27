using System;
using CorridorGravity.GameLogic.Animator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorridorGravity.GameLogic.AnimatedEntity
{
    class Magic : AnimationEntity
    {
        public const double INTERVAL_DEFAULT = .25;
        public const double INTERVAL_PORTAL = .25;
        public const double INTERVAL_APPEAR = .10;

        public Magic()
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

            // BlackPortal  (each 66x56) start in 320x104
            Idle.AddFrame(new Rectangle(413, 99, 66, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));
            Idle.AddFrame(new Rectangle(10, 190, 66, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));
            Idle.AddFrame(new Rectangle(87, 190, 66, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));
            Idle.AddFrame(new Rectangle(174, 190, 66, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));
            Idle.AddFrame(new Rectangle(262, 190, 66, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));
            Idle.AddFrame(new Rectangle(337, 190, 66, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));
            Idle.AddFrame(new Rectangle(423, 190, 66, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));
            Idle.AddFrame(new Rectangle(9, 292, 66, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));
            Idle.AddFrame(new Rectangle(78, 292, 66, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));
            Idle.AddFrame(new Rectangle(142, 292, 66, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));
            Idle.AddFrame(new Rectangle(208, 292, 64, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));
            Idle.AddFrame(new Rectangle(271, 292, 62, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));
            Idle.AddFrame(new Rectangle(330, 292, 58, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));
            Idle.AddFrame(new Rectangle(383, 292, 54, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));
            Idle.AddFrame(new Rectangle(434, 292, 50, 60), TimeSpan.FromSeconds(INTERVAL_APPEAR));

            // PurplePortal  (each 48x60) start in 212x99
            Walk.AddFrame(new Rectangle(212, 99, 44, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(50, 239, 44, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(99, 239, 44, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(149, 239, 44, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Walk.AddFrame(new Rectangle(199, 239, 44, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));

            // TurbinePurple  (each 41x92) start in 205x7 
            Jump.AddFrame(new Rectangle(254, 9, 36, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Jump.AddFrame(new Rectangle(296, 9, 32, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Jump.AddFrame(new Rectangle(332, 9, 36, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Jump.AddFrame(new Rectangle(296, 9, 32, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Jump.AddFrame(new Rectangle(254, 9, 36, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
             
            // TurbineBlack  (each 41x92) start in 205x7 
            Dead.AddFrame(new Rectangle(254, 9, 36, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Dead.AddFrame(new Rectangle(296, 9, 32, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Dead.AddFrame(new Rectangle(332, 9, 36, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Dead.AddFrame(new Rectangle(296, 9, 32, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));
            Dead.AddFrame(new Rectangle(254, 9, 36, 90), TimeSpan.FromSeconds(INTERVAL_DEFAULT));


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
