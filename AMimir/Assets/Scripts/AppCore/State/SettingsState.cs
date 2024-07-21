using System;

namespace Busta.AppCore.State
{
    [Serializable]
    public class SettingsState
    {
        public bool isMusicOff;
        public bool isSFXOff;
        public string currentLanguage;
        public bool seenTutorial;
    }
}