using TMPro;
using UnityEngine;

namespace AppCore.Localization
{
    public class LocalizedText : MonoBehaviour
    {
        private void Start()
        {
            if (!Application.Initialized)
            {
                return;
            }

            var localizationService = Application.Get<LocalizationService>();

            var textToLocalize = gameObject.GetComponent<TMP_Text>();
            textToLocalize.text = localizationService.GetTranslatedText(textToLocalize.text);
        }
    }
}