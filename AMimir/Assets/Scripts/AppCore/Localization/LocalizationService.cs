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

        private StateService stateService;

        public void Init(LocalizationConfigurations configurations, StateService state)
        {
            languageMap = configurations.Localizations.ToDictionary(l => l.Key, l => l);

            stateService = state;
            if (stateService.gameState.settingsState.currentLanguage.IsNullOrEmpty() ||
                !languageMap.ContainsKey(stateService.gameState.settingsState.currentLanguage))
            {
                stateService.gameState.settingsState.currentLanguage = GetCurrentSystemLanguage();
            }

            SetCurrentLanguage(stateService.gameState.settingsState.currentLanguage);
        }

        private string GetCurrentSystemLanguage()
        {
            return UnityEngine.Application.systemLanguage switch
            {
                SystemLanguage.Portuguese => "pt_br",
                SystemLanguage.English => "en_us",
                SystemLanguage.German => "de_de",
                SystemLanguage.Spanish => "es_es",
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

            stateService.gameState.settingsState.currentLanguage = language;
            stateService.Save();
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