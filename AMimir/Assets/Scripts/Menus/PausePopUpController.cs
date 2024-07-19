using Busta.AppCore.Audio;
using Busta.AppCore.BackKey;
using Busta.AppCore.Tracking;
using Busta.Commands;
using Busta.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Application = Busta.AppCore.Application;

namespace Busta.Menus
{
    public class PausePopUpController : MonoBehaviour, IPopUp
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button backToLevelSelectorButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private ResetPopUpController resetPopUp;
    
        private static AudioService _audioService;
        private static BackKeyService _backKeyService;
        private static TrackingService _trackingService;
    
        void Awake()
        {
            AddListeners();
            GetServices();
        }

        private static void GetServices()
        {
            _audioService = Application.Get<AudioService>();
            _backKeyService = Application.Get<BackKeyService>();
            _trackingService = Application.Get<TrackingService>();
        }

        private void AddListeners()
        {
            continueButton.onClick.AddListener(OnContinueButtonClicked);
            backToLevelSelectorButton.onClick.AddListener(OnBackToLevelSelectorButton);
            closeButton.onClick.AddListener(OnClose);
            backgroundButton.onClick.AddListener(OnClose);
            resetButton.onClick.AddListener(OnShowResetPopUp);
        }

        private void OnShowResetPopUp()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            resetPopUp.Show();
        }

        private void OnBackToLevelSelectorButton()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
        
            var command = new BackToLevelSelectorCommand();
            command.Execute();
            GameController.Instance.TrackAbandonLevel();
        }

        private void OnContinueButtonClicked()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            Hide();
        }

        private void OnClose()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _backKeyService.PushAction(Hide);
            _trackingService.TrackPausedGame(SceneManager.GetActiveScene().name);
        }

        public void Hide()
        {
            _backKeyService.PopAction();
            gameObject.SetActive(false);
        }
    }
}
