using System.Collections.Generic;
using Busta.AppCore.Audio;
using UnityEngine;

namespace Busta.AppCore
{
    [CreateAssetMenu(fileName = "GameConfigs", menuName = "Mimir/GameConfigs")]
    public class GameConfigurations : ScriptableObject
    {
        [Header("Audio")] // Audio Configs
        public int sfxSourcesCount = 4;
        
        public List<AudioConfigEntry<AudioMusicEnum>> musicConfigEntries = new ();
        public List<AudioConfigEntry<AudioSFXEnum>> sfxConfigEntries = new ();
    }
}