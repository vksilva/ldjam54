using System;
using System.Collections.Generic;
using System.Linq;
using Busta.AppCore.Configurations;
using Busta.AppCore.State;
using Busta.Extensions;
using UnityEngine;

namespace Busta.AppCore.Audio
{
    public class AudioService
    {
        private Dictionary<AudioMusicEnum, AudioClip> musicMap = new();
        private Dictionary<AudioSFXEnum, AudioClip> sfxMap = new();

        private AudioSource MusicSource;
        private readonly List<AudioSource> SfxSources = new();

        private AudioMusicEnum currentMusic = AudioMusicEnum.none;
        private AudioMusicEnum previousMusic = AudioMusicEnum.none;

        private bool musicOff;
        private bool sfxOff;

        public void Init(AudioConfigurations configurations, StateService stateService, GameObject applicationObject)
        {
            musicMap = configurations.musicConfigEntries.ToDictionary(x => x.name, x => x.audioClip);
            sfxMap = configurations.sfxConfigEntries.ToDictionary(x => x.name, x => x.audioClip);
            
            MusicSource = applicationObject.CreateChildObject<AudioSource>("musicSource");
            MusicSource.playOnAwake = false;
            MusicSource.loop = true;

            for (var i = 0; i < configurations.sfxSourcesCount; i++)
            {
                var sfxSource = applicationObject.CreateChildObject<AudioSource>($"SfxSource{i}");
                SfxSources.Add(sfxSource);
                sfxSource.playOnAwake = false;
                sfxSource.loop = false;
            }

            musicOff = stateService.gameState.settingsState.isMusicOff;
            sfxOff = stateService.gameState.settingsState.isSFXOff;
        }

        public void PlayMusic(AudioMusicEnum music)
        {
            if (musicOff)
            {
                previousMusic = music;
                return;
            }

            if (music == currentMusic)
            {
                return;
            }

            StopMusic();

            if (musicMap.TryGetValue(music, out var clip))
            {
                currentMusic = music;
                MusicSource.clip = clip;
                MusicSource.Play();
            }
            else
            {
                throw new Exception($"Music {music} not configured.");
            }
        }

        public void ResumeMusic()
        {
            PlayMusic(previousMusic);
        }

        public void StopMusic()
        {
            if (currentMusic == AudioMusicEnum.none)
            {
                return;
            }

            previousMusic = currentMusic;
            currentMusic = AudioMusicEnum.none;
            MusicSource.Stop();
        }

        public void SetMusicOff(bool off)
        {
            musicOff = off;
            if (off)
            {
                StopMusic();
            }
            else
            {
                ResumeMusic();
            }
        }

        public void SetSfxOff(bool off)
        {
            sfxOff = off;
            if (off)
            {
                StopAllSounds();
            }
        }

        public SfxHandler PlaySfx(AudioSFXEnum sfx)
        {
            if (sfxOff)
            {
                return new SfxHandler(null);
            }

            foreach (var sfxSource in SfxSources)
            {
                if (sfxSource.isPlaying)
                {
                    continue;
                }

                if (sfxMap.TryGetValue(sfx, out var clip))
                {
                    sfxSource.clip = clip;
                    sfxSource.Play();
                    return new SfxHandler(sfxSource);
                }

                throw new Exception($"Sound effect {sfx} not configured.");
            }

            return new SfxHandler(null);
        }

        public void StopAllSounds()
        {
            foreach (var audioSource in SfxSources)
            {
                audioSource.Stop();
            }
        }
    }
}