using System;
using AppCore.Localization;

namespace AppCore
{
    [Serializable]
    public class SettingsState
    {
        public bool isMusicOff;
        public bool isSFXOff;
        public LanguagesEnum currentLanguage;
    }
}