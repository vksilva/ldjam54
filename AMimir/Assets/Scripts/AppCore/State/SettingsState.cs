using System;
using Busta.AppCore.Localization;

namespace Busta.AppCore.State
{
    [Serializable]
    public class SettingsState
    {
        public bool isMusicOff;
        public bool isSFXOff;
        public LanguagesEnum currentLanguage;
    }
}