using System;
using CorridorGravity.GameLogic.Animator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CorridorGravity.GameLogic.AnimatedEntity
{
    class Magic : AnimationEntity
    {
        public const double INTERVAL_PORTAL_PURPLE = .10; 
        public const double INTERVAL_PORTAL_BLACK = .10;
        public const double INTERVAL_TURBINE_BLACK = .10; 
        public const double INTERVAL_TURBINE_PURPLE = .25;

        public  Texture2D MagicSprite { get; }
        

        public Magic(ContentManager content)
        {
            MagicSprite = content.Load<Texture2D>("magic-white");
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
            Idle.AddFrame(new Rectangle(1, 1, 1, 1), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(413, 99, 66, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(10, 190, 66, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(87, 190, 66, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(174, 190, 66, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(262, 190, 66, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(337, 190, 66, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(423, 190, 66, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(9, 292, 66, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(78, 292, 66, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(142, 292, 66, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(208, 292, 64, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(271, 292, 62, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(340, 292, 58, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(393, 292, 54, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));
            Idle.AddFrame(new Rectangle(444, 292, 50, 60), TimeSpan.FromSeconds(INTERVAL_PORTAL_BLACK));

            // PurplePortal  (each 32x78) start in 212x99
            Idle.AddFrame(new Rectangle(1, 1, 1, 1), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(1, 541, 20, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(16, 541, 20, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(37, 541, 20, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(59, 541, 20, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(82, 541, 20, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(102, 541, 30, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(128, 541, 30, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(158, 541, 32, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(187, 541, 32, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(217, 541, 32, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(245, 541, 32, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(276, 541, 32, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(311, 541, 32, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(346, 541, 32, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(382, 541, 32, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(419, 541, 32, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(457, 541, 32, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(5, 577, 32, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(41, 577, 32, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(81, 577, 33, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(121, 577, 33, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(154, 577, 33, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));
            Walk.AddFrame(new Rectangle(184, 577, 33, 83), TimeSpan.FromSeconds(INTERVAL_PORTAL_PURPLE));

            // TurbinePurple  (each 41x92) start in 205x7 
            Jump.AddFrame(new Rectangle(254, 9, 36, 90), TimeSpan.FromSeconds(INTERVAL_TURBINE_PURPLE));
            Jump.AddFrame(new Rectangle(296, 9, 32, 90), TimeSpan.FromSeconds(INTERVAL_TURBINE_PURPLE));
            Jump.AddFrame(new Rectangle(332, 9, 36, 90), TimeSpan.FromSeconds(INTERVAL_TURBINE_PURPLE));
            Jump.AddFrame(new Rectangle(296, 9, 32, 90), TimeSpan.FromSeconds(INTERVAL_TURBINE_PURPLE));
            Jump.AddFrame(new Rectangle(254, 9, 36, 90), TimeSpan.FromSeconds(INTERVAL_TURBINE_PURPLE));
             
            // TurbineBlack  (each 70x78) start in 205x7 
            Dead.AddFrame(new Rectangle(0, 362, 70, 78), TimeSpan.FromSeconds(INTERVAL_TURBINE_BLACK));
            Dead.AddFrame(new Rectangle(60, 362, 70, 78), TimeSpan.FromSeconds(INTERVAL_TURBINE_BLACK));
            Dead.AddFrame(new Rectangle(136, 362, 70, 78), TimeSpan.FromSeconds(INTERVAL_TURBINE_BLACK));
            Dead.AddFrame(new Rectangle(218, 362, 70, 78), TimeSpan.FromSeconds(INTERVAL_TURBINE_BLACK));
            Dead.AddFrame(new Rectangle(304, 362, 70, 78), TimeSpan.FromSeconds(INTERVAL_TURBINE_BLACK));
            Dead.AddFrame(new Rectangle(389, 362, 70, 78), TimeSpan.FromSeconds(INTERVAL_TURBINE_BLACK));
            Dead.AddFrame(new Rectangle(6, 446, 70, 78), TimeSpan.FromSeconds(INTERVAL_TURBINE_BLACK));
            Dead.AddFrame(new Rectangle(86, 446, 70, 78), TimeSpan.FromSeconds(INTERVAL_TURBINE_BLACK));
            Dead.AddFrame(new Rectangle(171, 446, 70, 78), TimeSpan.FromSeconds(INTERVAL_TURBINE_BLACK));
            Dead.AddFrame(new Rectangle(255, 446, 70, 78), TimeSpan.FromSeconds(INTERVAL_TURBINE_BLACK));
            Dead.AddFrame(new Rectangle(339, 446, 70, 78), TimeSpan.FromSeconds(INTERVAL_TURBINE_BLACK));


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
