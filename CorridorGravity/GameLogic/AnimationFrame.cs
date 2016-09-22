using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CorridorGravity.GameLogic
{
    public class AnimationFrame
    {
        public Rectangle SourceRectangle { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
