using AppCore;
using AppCore.Audio;
using AppCore.BackKey;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Application = AppCore.Application;

namespace Menus
{
    public class ResetPopUpController : MonoBehaviour, IPopUp
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button resetButton;

        private static AudioService _audioService;
        private static BackKeyService _backKeyService;
        
        void Awake()
        {
            AddListeners();

            _audioService = Application.Get<AudioService>();
            _backKeyService = Application.Get<BackKeyService>();
        }

        private void AddListeners()
        {
            continueButton.onClick.AddListener(OnContinueButtonClicked);
            resetButton.onClick.AddListener(OnResetButton);
            closeButton.onClick.AddListener(OnClose);
            backgroundButton.onClick.AddListener(OnClose);
        }

        private void OnResetButton()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _backKeyService.PopAction();
        }
    }
}
