using UnityEngine;

namespace Busta.AppCore.State
{
    public class StateService
    {
        private const string _gameStatePrefsKey = "GAME_STATE_KEY";
        
        public GameState gameState;
        public StateService Init()
        {
            Load();
            return this;
        }

        public void Load()
        {
            var json = PlayerPrefs.GetString(_gameStatePrefsKey, null);
            gameState = JsonUtility.FromJson<GameState>(json) ?? new GameState();
        }

        public void Save()
        {
            var json = JsonUtility.ToJson(gameState);
            PlayerPrefs.SetString(_gameStatePrefsKey, json);
        }
    }
}
