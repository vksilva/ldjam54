using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

public class LevelSelectorController : MonoBehaviour
{
    [SerializeField] private int[] levelCount;
    
    [SerializeField] private TMP_Text worldTemplateLabel;
    [SerializeField] private Button levelTemplateButton;

    private void Start()
    {
        for (var w = 1; w <= levelCount.Length; w++)
        {
            var world = w;
            var worldLabel = Instantiate(worldTemplateLabel, worldTemplateLabel.transform.parent);
            worldLabel.text = $"World {world:D2}";
            for (var l = 1; l <= levelCount[world-1]; l++)
            {
                var level = l;
                var newLevelButton = Instantiate(levelTemplateButton, levelTemplateButton.transform.parent);
                newLevelButton.onClick.AddListener(()=>LoadLevel(world, level));
                var text = newLevelButton.GetComponentInChildren<TMP_Text>();
                text.text = $"Level {level:D2}";
            }    
        }
        
        worldTemplateLabel.gameObject.SetActive(false);
        levelTemplateButton.gameObject.SetActive(false);
    }

    private static void LoadLevel(int world, int level)
    {
        var sceneName = $"world_{world:D2}_level_{level:D2}";
        SceneManager.LoadScene(sceneName);
        Debug.Log($"Load level: {sceneName}");
    }
}
