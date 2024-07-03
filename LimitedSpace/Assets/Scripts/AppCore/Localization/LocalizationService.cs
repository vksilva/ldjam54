using System.Collections.Generic;
using System.Linq;
using AppCore.State;
using UnityEngine;
using Newtonsoft.Json;

namespace AppCore.Localization
{
    public class LocalizationService : MonoBehaviour
    {
        [SerializeField] private List<LocalizedConfigEntry<LanguagesEnum>> languageEnumList;

        private Dictionary<string, string> languageDictionary;
        private Dictionary<LanguagesEnum, TextAsset> languageMap = new();

        private StateService _stateService;

        public void Init(StateService state)
        {
            _stateService = state;
            if (_stateService.gameState.settingsState.currentLanguage == LanguagesEnum.none)
            {
                _stateService.gameState.settingsState.currentLanguage = LanguagesEnum.en_us;
            }
            
            languageMap = languageEnumList.ToDictionary(x => x.name, x => x.language);
            var json = languageMap[_stateService.gameState.settingsState.currentLanguage].text;
            languageDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public void SetCurrentLanguage(LanguagesEnum language)
        {
            var json = languageMap[language].text;
            languageDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            UpdateTextLanguage();
            
            _stateService.gameState.settingsState.currentLanguage = language;
            _stateService.Save();
        }

        public string GetTranslatedText(string key)
        {
            key = key.Trim();
            if (languageDictionary.TryGetValue(key, out var value)) return value;
            Debug.LogWarning($"Key {key} does not exist on languageDictionary");
            return key;
        }
        
        private void UpdateTextLanguage()
        {
            var texts = FindObjectsOfType<LocalizedText>();

            foreach (var text in texts)
            {
                text.UpdateText();
            }
        }
    }
}