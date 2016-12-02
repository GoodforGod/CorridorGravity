using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CorridorGravity.Animations
{
    /// <summary>
    /// Represents signle frame into animation <see cref="Animation"/>
    /// </summary>
    class AFrame
    {
        public AFrame() { }
        public AFrame(Rectangle source, TimeSpan duration)
        {
            this.Source = source;
            this.Duration = duration;
        }

        /// <summary>
        /// Is the source rectangle of the animation frame
        /// </summary>
        public Rectangle Source { get; set; }
    
        /// <summary>
        /// Is the duration of the frame into animation
        /// </summary>
        public TimeSpan Duration { get; set; }
    }
}
