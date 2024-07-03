using AppCore;
using AppCore.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Application = AppCore.Application;

namespace Menus
{
    public class ResetPopUpController : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button resetButton;

        private static AudioService _audioService;
        
        void Start()
        {
            AddListeners();

            _audioService = Application.Get<AudioService>();
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
            
            gameObject.SetActive(false);
        }
        
        private void OnClose()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            
            gameObject.SetActive(false);
        }
    }
}
