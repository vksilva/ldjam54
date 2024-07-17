using System;

namespace AppCore.State
{
    [Serializable]
    public class GameState
    {
        public SettingsState settingsState = new ();
        public LevelsState LevelsState = new ();
    }
}