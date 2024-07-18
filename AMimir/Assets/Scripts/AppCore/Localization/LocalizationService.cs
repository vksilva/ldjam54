using System.Collections.Generic;
using System.Linq;
using Busta.AppCore.Configurations;
using Busta.AppCore.State;
using Busta.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Busta.AppCore.Localization
{
    public class LocalizationService
    {
        private Dictionary<string, string> languageDictionary;
        private Dictionary<string, LocalizationData> languageMap = new();

        private StateService _stateService;

        public void Init(LocalizationConfigurations configurations, StateService state)
        {
            languageMap = configurations.Localizations.ToDictionary(l => l.Key, l => l);

            _stateService = state;
            if (_stateService.gameState.settingsState.currentLanguage.IsNullOrEmpty() ||
                !languageMap.ContainsKey(_stateService.gameState.settingsState.currentLanguage))
            {
                _stateService.gameState.settingsState.currentLanguage = GetCurrentSystemLanguage();
            }

            SetCurrentLanguage(_stateService.gameState.settingsState.currentLanguage);
        }

        private string GetCurrentSystemLanguage()
        {
            return UnityEngine.Application.systemLanguage switch
            {
                SystemLanguage.Portuguese => "pt_br",
                SystemLanguage.English => "en_us",
                SystemLanguage.German => "de_de",
                _ => "en_us"
            };
        }

        public List<(string Key, string Name)> GetLanguageInfo()
        {
            return languageMap.Select(language => (language.Key, language.Value.Name)).ToList();
        }

        public void SetCurrentLanguage(string language)
        {
            languageDictionary = languageMap[language].Translations.ToDictionary(t => t.Key, t => t.Value);

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
            SceneManager.LoadScene(1);
        }
    }
}