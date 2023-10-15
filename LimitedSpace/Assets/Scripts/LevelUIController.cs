using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUIController : MonoBehaviour
{
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button backToHomeButton;
    [SerializeField] private Button continueButton;
    
    private void Start()
    {
        pauseCanvas.gameObject.SetActive(false);
        
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        backToHomeButton.onClick.AddListener(OnBackToHomeButtonClicked);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    private void OnPauseButtonClicked()
    {
        pauseCanvas.gameObject.SetActive(true);
    }

    private void OnBackToHomeButtonClicked()
    {
        SceneManager.LoadScene("Home");
    }

    private void OnContinueButtonClicked()
    {
        pauseCanvas.gameObject.SetActive(false);
    }
}
