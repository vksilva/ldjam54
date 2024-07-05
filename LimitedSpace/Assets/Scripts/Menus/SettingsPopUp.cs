using System;
using AppCore;
using AppCore.Audio;
using AppCore.BackKey;
using AppCore.State;
using Unity.VisualScripting;
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
            closeButton.onClick.AddListener(Hide);
            backgroundButton.onClick.AddListener(Hide);
            soundToggle.onValueChanged.AddListener(OnSoundToggled);
            musicToggle.onValueChanged.AddListener(OnMusicToggled);
            selectLanguageButton.onClick.AddListener(OnSelectLanguage);
        }

        private void OnSelectLanguage()
        {
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

        public void Show()
        {
            gameObject.SetActive(true);
            _backKeyService.PushAction(Hide);
        }

        public void Hide()
        {
            _audioService.PlaySfx(AudioSFXEnum.closePopUp);
            _backKeyService.PopAction();
        
            gameObject.SetActive(false);
        }
    }
}
