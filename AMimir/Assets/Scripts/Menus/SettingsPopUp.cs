using Busta.AppCore.Audio;
using Busta.AppCore.BackKey;
using Busta.AppCore.State;
using Busta.AppCore.Tracking;
using UnityEngine;
using UnityEngine.UI;
using Application = Busta.AppCore.Application;

namespace Busta.Menus
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
        private TrackingService _trackingService;

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
            _trackingService = Application.Get<TrackingService>();
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
            UnityEngine.Application.OpenURL("https://busta.dev/legal/terms-and-conditions/games.busta.mimir/");
        }

        private void OnPrivacyPolicy()
        {
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
            _trackingService.TrackOpenSettings();
        }

        public void Hide()
        {
            _backKeyService.PopAction();
            gameObject.SetActive(false);
        }
    }
}
