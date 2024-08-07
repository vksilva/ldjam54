using Busta.AppCore.Audio;
using Busta.AppCore.BackKey;
using Busta.Menus;
using UnityEngine;
using Application = Busta.AppCore.Application;

namespace Busta.Gameplay
{
    public class LevelUIController : MonoBehaviour
    {
        [SerializeField] private PausePopUpController pausePopUp;
        [SerializeField] private UIController uiController;
        [SerializeField] private EndGameController endGameController;
        [SerializeField] private ResetPopUpController resetPopUpController;
        
        public static LevelUIController Instance { get; private set; }

        private AudioService _audioService;
        private BackKeyService _backKeyService;

        private void Awake()
        {
            Instance = this;
        }
    
        private void Start()
        {
            GetServices();
            
            uiController.gameObject.SetActive(true);
            pausePopUp.gameObject.SetActive(false);
            endGameController.gameObject.SetActive(false);
            resetPopUpController.gameObject.SetActive(false);

            _audioService.PlayMusic(AudioMusicEnum.gameplay);
            _backKeyService.PushAction(pausePopUp.Show);
        }

        private void GetServices()
        {
            _backKeyService = Application.Get<BackKeyService>();
            _backKeyService.CleanActions();
            
            _audioService = Application.Get<AudioService>();
        }

        public void ShowEndGameCanvas()
        {
            endGameController.gameObject.SetActive(true);
        }
    }
}
