using System;
using System.Collections.Generic;
using Busta.AppCore.Audio;

namespace Busta.AppCore.Configurations
{
    [Serializable]
    public class AudioConfigurations
    {
        public int sfxSourcesCount = 4;
        
        public List<AudioConfigEntry<AudioMusicEnum>> musicConfigEntries = new ();
        public List<AudioConfigEntry<AudioSFXEnum>> sfxConfigEntries = new ();
    }
}