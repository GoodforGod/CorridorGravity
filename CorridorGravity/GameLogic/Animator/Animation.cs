﻿using System;
using CorridorGravity.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 
using System.Collections.Generic;
using System.Linq;

namespace CorridorGravity.GameLogic
{
    public class Animation
    {
        private List<AnimationFrame> FrameList = new List<AnimationFrame>();
        private TimeSpan TimeIntoAnimation;
        private int SingleAnimationCounter = 0;
        private DateTime SingleAnimationStartTime;

         public TimeSpan Duration {
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

        public double UpdateSingleAnimationTimeSlapsed(GameTime gameTime)
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

            var TimeElapsed = (DateTime.Now - SingleAnimationStartTime).TotalMilliseconds - Duration.TotalMilliseconds / 100;

            if (TimeElapsed > Duration.TotalMilliseconds)
            {
                SingleAnimationCounter = 0;
                TimeIntoAnimation = TimeSpan.FromSeconds(0);
                return -1;
            }
            else return TimeElapsed;
        }

        public bool UpdateSingleAnimationIsEnded(GameTime gameTime)
        {
            if (SingleAnimationCounter == 0) {
                SingleAnimationStartTime = DateTime.Now;
                SingleAnimationCounter = 1;
            } 
            double secondsIntoAnimation =
                TimeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;

            double remainder = secondsIntoAnimation % Duration.TotalSeconds; 

            TimeIntoAnimation = TimeSpan.FromSeconds(remainder);

            if ((DateTime.Now - SingleAnimationStartTime).TotalMilliseconds - Duration.TotalMilliseconds/100 > Duration.TotalMilliseconds) {
                SingleAnimationCounter = 0;
                TimeIntoAnimation = TimeSpan.FromSeconds(0);
                return false;
            }
            else return true;
        }

        public void UpdateCycleAnimation(GameTime gameTime) {
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
