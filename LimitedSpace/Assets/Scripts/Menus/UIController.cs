using AppCore;
using Menus;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Application = AppCore.Application;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private Button pauseButton;
    [SerializeField] private PausePopUpController pausePopUp;
    [SerializeField] private Button resetButton;
    [SerializeField] private ResetPopUpController resetPopUp;

    private static AudioService _audioService;

    private void Start()
    {
        GetServices();
        
        uiCanvas.gameObject.SetActive(true);

        AddListeners();
    }

    private static void GetServices()
    {
        _audioService = Application.Instance.Get<AudioService>();
    }

    private void AddListeners()
    {
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        resetButton.onClick.AddListener(OnResetButtonClicked);
    }

    private void OnPauseButtonClicked()
    {
        _audioService.PlaySfx(AudioSFXEnum.click);
        
        pausePopUp.gameObject.SetActive(true);
    }

    private void OnResetButtonClicked()
    {
        _audioService.PlaySfx(AudioSFXEnum.click);
        
        resetPopUp.gameObject.SetActive(true);
    }
}
