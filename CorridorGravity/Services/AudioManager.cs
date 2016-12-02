using System.Collections.Generic;

using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;

namespace CorridorGravity.Services
{
    public enum SongTypes
    {
        INTRO,
        LAUGH,
        INGAME,
        DEAD
    }

    /// <summary>
    /// Manage all game audio
    /// </summary>
    class AudioManager
    {
        ServiceLocator SLocator;
        TimeSpan Duration;
        DateTime Cooldown;

        Dictionary<SongTypes, Song>          SongList;
        Dictionary<SongTypes, SoundEffect>   SoundList;

        public float Volume { get { return MediaPlayer.Volume; } }

        /// <summary>
        /// Check whenether sound effect is still playing
        /// </summary>
        public bool IsSoundPlaying { get {
                if ((DateTime.Now - Cooldown).TotalSeconds > Duration.TotalSeconds)
                    return false;
                else return true;
            } }

        private static AudioManager _Instance;
        public static AudioManager Instance
        { get {
                if (_Instance == null)
                    _Instance = new AudioManager();
                return _Instance;
            } }

        private AudioManager() { }

        public void Initialize()
        {
            MediaPlayer.IsRepeating = true;

            SLocator = ServiceLocator.Instance;

            SongList = new Dictionary<SongTypes, Song>() {
                { SongTypes.INGAME, SLocator.PLManager.ALoad(SLocator.PLManager.AList[SongTypes.INGAME])},
                { SongTypes.INTRO,  SLocator.PLManager.ALoad(SLocator.PLManager.AList[SongTypes.INTRO])}
            };

            SoundList = new Dictionary<SongTypes, SoundEffect>() {
                { SongTypes.LAUGH,  SLocator.PLManager.SLoad(SLocator.PLManager.AList[SongTypes.LAUGH])},
                { SongTypes.INGAME, SLocator.PLManager.SLoad(SLocator.PLManager.AList[SongTypes.DEAD])}
            };
        }

        public void Play(SongTypes nextSong)
        {
            if (SongList.ContainsKey(nextSong))
                MediaPlayer.Play(SongList[nextSong]);
            else if(SoundList.ContainsKey(nextSong))
            {
                //if (nextSong == SongTypes.DEAD)
                //    MediaPlayer.IsRepeating = false;
                SoundList[nextSong].Play();
                Duration = SoundList[nextSong].Duration;
                Cooldown = DateTime.Now;
            }
        }

        public void Mute(float ratio)
        {
            MediaPlayer.Volume -= ratio;

            if (MediaPlayer.Volume < 0)
            {
                MediaPlayer.Volume = 0;
                MediaPlayer.Pause();
            }
        }

        public void MaxVolume(float ratio)
        {
            MediaPlayer.Volume += ratio;

            if (MediaPlayer.Volume > 0)
                MediaPlayer.Volume = 1;
        }

        public void PauseOrResume()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Pause();
            else if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}
