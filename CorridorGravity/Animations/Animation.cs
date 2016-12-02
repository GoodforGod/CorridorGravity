using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace CorridorGravity.Animations
{
    /// <summary>
    /// Indicates current animation state/states
    /// </summary> 
    [Flags]
    public enum AnimationState
    {
        NONE = 0,
        DEAD = 1,
        IDLE = DEAD << 1,
        WALK = IDLE << 1,
        JUMP = WALK << 1,
        FALL = JUMP << 1,
        DUCK = FALL << 1,
        FIRE = DUCK << 1,
        FIRE_OTHER      = FIRE << 1,
        FIRE_SPECIAL    = FIRE_OTHER << 1,
        FIRE_AIR        = FIRE_SPECIAL << 1,
        CELEBRATE       = FIRE_AIR << 1
    }

    /// <summary>
    /// Class stores all frames <see cref="Frame"/> for the specific animation, is used for entity
    /// </summary>
    /// <remarks>
    /// TimeSpan for animation duration <see cref="Duration"/> 
    /// TimeSpan for time elapsed into animation <see cref="TimeIntoAnimation"/>
    /// List of Frames in animation <see cref="AFrame"/>, 
    /// SpriteEffects for animation <see cref="Effect"/>
    /// Current rectangle of the animation frame <see cref="Source"/>
    /// Method to add animations <see cref="AddFrame(Rectangle, TimeSpan)"/>
    /// </remarks>
    class Animation
    {
        /// <summary>
        /// List of Frames in animation <see cref="Frame"/>
        /// </summary>
        public List<AFrame> FrameList           { get; private set; }

        /// <summary>
        ///  TimeSpan for time elapsed into animation
        /// </summary>
        public TimeSpan     TimeIntoAnimation   { get; set; }
        
        /// <summary>
        /// Represents the start time of the animation (used like timer)
        /// </summary>
        public DateTime     StartTime           { get; set; }

        private double     _TimeCorrention { get; set; }
        /// <summary>
        /// Amout of milliseconds needed to corrent animation time and make transition smoother
        /// </summary>
        public double       TimeCorrention
        { get {
                if (_TimeCorrention == .0)
                    _TimeCorrention = Duration.TotalSeconds / 100;
                return _TimeCorrention;
            } }

        /// <summary>
        /// FALSE - means animation is ended,
        /// TRUE  - means animation is playing
        /// </summary>
        public bool         IsPlaying           { get; set; }

        /// <summary>
        /// SpriteEffects for animation
        /// </summary>
        public SpriteEffects Effect             { get; private set; }

        public Animation() { FrameList = new List<AFrame>(); }
        public Animation(SpriteEffects effect) : this() { Effect = effect; }

        /// <summary>
        /// Represents the total duration of the animation in TotalSeconds
        /// </summary>
        public TimeSpan Duration
        { get {
                double totalSeconds = 0;
                foreach (var frame in FrameList)
                    totalSeconds += frame.Duration.TotalSeconds;
                return TimeSpan.FromSeconds(totalSeconds);
            } }

        /// <summary>
        /// Uses <see cref="TimeIntoAnimation"/> to go through all frames and get the Rectangle of the current frame
        /// </summary>
        public Rectangle Source
        { get {
                AFrame currentFrame = null;
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
                    return currentFrame.Source;
                else
                    return Rectangle.Empty;
            } }


        /// <summary>
        /// Gets rectangle of the sprite, and the duration of the frame, creates new frame in animation
        /// </summary>
        public void AddFrame(Rectangle source, TimeSpan duration)
        {
            FrameList.Add(new AFrame()
            {
                Source = source,
                Duration = duration
            });
        }

        public void Reset() { TimeIntoAnimation = TimeSpan.FromSeconds(0); }
    }
}
