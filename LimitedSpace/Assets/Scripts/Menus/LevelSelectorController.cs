using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

namespace Menus
{
    public class LevelSelectorController : MonoBehaviour
    {
        // Each entry value is how many levels each world have
        [SerializeField] private int[] worldLevelCount;
    
        [SerializeField] private TMP_Text worldTemplateLabel;
        [SerializeField] private Button levelTemplateButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private SettingsPopUp settingsPopUp;
        
        private void Start()
        {
            for (var world = 1; world <= worldLevelCount.Length; world++)
            {
                CreateWorldSection(world);
            }
        
            worldTemplateLabel.gameObject.SetActive(false);
            levelTemplateButton.gameObject.SetActive(false);
            settingsPopUp.gameObject.SetActive(false);
            
            ConnectButtons();
        }

        private void CreateWorldSection(int world)
        {
            var worldLabel = Instantiate(worldTemplateLabel, worldTemplateLabel.transform.parent);
            worldLabel.text = $"World {world:D2}";
            for (var l = 1; l <= worldLevelCount[world-1]; l++)
            {
                CreateLevelButton(world, l);
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
            var sceneName = $"world_{world:D2}_level_{level:D2}";
            SceneManager.LoadScene(sceneName);
            Debug.Log($"Load level: {sceneName}");
        }

        private void ConnectButtons()
        {
            settingsButton.onClick.AddListener(ShowSettings);
        }

        private void ShowSettings()
        {
            settingsPopUp.gameObject.SetActive(true);
        }
    }
}
