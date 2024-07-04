using AppCore;
using AppCore.Audio;
using AppCore.BackKey;
using AppCore.Localization;
using AppCore.State;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] private Button settingsButton;
        [SerializeField] private SettingsPopUp settingsPopUp;
        [SerializeField] private Button creditsButton;
        [SerializeField] private CreditsPopUp creditsPopUp;
        
        private static AudioService _audioService;
        private static StateService _stateService;
        private static LocalizationService _localizationService;
        private static BackKeyService _backKeyService;
        
        private void Start()
        {
            if (!Application.Initialized)
            {
                Initialize.SceneToStart = SceneManager.GetActiveScene().name;
                Debug.LogWarning($"Application not initialized, starting from {Initialize.SceneToStart}");
                SceneManager.LoadScene(0);
                return;
            }
            
            GetServices();
            _audioService.PlayMusic(AudioMusicEnum.menu);
            
            for (var index = 0; index < worlds.Length; index++)
            {
                CreateWorldSection(worlds[index]);
            }
        
            worldTemplateLabel.gameObject.SetActive(false);
            levelTemplateButton.gameObject.SetActive(false);
            settingsPopUp.gameObject.SetActive(false);
            creditsPopUp.gameObject.SetActive(false);
            
            _backKeyService.PushAction(CloseLevelSelector);
            
            ConnectButtons();
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
            var worldLabel = Instantiate(worldTemplateLabel, worldTemplateLabel.transform.parent);

            var localizedWorld = _localizationService.GetTranslatedText(world.name);
            
            worldLabel.text = localizedWorld.ToUpper();
            for (var l = 1; l <= world.levelCount; l++)
            {
                CreateLevelButton(world.number, l);
            }
        }

        private void CreateLevelButton(int world, int level)
        {
            var localizedLevel = _localizationService.GetTranslatedText("level");
            
            var newLevelButton = Instantiate(levelTemplateButton, levelTemplateButton.transform.parent);
            bool isCompleted = _stateService.gameState.LevelsState.winLevels.Contains($"world_{world:D2}_level_{level:D2}");
            newLevelButton.Setup($"{localizedLevel} {level:D2}", isCompleted, ()=>LoadLevel(world, level));
            
        }

        private static void LoadLevel(int world, int level)
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            
            var sceneName = $"world_{world:D2}_level_{level:D2}";
            SceneManager.LoadScene(sceneName);
        }

        private void ConnectButtons()
        {
            settingsButton.onClick.AddListener(ShowSettings);
            creditsButton.onClick.AddListener(ShowCredits);
        }

        private void ShowCredits()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            creditsPopUp.Show();
        }

        private void ShowSettings()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            settingsPopUp.gameObject.SetActive(true);
        }

        private void CloseLevelSelector()
        {
            Debug.Log("Show confirm popup");
            UnityEngine.Application.Quit();
            // To do: Implement confirmation to exit pop up
        }
    }
}
