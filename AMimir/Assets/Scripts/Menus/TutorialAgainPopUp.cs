using Busta.AppCore.Audio;
using Busta.AppCore.BackKey;
using Busta.AppCore.State;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Application = Busta.AppCore.Application;

namespace Busta.Menus
{
    public class TutorialAgainPopUp : MonoBehaviour, IPopUp
    {
        [SerializeField] private Button confirmTutorialAgainButton;
        [SerializeField] private Button declineTutorialAgainButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button closePopUpButton;
        [SerializeField] private SettingsPopUp settingsPopUp;

        private static BackKeyService backKeyService;
        private static AudioService audioSource;
        private static StateService stateService;

        public void Awake()
        {
            audioSource = Application.Get<AudioService>();
            backKeyService = Application.Get<BackKeyService>();
            stateService = Application.Get<StateService>();
            
            declineTutorialAgainButton.onClick.AddListener(OnClose);
            backgroundButton.onClick.AddListener(OnClose);
            closePopUpButton.onClick.AddListener(OnClose);
            confirmTutorialAgainButton.onClick.AddListener(OnTutorialAgain);
        }

        private void OnTutorialAgain()
        {
            stateService.gameState.settingsState.seenTutorial = false;
            stateService.Save();
            SceneManager.LoadScene(1);
        }

        private void OnClose()
        {
            audioSource.PlaySfx(AudioSFXEnum.click);
            Hide();
        }

        public void Show()
        {
            settingsPopUp.Hide();
            gameObject.SetActive(true);
            backKeyService.PushAction(Hide);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            backKeyService.PopAction();
            settingsPopUp.Show();
        }
    }
}
