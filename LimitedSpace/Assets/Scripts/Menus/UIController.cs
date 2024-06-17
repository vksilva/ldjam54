using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private Button pauseButton;
    [SerializeField] private PausePopUpController pausePopUp;
    

    private void Start()
    {
        uiCanvas.gameObject.SetActive(true);
        
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
    }
    
    private void OnPauseButtonClicked()
    {
        pausePopUp.gameObject.SetActive(true);
    }
}
