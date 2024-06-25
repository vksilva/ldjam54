﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AppCore
{
    public class AudioService : MonoBehaviour
    {
        [SerializeField] private int sfxCount = 4;

        [SerializeField] private List<AudioConfigEntry> _musicConfigEntries = new ();
        [SerializeField] private List<AudioConfigEntry> _sfxConfigEntries = new ();

        private Dictionary<string, AudioClip> _musicMap = new();
        private Dictionary<string, AudioClip> _sfxMap = new();
        
        private AudioSource MusicSource;
        private readonly List<AudioSource> SfxSources = new ();

        private string currentMusic = string.Empty;

        public void Init()
        {
            _musicMap = _musicConfigEntries.ToDictionary(x => x.name, x => x.audioClip);
            _sfxMap = _sfxConfigEntries.ToDictionary(x => x.name, x => x.audioClip);
            
            MusicSource = gameObject.AddComponent<AudioSource>();
            MusicSource.playOnAwake = false;
            MusicSource.loop = true;
            
            for (var i = 0; i < sfxCount; i++)
            {
                var sfxSource = gameObject.AddComponent<AudioSource>(); 
                SfxSources.Add(sfxSource);
                sfxSource.playOnAwake = false;
                sfxSource.loop = false;
            }
        }
        
        public void PlayMusic(string music)
        {
            if (music == currentMusic)
            {
                return;
            }

            StopMusic();

            if (_musicMap.TryGetValue(music, out var clip))
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

        public void StopMusic()
        {
            currentMusic = string.Empty;
            MusicSource.Stop();
        }

        public SfxHandler PlaySfx(string sfx)
        {
            foreach (var sfxSource in SfxSources)
            {
                if (sfxSource.isPlaying)
                {
                    continue;
                }
                
                if (_sfxMap.TryGetValue(sfx, out var clip))
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