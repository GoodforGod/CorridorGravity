using System;
using CorridorGravity.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace CorridorGravity.GameLogic
{
    public class Animation
    {
        private List<AnimationFrame> FrameList = new List<AnimationFrame>();
        private TimeSpan TimeIntoAnimation;
    
        TimeSpan Duration {
            get {
                double totalSeconds = 0;
                foreach (var frame in FrameList)
                    totalSeconds += frame.Duration.TotalSeconds;
                return TimeSpan.FromSeconds(totalSeconds);
            }
        }

        public void AddFrame(Rectangle rectangle, TimeSpan duration) {
            FrameList.Add(new AnimationFrame() {
                SourceRectangle = rectangle,
                Duration = duration
            });
        }

        public void Update(GameTime gameTime) {
            double secondsIntoAnimation =
                TimeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;

            double remainder = secondsIntoAnimation % Duration.TotalSeconds;

            TimeIntoAnimation = TimeSpan.FromSeconds(remainder);
        }

        public Rectangle CurrentRectangle {
            get {
                AnimationFrame currentFrame = null;
                TimeSpan accumulatedTime = new TimeSpan();

                foreach (var frame in FrameList) {
                    if (accumulatedTime + frame.Duration >= TimeIntoAnimation) {
                        currentFrame = frame;
                        break;
                    }
                    else accumulatedTime += frame.Duration;
                }

                if (currentFrame == null)
                    currentFrame = FrameList.LastOrDefault();
                if (currentFrame != null)
                    return currentFrame.SourceRectangle;
                else
                    return Rectangle.Empty;
            }
        }
    }
}
