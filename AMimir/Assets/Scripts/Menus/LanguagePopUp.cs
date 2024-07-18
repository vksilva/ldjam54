using Busta.AppCore.Audio;
using Busta.AppCore.BackKey;
using Busta.AppCore.Localization;
using UnityEngine;
using UnityEngine.UI;
using Application = Busta.AppCore.Application;

namespace Busta.Menus
{
    public class LanguagePopUp : MonoBehaviour, IPopUp
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private TextButton languageButtonTemplate;
        [SerializeField] private SettingsPopUp settingsPopUp;
    
        private AudioService audioService;
        private LocalizationService localizationService;
        private BackKeyService backKeyService;
    
        private void Awake()
        {
            GetServices();
            AddListeners();
            CreateLanguageButtons();
        }

        private void GetServices()
        {
            audioService = Application.Get<AudioService>();
            localizationService = Application.Get<LocalizationService>();
            backKeyService = Application.Get<BackKeyService>();
        }

        private void AddListeners()
        {
            closeButton.onClick.AddListener(Hide);
            backgroundButton.onClick.AddListener(Hide);
        }

        private void CreateLanguageButtons()
        {
            var languages = localizationService.GetLanguageInfo();

            foreach (var language in languages)
            {
                var button = Instantiate(languageButtonTemplate, languageButtonTemplate.transform.parent);
                button.Init(language.Name, () => OnChangeLanguage(language.Key));
            }
            
            languageButtonTemplate.gameObject.SetActive(false);
        }
        
        private void OnChangeLanguage(string language)
        {
            localizationService.SetCurrentLanguage(language);
        }

        public void Show()
        {
            settingsPopUp.Hide();
            gameObject.SetActive(true);
            backKeyService.PushAction(Hide);
        }

        public void Hide()
        {
            audioService.PlaySfx(AudioSFXEnum.closePopUp);
            backKeyService.PopAction();
        
            gameObject.SetActive(false);
        
            settingsPopUp.Show();
        }
    }
}
