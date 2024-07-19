﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dogabeey.SimpleJSON;

namespace Dogabeey
{
    public class SoundManager : SingletonComponent<SoundManager>, ISaveable
    {
        #region Classes

        [System.Serializable]
        public class SoundInfo
        {
            public string id = "";
            public List<AudioClip> audioClips;
            public SoundType type = SoundType.SoundEffect;
            public bool playAndLoopOnStart = false;

            [Range(0, 1)] public float clipVolume = 1;
        }

        public class PlayingSound
        {
            public SoundInfo soundInfo = null;
            public AudioSource audioSource = null;
        }

        #endregion

        #region Enums

        public enum SoundType
        {
            SoundEffect,
            Music
        }



        #endregion

        #region Inspector Variables

        [SerializeField] private List<SoundInfo> soundInfos = null;

        #endregion

        #region Member Variables

        internal List<PlayingSound> playingAudioSources;
        internal List<PlayingSound> loopingAudioSources;

        public string SaveId { get { return "sound_manager"; } }

        #endregion

        #region Properties

        public bool IsMusicOn { get; private set; }
        public bool IsSoundEffectsOn { get; private set; }
        public bool IsVibrationOn { get; private set; }
        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            SaveManager.Instance.Register(this);

            playingAudioSources = new List<PlayingSound>();
            loopingAudioSources = new List<PlayingSound>();

            if (!LoadSave())
            {
                IsMusicOn = true;
                IsSoundEffectsOn = true;
                IsVibrationOn = true;
            }

        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < soundInfos.Count; i++)
            {
                SoundInfo soundInfo = soundInfos[i];

                if (soundInfo.playAndLoopOnStart)
                {
                    Play(soundInfo.id, true, 0);
                }
            }

            yield break;
        }

        private void Update()
        {
            for (int i = 0; i < playingAudioSources.Count; i++)
            {
                AudioSource audioSource = playingAudioSources[i].audioSource;

                // If the Audio Source is no longer playing then return it to the pool so it can be re-used
                if (!audioSource.isPlaying)
                {
                    Destroy(audioSource.gameObject);
                    playingAudioSources.RemoveAt(i);
                    i--;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Plays the sound with the give id
        /// </summary>
        public void Play(string id)
        {
            Play(id, false, 0);
        }

        /// <summary>
        /// Plays the sound with the give id, if loop is set to true then the sound will only stop if the Stop method is called
        /// </summary>
        public void Play(string id, bool loop = false, float playDelay = 0, float volumeMultiplier = 1, float pitchOffset = 0)
        {
            SoundInfo soundInfo = GetSoundInfo(id);

            if (soundInfo == null)
            {
                Debug.LogError("[SoundManager] There is no Sound Info with the given id: " + id);

                return;
            }

            if ((soundInfo.type == SoundType.Music && !IsMusicOn) ||
                (soundInfo.type == SoundType.SoundEffect && !IsSoundEffectsOn))
            {
                return;
            }

            AudioSource audioSource = CreateAudioSource(id);

            audioSource.clip = soundInfo.audioClips.GetRandomElement();
            audioSource.loop = loop;
            audioSource.time = 0;
            audioSource.volume = soundInfo.clipVolume * volumeMultiplier;
            audioSource.pitch += pitchOffset;


            if (playDelay > 0)
            {
                audioSource.PlayDelayed(playDelay);
            }
            else
            {
                audioSource.Play();
            }

            PlayingSound playingSound = new PlayingSound();

            playingSound.soundInfo = soundInfo;
            playingSound.audioSource = audioSource;

            if (loop)
            {
                loopingAudioSources.Add(playingSound);
            }
            else
            {
                playingAudioSources.Add(playingSound);
            }
        }

        /// <summary>
        /// Stops all playing sounds with the given id
        /// </summary>
        public void Stop(string id)
        {
            StopAllSounds(id, playingAudioSources);
            StopAllSounds(id, loopingAudioSources);
        }

        /// <summary>
        /// Stops all playing sounds with the given type
        /// </summary>
        public void Stop(SoundType type)
        {
            StopAllSounds(type, playingAudioSources);
            StopAllSounds(type, loopingAudioSources);
        }

        /// <summary>
        /// Sets the SoundType on/off
        /// </summary>
        public void SetSoundTypeOnOff(SoundType type, bool isOn)
        {
            switch (type)
            {
                case SoundType.SoundEffect:

                    if (isOn == IsSoundEffectsOn)
                    {
                        return;
                    }

                    IsSoundEffectsOn = isOn;

                    break;
                case SoundType.Music:

                    if (isOn == IsMusicOn)
                    {
                        return;
                    }

                    IsMusicOn = isOn;

                    // If it was turned off then stop all sounds that are currently playing
                    if (!isOn)
                    {
                        Stop(type);
                    }
                    // Else it was turned on so play any sounds that have playAndLoopOnStart set to true
                    else
                    {
                        PlayAtStart(type);
                    }

                    break;
            }


        }

        public void SetVibrationOnOff(bool isOn)
        {
            if (isOn == IsVibrationOn)
            {
                return;
            }

            IsVibrationOn = isOn;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Plays all sounds that are set to play on start and loop and are of the given type
        /// </summary>
        private void PlayAtStart(SoundType type)
        {
            for (int i = 0; i < soundInfos.Count; i++)
            {
                SoundInfo soundInfo = soundInfos[i];

                if (soundInfo.type == type)
                {
                    Play(soundInfo.id, true, 0);
                }
            }
        }

        /// <summary>
        /// Stops all sounds with the given id
        /// </summary>
        private void StopAllSounds(string id, List<PlayingSound> playingSounds)
        {
            for (int i = 0; i < playingSounds.Count; i++)
            {
                PlayingSound playingSound = playingSounds[i];

                if (id == playingSound.soundInfo.id)
                {
                    playingSound.audioSource.Stop();
                    Destroy(playingSound.audioSource.gameObject);
                    playingSounds.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Stops all sounds with the given type
        /// </summary>
        private void StopAllSounds(SoundType type, List<PlayingSound> playingSounds)
        {
            for (int i = 0; i < playingSounds.Count; i++)
            {
                PlayingSound playingSound = playingSounds[i];

                if (type == playingSound.soundInfo.type)
                {
                    playingSound.audioSource.Stop();
                    Destroy(playingSound.audioSource.gameObject);
                    playingSounds.RemoveAt(i);
                    i--;
                }
            }
        }

        private SoundInfo GetSoundInfo(string id)
        {
            for (int i = 0; i < soundInfos.Count; i++)
            {
                if (id == soundInfos[i].id)
                {
                    return soundInfos[i];
                }
            }

            return null;
        }

        private AudioSource CreateAudioSource(string id)
        {
            GameObject obj = new GameObject("sound_" + id);

            obj.transform.SetParent(transform);

            return obj.AddComponent<AudioSource>(); ;
        }

        #endregion

        #region Save Methods

        public Dictionary<string, object> Save()
        {
            Dictionary<string, object> json = new Dictionary<string, object>();

            json["is_music_on"] = IsMusicOn;
            json["is_sound_effects_on"] = IsSoundEffectsOn;
            json["is_vibration_on"] = IsVibrationOn;
            return json;
        }

        public bool LoadSave()
        {
            JSONNode json = SaveManager.Instance.LoadSave(this);

            if (json == null)
            {
                return false;
            }

            IsMusicOn = json["is_music_on"].AsBool;
            IsSoundEffectsOn = json["is_sound_effects_on"].AsBool;
            IsVibrationOn = json["is_vibration_on"].AsBool;
            return true;
        }

        #endregion
    }
}