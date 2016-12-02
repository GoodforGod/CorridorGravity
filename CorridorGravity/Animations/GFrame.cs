using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorridorGravity.Animations
{
    /// <summary>
    /// Frame used for glide animation, represent specific frame with glide coordinates
    /// </summary>
    class GFrame
    {
        public GFrame() { }
        public GFrame(int offset, TimeSpan duration)
        {
            this.Offset = offset;
            this.Duration = duration;
        }

        /// <summary>
        /// Represents offset of glide animation frame
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Is the duration of the frame into animation
        /// </summary>
        public TimeSpan Duration { get; set; }
    }
}
