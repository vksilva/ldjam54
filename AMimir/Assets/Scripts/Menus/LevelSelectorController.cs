using System.Threading.Tasks;
using AppCore;
using AppCore.Audio;
using AppCore.BackKey;
using AppCore.Localization;
using AppCore.State;
using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Application = AppCore.Application;
using Button = UnityEngine.UI.Button;

namespace Menus
{
    public class LevelSelectorController : MonoBehaviour
    {
        // Each entry value is how many levels each world have
        [SerializeField] private WorldData[] worlds;

        [SerializeField] private TMP_Text worldTemplateLabel;
        [SerializeField] private LevelButton levelTemplateButton;
        [SerializeField] private LayoutGroup worldTemplateLayout;

        [SerializeField] private Button settingsButton;
        [SerializeField] private SettingsPopUp settingsPopUp;
        [SerializeField] private CloseGamePopUp closeGamePopUp;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Canvas loadingCanvas;
        
        private static AudioService _audioService;
        private static StateService _stateService;
        private static LocalizationService _localizationService;
        private static BackKeyService _backKeyService;
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
            _audioService.PlayMusic(AudioMusicEnum.menu);

            foreach (var t in worlds)
            {
                CreateWorldSection(t);
            }

            worldTemplateLabel.gameObject.SetActive(false);
            levelTemplateButton.gameObject.SetActive(false);
            settingsPopUp.gameObject.SetActive(false);
            closeGamePopUp.gameObject.SetActive(false);

            _backKeyService.PushAction(CloseLevelSelector);

            ConnectButtons();

            await FocusLastPlayedLevel();

            loadingCanvas.gameObject.SetActive(false);
        }

        private async Task FocusLastPlayedLevel()
        {
            Canvas.ForceUpdateCanvases();
            await Task.Delay(100);
            var button = GameObject.Find(_stateService.gameState.LevelsState.lastPlayedLevel);
            
            if (_stateService.gameState.LevelsState.lastPlayedLevel.IsNullOrEmpty() || !button)
            {
                return;
            }
            Debug.Log($"Focusing on button for level {button.name}");

            var originalMovementType = scrollRect.movementType;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            var buttonTransform = button.GetComponent<RectTransform>();
            var layoutTransform = button.transform.parent.GetComponent<RectTransform>();
            var contentTransform = scrollRect.content.GetComponent<RectTransform>();
            var contentPos = contentTransform.anchoredPosition;
            contentPos.y = -(buttonTransform.anchoredPosition.y + layoutTransform.anchoredPosition.y + scrollOffset);
            contentTransform.anchoredPosition = contentPos;

            Canvas.ForceUpdateCanvases();

            await Task.Delay(1);
            scrollRect.movementType = originalMovementType;
        }

        private static void GetServices()
        {
            _audioService = Application.Get<AudioService>();
            _stateService = Application.Get<StateService>();
            _localizationService = Application.Get<LocalizationService>();
            _backKeyService = Application.Get<BackKeyService>();
        }

        private void CreateWorldSection(WorldData world)
        {
            var parent = worldTemplateLabel.transform.parent;
            var worldLabel = Instantiate(worldTemplateLabel, parent);
            var worldLayout = Instantiate(worldTemplateLayout, parent);
            worldLayout.name = $"world_{world.number:2D}_layout";

            var localizedWorld = _localizationService.GetTranslatedText(world.name);

            worldLabel.text = localizedWorld.ToUpper();
            for (var l = 1; l <= world.levelCount; l++)
            {
                CreateLevelButton(world.number, l, worldLayout.transform);
            }
        }

        private void CreateLevelButton(int world, int level, Transform container)
        {
            var localizedLevel = _localizationService.GetTranslatedText("level");
            var levelName = LevelUtils.GetLevelName(world, level);
            var newLevelButton = Instantiate(levelTemplateButton, container);
            newLevelButton.name = levelName;
            bool isCompleted = _stateService.gameState.LevelsState.winLevels.Contains(levelName);
            newLevelButton.Setup($"{localizedLevel} {level:D2}", world, isCompleted, () => LoadLevel(levelName));
        }

        private static void LoadLevel(string levelName)
        {
            _audioService.PlaySfx(AudioSFXEnum.click);

            var sceneName = levelName;
            SceneManager.LoadScene(sceneName);
        }

        private void ConnectButtons()
        {
            settingsButton.onClick.AddListener(ShowSettings);
        }

        private void ShowSettings()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            settingsPopUp.Show();
        }

        private void CloseLevelSelector()
        {
            closeGamePopUp.Show();
        }
    }
}