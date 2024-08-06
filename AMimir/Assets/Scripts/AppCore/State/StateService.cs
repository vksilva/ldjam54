using UnityEngine;

namespace Busta.AppCore.State
{
    public class StateService
    {
        private const string GameStatePrefsKey = "GAME_STATE_KEY";

        public GameState gameState;

        public StateService Init()
        {
            Load();
            return this;
        }

        private void Load()
        {
            var json = PlayerPrefs.GetString(GameStatePrefsKey, null);
            gameState = JsonUtility.FromJson<GameState>(json) ?? new GameState();
        }

        public void Save()
        {
            var json = JsonUtility.ToJson(gameState);
            PlayerPrefs.SetString(GameStatePrefsKey, json);
        }
    }
}