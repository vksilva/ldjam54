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
        [SerializeField] private Button levelTemplateButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private SettingsPopUp settingsPopUp;
        
        private static AudioService _audioService;
        private static StateService _stateService;
        
        private void Start()
        {
            for (var index = 0; index < worlds.Length; index++)
            {
                CreateWorldSection(worlds[index]);
            }

            GetServices();
            _audioService.PlayMusic(AudioMusicEnum.menu);
        
            worldTemplateLabel.gameObject.SetActive(false);
            levelTemplateButton.gameObject.SetActive(false);
            settingsPopUp.gameObject.SetActive(false);
            
            ConnectButtons();
        }

        private static void GetServices()
        {
            _audioService = Application.Instance.Get<AudioService>();
            _stateService = Application.Instance.Get<StateService>();
        }

        private void CreateWorldSection(WorldData world)
        {
            var worldLabel = Instantiate(worldTemplateLabel, worldTemplateLabel.transform.parent);
            worldLabel.text = world.name;
            for (var l = 1; l <= world.levelCount; l++)
            {
                CreateLevelButton(world.number, l);
            }
        }

        private void CreateLevelButton(int world, int level)
        {
            var newLevelButton = Instantiate(levelTemplateButton, levelTemplateButton.transform.parent);
            newLevelButton.onClick.AddListener(()=>LoadLevel(world, level));
            var text = newLevelButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Level {level:D2}";
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
