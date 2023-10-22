using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private int levelCount;
    
    [SerializeField] private Button levelTemplateButton;

    private void Start()
    {
        for (int i = 1; i <= levelCount; i++)
        {
            var levelNumber = i;
            var newLevelButton = Instantiate(levelTemplateButton, levelTemplateButton.transform.parent);
            newLevelButton.onClick.AddListener(()=>LoadLevel(levelNumber));
            var text = newLevelButton.GetComponentInChildren<TMP_Text>();
            text.text = $"Level {levelNumber}";
        }
        levelTemplateButton.gameObject.SetActive(false);
    }

    private void LoadLevel(int levelId)
    {
        SceneManager.LoadScene($"Bed{levelId:D2}Scene");
        Debug.Log($"Load Bed{levelId:D2}Scene level");
    }
}
