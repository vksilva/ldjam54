using Menus;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private Button pauseButton;
    [SerializeField] private PausePopUpController pausePopUp;
    [SerializeField] private Button resetButton;
    [SerializeField] private ResetPopUpController resetPopUp;

    private void Start()
    {
        uiCanvas.gameObject.SetActive(true);
        
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        resetButton.onClick.AddListener(OnResetButtonClicked);
    }
    
    private void OnPauseButtonClicked()
    {
        pausePopUp.gameObject.SetActive(true);
    }

    private void OnResetButtonClicked()
    {
        resetPopUp.gameObject.SetActive(true);
    }
}
