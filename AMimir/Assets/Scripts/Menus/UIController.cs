using Busta.AppCore.Audio;
using UnityEngine;
using UnityEngine.UI;
using Application = Busta.AppCore.Application;

namespace Busta.Menus
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private Button pauseButton;
        [SerializeField] private PausePopUpController pausePopUp;
        [SerializeField] private Button hintButton;

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
            hintButton.onClick.AddListener(OnHintButtonClicked);
        }

        private void OnHintButtonClicked()
        {
            throw new System.NotImplementedException();
        }

        private void OnPauseButtonClicked()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            pausePopUp.Show();
        }
    }
}
