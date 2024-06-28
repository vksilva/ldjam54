using System;
using UnityEngine;

namespace AppCore
{
    public class StateService
    {
        private const string _gameStatePrefsKey = "GAME_STATE_KEY";
        
        public GameState gameState;
        public void Init()
        {
            Load();
        }

        public void Load()
        {
            var json = PlayerPrefs.GetString(_gameStatePrefsKey, null);
            gameState = JsonUtility.FromJson<GameState>(json) ?? new GameState();
            Debug.Log($"Load {json}");
        }

        public void Save()
        {
            var json = JsonUtility.ToJson(gameState);
            PlayerPrefs.SetString(_gameStatePrefsKey, json);
            Debug.Log($"Saved {json}");
        }
    }
}
