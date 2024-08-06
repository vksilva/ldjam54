using TMPro;
using UnityEngine;

namespace Busta.AppCore.Localization
{
    public class LocalizedText : MonoBehaviour
    {
        private LocalizationService localizationService;
        private TMP_Text textToLocalize;
        private string key;

        private void Start()
        {
            if (!Application.Initialized)
            {
                return;
            }

            localizationService = Application.Get<LocalizationService>();
            textToLocalize = gameObject.GetComponent<TMP_Text>();
            key = textToLocalize.text;

            UpdateText();
        }

        public void UpdateText()
        {
            textToLocalize.text = localizationService.GetTranslatedText(key);
        }
    }
}