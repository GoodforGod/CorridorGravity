using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CorridorGravity.Animations
{
    /// <summary>
    /// 
    /// </summary>
    class GlideAnimation
    {
        /// <summary>
        /// Collection of all glide frames
        /// </summary>
        List<GFrame> FrameList = new List<GFrame>();

        /// <summary>
        ///  TimeSpan for time elapsed into animation
        /// </summary>
        public TimeSpan TimeIntoAnimation { get; set; }

        /// <summary>
        /// Duration of the glide animation
        /// </summary>
        public TimeSpan Duration
        { get {
                double totalSeconds = 0;
                foreach (var frame in FrameList)
                    totalSeconds += frame.Duration.TotalSeconds;
                return TimeSpan.FromSeconds(totalSeconds);
            } }

        /// <summary>
        /// Current glide animation offest
        /// </summary>
        public int COffset
        { get {
                GFrame currentFrame = null;
                var accumulatedTime = new TimeSpan();

                foreach (var frame in FrameList)
                {
                    if (accumulatedTime + frame.Duration >= TimeIntoAnimation)
                    {
                        currentFrame = frame;
                        break;
                    }
                    else accumulatedTime += frame.Duration;
                }

                if (currentFrame == null)
                    currentFrame = FrameList.LastOrDefault();
                if (currentFrame != null)
                    return currentFrame.Offset;
                else
                    return 0;
            } }

        public GlideAnimation(int amount, int step = 2, double interval = 0.25)
        {
            Initialize(amount, step, interval);
        }

        public void Initialize(int amount, int step = 2, double interval = 0.25)
        {
            for (int i = 1; i < amount; i++)
                FrameList.Add(new GFrame(step * i, TimeSpan.FromSeconds(interval)));
            for (int i = amount; i > 1; i--)
                FrameList.Add(new GFrame(step * i, TimeSpan.FromSeconds(interval)));
        }

        public void Update(GameTime gameTime)
        {
            double secondsIntoAnimation = TimeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;

            double remainder = secondsIntoAnimation % Duration.TotalSeconds;

            TimeIntoAnimation = TimeSpan.FromSeconds(remainder);
        }

        public void Reset() { TimeIntoAnimation = TimeSpan.FromSeconds(0); }
    }
}
