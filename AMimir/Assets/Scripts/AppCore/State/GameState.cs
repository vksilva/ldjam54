using System;

namespace Busta.AppCore.State
{
    [Serializable]
    public class GameState
    {
        public SettingsState settingsState = new ();
        public LevelsState levelsState = new ();
    }
}