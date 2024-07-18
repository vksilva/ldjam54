using Busta.AppCore.Audio;
using Busta.AppCore.BackKey;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Application = Busta.AppCore.Application;

namespace Busta.Menus
{
    public class ResetPopUpController : MonoBehaviour, IPopUp
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private PausePopUpController pausePopUp;

        private static AudioService _audioService;
        private static BackKeyService _backKeyService;
        
        void Awake()
        {
            AddListeners();
            GetServices();
        }

        private static void GetServices()
        {
            _audioService = Application.Get<AudioService>();
            _backKeyService = Application.Get<BackKeyService>();
        }

        private void AddListeners()
        {
            continueButton.onClick.AddListener(OnClose);
            resetButton.onClick.AddListener(OnResetButton);
            closeButton.onClick.AddListener(OnClose);
            backgroundButton.onClick.AddListener(OnClose);
        }

        private void OnResetButton()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        private void OnClose()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            Hide();
        }

        public void Show()
        {
            pausePopUp.Hide();
            gameObject.SetActive(true);
            _backKeyService.PushAction(Hide);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _backKeyService.PopAction();
            pausePopUp.Show();
        }
    }
}
