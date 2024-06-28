using System;

namespace AppCore
{
    [Serializable]
    public class GameState
    {
        public SettingsState settingsState = new ();
        public LevelsState LevelsState = new ();
    }
}