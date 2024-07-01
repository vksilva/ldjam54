using AppCore;
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
        
        private static AudioService _audioService;
        private static StateService _stateService;
        
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
            
            ConnectButtons();
        }

        private static void GetServices()
        {
            _audioService = Application.Get<AudioService>();
            _stateService = Application.Get<StateService>();
        }

        private void CreateWorldSection(WorldData world)
        {
            var worldLabel = Instantiate(worldTemplateLabel, worldTemplateLabel.transform.parent);
            worldLabel.text = world.name.ToUpper();
            for (var l = 1; l <= world.levelCount; l++)
            {
                CreateLevelButton(world.number, l);
            }
        }

        private void CreateLevelButton(int world, int level)
        {
            var newLevelButton = Instantiate(levelTemplateButton, levelTemplateButton.transform.parent);
            bool isCompleted = _stateService.gameState.LevelsState.winLevels.Contains($"world_{world:D2}_level_{level:D2}");
            newLevelButton.Setup($"Level {level:D2}", isCompleted, ()=>LoadLevel(world, level));
            
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
        }

        private void ShowSettings()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            settingsPopUp.gameObject.SetActive(true);
        }
    }
}
