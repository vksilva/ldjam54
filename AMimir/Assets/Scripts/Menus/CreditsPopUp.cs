using Busta.AppCore.Audio;
using Busta.AppCore.BackKey;
using Busta.AppCore.Tracking;
using UnityEngine;
using UnityEngine.UI;
using Application = Busta.AppCore.Application;

namespace Busta.Menus
{
    public class CreditsPopUp : MonoBehaviour, IPopUp
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private SettingsPopUp settingsPopUp;

        private AudioService _audioService;
        private BackKeyService _backKeyService;
        private TrackingService _trackingService;

        private void Start()
        {
            closeButton.onClick.AddListener(Hide);
            backgroundButton.onClick.AddListener(Hide);
        }

        private void Awake()
        {
            _audioService = Application.Get<AudioService>();
            _backKeyService = Application.Get<BackKeyService>();
            _trackingService = Application.Get<TrackingService>();
        }

        public void Show()
        {
            settingsPopUp.Hide();
            gameObject.SetActive(true);
            _backKeyService.PushAction(Hide);
            _trackingService.TrackOpenCredits();
        }

        public void Hide()
        {
            _audioService.PlaySfx(AudioSFXEnum.closePopUp);
            _backKeyService.PopAction();
        
            gameObject.SetActive(false);
            
            settingsPopUp.Show();
        }
    }
}
