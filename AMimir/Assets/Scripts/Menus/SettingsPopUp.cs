using AppCore.Audio;
using AppCore.BackKey;
using AppCore.State;
using UnityEngine;
using UnityEngine.UI;
using Application = AppCore.Application;

namespace Menus
{
    public class SettingsPopUp : MonoBehaviour, IPopUp
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Toggle soundToggle;
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Button selectLanguageButton;
        [SerializeField] private LanguagePopUp languagePopUp;
        [SerializeField] private Button creditsButton;
        [SerializeField] private CreditsPopUp creditsPopUp;
        [SerializeField] private Button privacyPolicyButton;
        [SerializeField] private Button termsOfServiceButton;

        private AudioService _audioService;
        private StateService _stateService;
        private BackKeyService _backKeyService;

        private void Awake()
        {
            GetServices();
            SetInitialState();
            AddListeners();
        }

        private void SetInitialState()
        {
            musicToggle.isOn = !_stateService.gameState.settingsState.isMusicOff;
            soundToggle.isOn = !_stateService.gameState.settingsState.isSFXOff;
            
            languagePopUp.gameObject.SetActive(false);
            ConnectButtons();
        }

        private void GetServices()
        {
            _audioService = Application.Get<AudioService>();
            _stateService = Application.Get<StateService>();
            _backKeyService = Application.Get<BackKeyService>();
        }

        private void AddListeners()
        {
            closeButton.onClick.AddListener(OnClose);
            backgroundButton.onClick.AddListener(OnClose);
            soundToggle.onValueChanged.AddListener(OnSoundToggled);
            musicToggle.onValueChanged.AddListener(OnMusicToggled);
            selectLanguageButton.onClick.AddListener(OnSelectLanguage);
            privacyPolicyButton.onClick.AddListener(OnPrivacyPolicy);
            termsOfServiceButton.onClick.AddListener(OnTermsOfService);
        }

        private void OnTermsOfService()
        {
            Debug.Log($"OnTermsOfService");
            UnityEngine.Application.OpenURL("https://busta.dev/legal/terms-and-conditions/games.busta.mimir/");
        }

        private void OnPrivacyPolicy()
        {
            Debug.Log($"OnPrivacyPolicy");
            UnityEngine.Application.OpenURL("https://busta.dev/legal/privacy-policy/games.busta.mimir/");
        }

        private void OnSelectLanguage()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            languagePopUp.Show();
        }

        private void ConnectButtons()
        {
            creditsButton.onClick.AddListener(ShowCredits);
        }

        private void ShowCredits()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            creditsPopUp.Show();
        }

        private void OnMusicToggled(bool isOn)
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            _audioService.SetMusicOff(!isOn);

            _stateService.gameState.settingsState.isMusicOff = !isOn;
            _stateService.Save();
        }

        private void OnSoundToggled(bool isOn)
        {
            _audioService.SetSfxOff(!isOn);
            _audioService.PlaySfx(AudioSFXEnum.click);

            _stateService.gameState.settingsState.isSFXOff = !isOn;
            _stateService.Save();
        }
        
        private void OnClose()
        {
            _audioService.PlaySfx(AudioSFXEnum.closePopUp);
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _backKeyService.PushAction(Hide);
        }

        public void Hide()
        {
            _backKeyService.PopAction();
            gameObject.SetActive(false);
        }
    }
}
