using System;
using CorridorGravity.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace CorridorGravity.GameLogic
{
    public class Animation
    {
        private List<AnimationFrame> FrameList = new List<AnimationFrame>();
        private TimeSpan TimeIntoAnimation;
        private int SingleAnimationCounter = 0;
        private DateTime SingleAnimationStartTime;
    
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

        public bool UpdateSingleAnimation(GameTime gameTime)
        {
            if (SingleAnimationCounter == 0)
            {
                SingleAnimationStartTime = DateTime.Now;
                SingleAnimationCounter = 1;
            }

            double secondsIntoAnimation =
                TimeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;

            double remainder = secondsIntoAnimation % Duration.TotalSeconds;

            TimeIntoAnimation = TimeSpan.FromSeconds(remainder);

            if ((DateTime.Now - SingleAnimationStartTime).TotalMilliseconds > Duration.TotalMilliseconds)
            {
                SingleAnimationCounter = 0;
                TimeIntoAnimation = TimeSpan.FromSeconds(0);
                return false;
            }
            else return true;
        }

        public void ResetTimer()
        {
            TimeIntoAnimation = TimeSpan.FromSeconds(0);
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
