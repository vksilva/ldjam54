using System;

namespace AppCore
{
    [Serializable]
    public class GameState
    {
        public SettingsState settingsState = new ();
    }
}