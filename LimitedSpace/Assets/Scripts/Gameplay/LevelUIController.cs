using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gameplay
{
    public class LevelUIController : MonoBehaviour
    {
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private Canvas pauseCanvas;
        [SerializeField] private Canvas endGameCanvas;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button backToHomeButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button endGameHomeButton;
    
        public static LevelUIController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    
        private void Start()
        {
            pauseCanvas.gameObject.SetActive(false);
            endGameCanvas.gameObject.SetActive(false);
        
            endGameHomeButton.onClick.AddListener(OnBackToHomeButtonClicked);
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
            backToHomeButton.onClick.AddListener(OnBackToHomeButtonClicked);
            continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        public void ShowEndGameCanvas()
        {
            endGameCanvas.gameObject.SetActive(true);
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
}
