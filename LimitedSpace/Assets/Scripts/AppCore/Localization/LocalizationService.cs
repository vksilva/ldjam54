using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace AppCore.Localization
{
    public class LocalizationService : MonoBehaviour
    {
        [SerializeField] private List<LocalizedConfigEntry<LanguagesEnum>> languageEnumList;

        private Dictionary<string, string> languageDictionary;
        private Dictionary<LanguagesEnum, TextAsset> languageMap = new();

        public void Init()
        {
            languageMap = languageEnumList.ToDictionary(x => x.name, x => x.language);
            var json = languageMap[LanguagesEnum.pt_br].text;
            languageDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public void SetCurrentLanguage(LanguagesEnum language)
        {
            var json = languageMap[language].text;
            languageDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            UpdateTextLanguage();
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