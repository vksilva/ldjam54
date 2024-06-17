using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EngGameController : MonoBehaviour
{
    [SerializeField] private Button backToLevelSelectorButton;
    
    void Start()
    {
        gameObject.SetActive(true);
        backToLevelSelectorButton.onClick.AddListener(OnBackToLevelSelectorButton);
    }

    private void OnBackToLevelSelectorButton()
    {
        var command = new BackToLevelSelectorCommand();
        command.Execute();
    }
}
