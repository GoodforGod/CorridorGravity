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
    class Alice : EntityAnimation
    {
        public const double INTERVAL = .25;

        public Alice()
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

            // Idle
            Idle.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Idle.AddFrame(new Rectangle(64, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Idle.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));

            // Walk
            Walk.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Walk.AddFrame(new Rectangle(64, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Walk.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Walk.AddFrame(new Rectangle(80, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));

            // Jump
            Jump.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Jump.AddFrame(new Rectangle(64, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Jump.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Jump.AddFrame(new Rectangle(80, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Jump.AddFrame(new Rectangle(80, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));

            // Intro
            Intro.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Intro.AddFrame(new Rectangle(64, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Intro.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Intro.AddFrame(new Rectangle(80, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Intro.AddFrame(new Rectangle(80, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));

            // Dead
            Dead.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Dead.AddFrame(new Rectangle(64, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Dead.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Dead.AddFrame(new Rectangle(80, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));

            // StrikeOne
            StrikeOne.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            StrikeOne.AddFrame(new Rectangle(64, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            StrikeOne.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            StrikeOne.AddFrame(new Rectangle(80, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));

            // StrikeTwo
            StrikeTwo.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            StrikeTwo.AddFrame(new Rectangle(64, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            StrikeTwo.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            StrikeTwo.AddFrame(new Rectangle(80, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));

            // JumpStrike
            JumpStrike.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            JumpStrike.AddFrame(new Rectangle(64, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            JumpStrike.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            JumpStrike.AddFrame(new Rectangle(80, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));

            // Celebrate
            Celebrate.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Celebrate.AddFrame(new Rectangle(64, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Celebrate.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));
            Celebrate.AddFrame(new Rectangle(80, 0, 16, 16), TimeSpan.FromSeconds(INTERVAL));

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
