using AppCore.Audio;
using UnityEngine;
using UnityEngine.UI;
using Application = AppCore.Application;

namespace Menus
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private Button pauseButton;
        [SerializeField] private PausePopUpController pausePopUp;

        private static AudioService _audioService;

        private void Start()
        {
            uiCanvas.gameObject.SetActive(true);
            GetServices();
            AddListeners();
        }

        private static void GetServices()
        {
            _audioService = Application.Get<AudioService>();
        }

        private void AddListeners()
        {
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }

        private void OnPauseButtonClicked()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            pausePopUp.Show();
        }
    }
}
