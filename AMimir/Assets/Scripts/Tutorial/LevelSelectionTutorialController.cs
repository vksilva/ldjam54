using System.Threading.Tasks;
using Busta.AppCore.Localization;
using Busta.AppCore.State;
using Busta.AppCore.Tracking;
using Busta.Extensions;
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

            localizationService = Application.Get<LocalizationService>();
            trackingService = Application.Get<TrackingService>();

            await Tutorial();
        }

        public async Task Tutorial()
        {
            // Initial tutorial setup
            SetUpTutorial();
            tutorialPopUp.SetActive(true);
            trackingService.TrackTutorialStarted();

            // Show cat and dialogue box
            await dialogueCanvas.DOFade(1, 0.5f).AsyncWaitForCompletion();
            await ShowText(localizationService.GetTranslatedText("tutorial_message_01"));
            await WaitForTap(backgroundButton);
            await ShowText(localizationService.GetTranslatedText("tutorial_message_02"));
            await WaitForTap(backgroundButton);
            await dialogueBox.DOFade(0, 0.5f).AsyncWaitForCompletion();

            // Hide cat and show highlight
            catAvatar.gameObject.SetActive(false);
            var tutorialLevelButton = GameObject.Find(TUTORIAL_LEVEL).GetComponent<Button>();
            var buttonTransform = tutorialLevelButton.GetComponent<RectTransform>();
            await ShowHighlight(tutorialLevelButton.image);
            tutorialPawContainer.gameObject.SetActive(true);
            await Tasks.WaitForSeconds(0.3f);
            ClearText();
            await dialogueBox.DOFade(1, 0.5f).AsyncWaitForCompletion();
            await ShowText(localizationService.GetTranslatedText("tutorial_message_03"));
            await WaitForTap(highlightButton);

            SceneManager.LoadScene(TUTORIAL_LEVEL);
        }
    }
}