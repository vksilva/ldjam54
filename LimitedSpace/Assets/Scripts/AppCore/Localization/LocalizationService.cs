using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace AppCore.Localization
{
    public class LocalizationService : MonoBehaviour
    {
        [SerializeField] private TextAsset en_us;
        [SerializeField] private TextAsset pt_br;
        
        private Dictionary<string, string> languageDictionary;
        
        public void Init()
        {
            var json = pt_br.text;
            languageDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public string GetTranslatedText(string key)
        {
            key = key.Trim();
            if (languageDictionary.TryGetValue(key, out var value))
            {
                return value;
            }
            Debug.LogWarning($"Key {key} does not exist on languageDictionary");
            return key;
        }

    }
}
