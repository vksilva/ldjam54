using System;
using AppCore.Localization;

namespace AppCore.State
{
    [Serializable]
    public class SettingsState
    {
        public bool isMusicOff;
        public bool isSFXOff;
        public LanguagesEnum currentLanguage;
    }
}