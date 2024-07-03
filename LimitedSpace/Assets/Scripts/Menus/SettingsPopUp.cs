using AppCore;
using AppCore.Audio;
using AppCore.State;
using UnityEngine;
using UnityEngine.UI;
using Application = AppCore.Application;

namespace Menus
{
    public class SettingsPopUp : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Toggle soundToggle;
        [SerializeField] private Toggle musicToggle;

        private AudioService _audioService;
        private StateService _stateService;

        private void Start()
        {
            GetServices();
            SetInitialState();

            AddListeners();
        }

        private void SetInitialState()
        {
            musicToggle.isOn = !_stateService.gameState.settingsState.isMusicOff;
            soundToggle.isOn = !_stateService.gameState.settingsState.isSFXOff;
        }

        private void GetServices()
        {
            _audioService = Application.Get<AudioService>();
            _stateService = Application.Get<StateService>();
        }

        private void AddListeners()
        {
            closeButton.onClick.AddListener(OnClose);
            backgroundButton.onClick.AddListener(OnClose);
            soundToggle.onValueChanged.AddListener(OnSoundToggled);
            musicToggle.onValueChanged.AddListener(OnMusicToggled);
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
        
            gameObject.SetActive(false);
        }
    }
}
