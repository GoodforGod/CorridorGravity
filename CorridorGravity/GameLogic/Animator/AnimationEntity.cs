using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorridorGravity.GameLogic.Animator;

namespace CorridorGravity.GameLogic.Animator
{
    abstract class AnimationEntity
    {
        public const double DEFAULT_INTERVAL = 0.15;

        public abstract Animation Idle { get; }

        public abstract Animation Walk { get; }

        public abstract Animation Jump { get; }

        public abstract Animation Intro { get; }

        public abstract Animation Dead { get; }

        public abstract Animation StrikeOne { get; }

        public abstract Animation StrikeTwo { get; }

        public abstract Animation JumpStrike { get; }

        public abstract Animation Celebrate { get; }

        public abstract Animation Portrait { get; }

        public abstract Animation Health { get; }
    }
}
