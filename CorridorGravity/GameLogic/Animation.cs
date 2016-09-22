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
        private List<AnimationFrame> frames = new List<AnimationFrame>();
        private TimeSpan timeIntoAnimation;
    
        TimeSpan Duration {
            get {
                double totalSeconds = 0;
                foreach (var frame in frames)
                    totalSeconds += frame.Duration.TotalSeconds;
                return TimeSpan.FromSeconds(totalSeconds);
            }
        }

        public void AddFrame(Rectangle rectangle, TimeSpan duration) {
            frames.Add(new AnimationFrame() {
                SourceRectangle = rectangle,
                Duration = duration
            });
        }

        public void Update(GameTime gameTime) {
            double secondsIntoAnimation =
                timeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;

            double remainder = secondsIntoAnimation % Duration.TotalSeconds;

            timeIntoAnimation = TimeSpan.FromSeconds(remainder);
        }

        public Rectangle CurrentRectangle {
            get {
                AnimationFrame currentFrame = null;
                TimeSpan accumulatedTime = new TimeSpan();

                foreach (var frame in frames) {
                    if (accumulatedTime + frame.Duration >= timeIntoAnimation) {
                        currentFrame = frame;
                        break;
                    }
                    else accumulatedTime += frame.Duration;
                }

                if (currentFrame == null)
                    currentFrame = frames.LastOrDefault();
                if (currentFrame != null)
                    return currentFrame.SourceRectangle;
                else
                    return Rectangle.Empty;
            }
        }
    }
}
