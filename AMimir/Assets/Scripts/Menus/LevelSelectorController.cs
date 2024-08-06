using System.Linq;
using System.Threading.Tasks;
using Busta.AppCore;
using Busta.AppCore.Audio;
using Busta.AppCore.BackKey;
using Busta.AppCore.Configurations;
using Busta.AppCore.Localization;
using Busta.AppCore.Review;
using Busta.AppCore.State;
using Busta.Extensions;
using Busta.Tutorial;
using Google.Play.Review;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Application = Busta.AppCore.Application;
using Button = UnityEngine.UI.Button;

namespace Busta.Menus
{
    public class LevelSelectorController : MonoBehaviour
    {
        [SerializeField] private WorldInformationContainer worldInformationContainer;
        [SerializeField] private LevelButton levelTemplateButton;
        [SerializeField] private LayoutGroup worldTemplateLayout;

        [SerializeField] private Button settingsButton;
        [SerializeField] private SettingsPopUp settingsPopUp;
        [SerializeField] private CloseGamePopUp closeGamePopUp;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Canvas loadingCanvas;

        public static bool IsCommingFromLevel = false;
        
        private static AudioService audioService;
        private static StateService stateService;
        private static LocalizationService localizationService;
        private static BackKeyService backKeyService;
        private static ConfigurationService configurationService;
        private static ReviewService reviewService;
        private readonly int scrollOffset = 300;

        private async void Start()
        {
            if (!Application.Initialized)
            {
                Initialize.SceneToStart = SceneManager.GetActiveScene().name;
                Debug.LogWarning($"Application not initialized, starting from {Initialize.SceneToStart}");
                SceneManager.LoadScene(0);
                return;
            }

            loadingCanvas.gameObject.SetActive(true);
            
            GetServices();
            audioService.PlayMusic(AudioMusicEnum.menu);
            CreateWorlds();
            DisableObjects();
            ConnectButtons();
            backKeyService.PushAction(CloseLevelSelector);
            await FocusLastPlayedLevel();
            
            loadingCanvas.gameObject.SetActive(false);

            if (IsCommingFromLevel)
            {
                await AskReview();
                IsCommingFromLevel = false;
            }
        }

        private async Task AskReview()
        {
            if (stateService.gameState.settingsState.seenReview ||
                stateService.gameState.levelsState.winLevels.Count() < 5)
            {
                return;
            }
            await reviewService.RequestReview();
            stateService.gameState.settingsState.seenReview = true;
            stateService.Save();
        }

        private void CreateWorlds()
        {
            var worlds = configurationService.Configs.WorldConfigurations.worlds;
            foreach (var t in worlds)
            {
                if (t.isEnabled)
                {
                    CreateWorldSection(t);
                }
            }
        }

        private void DisableObjects()
        {
            worldInformationContainer.gameObject.SetActive(false);
            levelTemplateButton.gameObject.SetActive(false);
            settingsPopUp.gameObject.SetActive(false);
            closeGamePopUp.gameObject.SetActive(false);
        }

        private async Task FocusLastPlayedLevel()
        {
            if (!stateService.gameState.settingsState.seenTutorial)
            {
                await FocusPlayedLevel(LevelSelectionTutorialController.TUTORIAL_LEVEL);
                return;
            }
            await FocusPlayedLevel(stateService.gameState.levelsState.lastPlayedLevel);
        }

        private async Task FocusPlayedLevel(string level)
        {
            Canvas.ForceUpdateCanvases();
            await Tasks.WaitForSeconds(0.1f);
            var button = GameObject.Find(level);
            
            if (level.IsNullOrEmpty() || !button)
            {
                return;
            }

            var originalMovementType = scrollRect.movementType;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            var buttonTransform = button.GetComponent<RectTransform>();
            var layoutTransform = button.transform.parent.GetComponent<RectTransform>();
            var contentTransform = scrollRect.content.GetComponent<RectTransform>();
            var contentPos = contentTransform.anchoredPosition;
            contentPos.y = -(buttonTransform.anchoredPosition.y + layoutTransform.anchoredPosition.y + scrollOffset);
            contentTransform.anchoredPosition = contentPos;

            Canvas.ForceUpdateCanvases();

            await Tasks.WaitUntilNextFrame();
            scrollRect.movementType = originalMovementType;
        }

        private static void GetServices()
        {
            audioService = Application.Get<AudioService>();
            stateService = Application.Get<StateService>();
            localizationService = Application.Get<LocalizationService>();
            backKeyService = Application.Get<BackKeyService>();
            configurationService = Application.Get<ConfigurationService>();
            reviewService = Application.Get<ReviewService>();
        }

        private void CreateWorldSection(WorldData world)
        {
            var parent = worldInformationContainer.transform.parent;
            var worldLabel = Instantiate(worldInformationContainer, parent);
            var worldLayout = Instantiate(worldTemplateLayout, parent);
            worldLayout.name = $"world_{world.number:D2}_layout";

            var localizedWorld = localizationService.GetTranslatedText(world.name);

            var completedCount = stateService.gameState.levelsState.winLevels
                .Count(level => level.StartsWith($"world_{world.number:D2}"));
            var worldProgress = $"{completedCount}/{world.levelCount}";
            var isWorldCompleted = completedCount == world.levelCount;
            worldLabel.SetValues(localizedWorld.ToUpper(), worldProgress, isWorldCompleted);
            for (var l = 1; l <= world.levelCount; l++)
            {
                CreateLevelButton(world, l, worldLayout.transform);
            }
        }

        private void CreateLevelButton(WorldData world, int level, Transform container)
        {
            var levelName = LevelUtils.GetLevelName(world.number, level);
            var newLevelButton = Instantiate(levelTemplateButton, container);
            newLevelButton.name = levelName;
            bool isCompleted = stateService.gameState.levelsState.winLevels.Contains(levelName);
            var levelExists = SceneUtility.GetBuildIndexByScenePath(levelName) > 0;
            newLevelButton.Setup(level.ToString(), world.buttonImage, world.textColor, isCompleted, 
                levelExists, () => LoadLevel(levelName));
        }

        private static void LoadLevel(string levelName)
        {
            audioService.PlaySfx(AudioSFXEnum.click);
            SceneManager.LoadScene(levelName);
        }

        private void ConnectButtons()
        {
            settingsButton.onClick.AddListener(ShowSettings);
        }

        private void ShowSettings()
        {
            audioService.PlaySfx(AudioSFXEnum.click);
            settingsPopUp.Show();
        }

        private void CloseLevelSelector()
        {
            closeGamePopUp.Show();
        }
    }
}