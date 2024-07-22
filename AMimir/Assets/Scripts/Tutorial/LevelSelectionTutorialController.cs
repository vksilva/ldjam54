using System.Threading.Tasks;
using Busta.AppCore.State;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Application = Busta.AppCore.Application;

namespace Busta.Tutorial
{
    public class LevelSelectionTutorialController : BaseTutorialController
    {
        public const string TUTORIAL_LEVEL = "world_01_level_01";

        public async void Start()
        {
            if (!Application.Initialized)
            {
                return;
            }

            tutorialPopUp.SetActive(false);
            stateService = Application.Get<StateService>();
            if (stateService.gameState.settingsState.seenTutorial)
            {
                SetUpTutorial();
                return;
            }

            await Tutorial();
        }

        public async Task Tutorial()
        {
            // Initial tutorial setup
            SetUpTutorial();
            tutorialPopUp.SetActive(true);

            // Show cat and dialogue box
            await dialogueCanvas.DOFade(1, 0.5f).AsyncWaitForCompletion();
            await ShowText("Welcome to \"A Mimir\", the cat puzzle game!");
            await WaitForTap(backgroundButton);
            await ShowText("My cat friends are sleepy and want to sleep in their bed.");
            await WaitForTap(backgroundButton);
            await dialogueBox.DOFade(0, 0.5f).AsyncWaitForCompletion();

            // Hide cat and show highlight
            catAvatar.gameObject.SetActive(false);
            var tutorialLevelButton = GameObject.Find(TUTORIAL_LEVEL).GetComponent<Button>();
            await ShowHighlight(tutorialLevelButton.image);
            ClearText();
            await dialogueBox.DOFade(1, 0.5f).AsyncWaitForCompletion();
            await ShowText("Click the level button to start a level.");
            await WaitForTap(highlightButton);

            SceneManager.LoadScene(TUTORIAL_LEVEL);
        }
    }
}