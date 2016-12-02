using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CorridorGravity.Services;

namespace CorridorGravity.Animations
{
    /// <summary>
    /// Manages all Animations for the specific entity object (Generic class)
    /// </summary>
    /// <typeparam name="TEnum">
    /// Represent flexible enum for animations states (Generic method)
    /// (each animation type could have specific Enum State for it) like <see cref="AnimationState"/>
    /// </typeparam>
    class AnimationManager<TEnum>
         where TEnum : struct, IConvertible
    {
        /// <summary>
        /// Collection where each state like <see cref="AnimationState"/> represents
        /// with the specific Animation for it <see cref="Animation"/>
        /// </summary>
        public Dictionary<TEnum, Animation> Animations  { get; private set; }
        public TimeSpan         CTimeIntoAnimation      { get { return CAnimation.TimeIntoAnimation; } }
        public TimeSpan         CDuration   { get { return CAnimation.Duration; } }

        /// <summary>
        /// Represents current animation effect <see cref="Animation.Effect"/>
        /// </summary>
        public SpriteEffects    CEffect     { get { return CAnimation.Effect; } }

        /// <summary>
        /// Represents current animation source rectangle <see cref="AFrame.Source"/>
        /// </summary>
        public Rectangle        CSource     { get { return CAnimation.Source; } }

        /// <summary>
        /// Represents current animation <see cref="Animation"/>
        /// </summary>
        Animation               CAnimation  { get; set; }

        /// <summary>
        /// Remembers previous animation state for more info see implementation
        /// <see cref="UpdateOnce(TEnum, GameTime)"/> or <see cref="UpdateCycle(TEnum, GameTime)"/> 
        /// </summary>
        TEnum PState;

        public AnimationManager() { Initialize(); }

        public void Initialize()
        {
            this.CAnimation = null;
            this.Animations = new Dictionary<TEnum, Animation>();
        }

        /// <summary>
        /// Call loader to fill the Animations collection
        /// </summary>
        /// <param name="loader">
        /// Loader <see cref="AnimationLoader"/> given by <see cref="Services.ServiceLocator"/>
        /// </param>
        /// <param name="fileName">
        /// Name of the texture/sprite for the animations
        /// </param>
        public void Load(string fileName)
        {
            Animations = ServiceLocator.Instance.ALoader.LoadAnimations<TEnum>(fileName);

            if (Animations == null || Animations.Count == 0)
                throw new NullReferenceException("Animations is NULL or EMPTY, check file: " + fileName);
            else
                CAnimation = Animations.FirstOrDefault().Value;
        }

        /// <summary>
        /// Updates Animation that should play once and then stop
        /// </summary>
        /// <param name="state">
        /// Get the animation to update from <see cref="Animations"/> by this state (where state is the key)
        /// </param>
        /// <returns> <see cref="Animation.IsPlaying"/> </returns>
        public bool UpdateOnce(TEnum state, GameTime gameTime)
        {
            // Check if we play the same animation, if new one, then reset previous animation and prev animation state
            if(!PState.Equals(state))
            {
                PState = state;
                CAnimation.Reset();
            }

            if (Animations.ContainsKey(state))
                CAnimation = Animations[state];

            // If animation starts from the begining, reset cooldown and set playing flag
            if (!CAnimation.IsPlaying)
            {
                CAnimation.StartTime = DateTime.Now;
                CAnimation.IsPlaying = true;
            }

            double secondsIntoAnimation =
                CAnimation.TimeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;

            double remainder =
                secondsIntoAnimation % CAnimation.Duration.TotalSeconds;

            CAnimation.TimeIntoAnimation = TimeSpan.FromSeconds(remainder);

            if ((DateTime.Now - CAnimation.StartTime).TotalMilliseconds
                - CAnimation.TimeCorrention > CAnimation.Duration.TotalMilliseconds)
            {
                CAnimation.Reset();
                return CAnimation.IsPlaying = false;
            }
            else return CAnimation.IsPlaying;
        }

        /// <summary>
        /// Updates Animations in a cycle
        /// </summary>
        /// <param name="state">
        /// Get the animation to update from <see cref="Animations"/> by this state (where state is the key)
        /// </param>
        public void UpdateCycle(TEnum state, GameTime gameTime)
        {
            // Check if we play the same animation, if new one, then reset previous animation and prev animation staten
            if (!PState.Equals(state))
            {
                PState = state;
                CAnimation.Reset();
            }

            CAnimation = Animations[state];

            double secondsIntoAnimation =
                CAnimation.TimeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;

            double remainder =
                secondsIntoAnimation % CAnimation.Duration.TotalSeconds;

            CAnimation.TimeIntoAnimation = TimeSpan.FromSeconds(remainder);
        }

        public void Unload()
        {
            Animations.Clear();
        }
    }
}
