using AppCore;
using UnityEngine;
using UnityEngine.UI;
using Application = AppCore.Application;

public class PausePopUpController : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button backToLevelSelectorButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button backgroundButton;
    
    private static AudioService _audioService;
    
    void Start()
    {
        AddListeners();

        GetServices();
    }

    private static void GetServices()
    {
        _audioService = Application.Get<AudioService>();
    }

    private void AddListeners()
    {
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        backToLevelSelectorButton.onClick.AddListener(OnBackToLevelSelectorButton);
        closeButton.onClick.AddListener(OnClose);
        backgroundButton.onClick.AddListener(OnClose);
    }

    private void OnClose()
    {
        _audioService.PlaySfx(AudioSFXEnum.click);
        
        gameObject.SetActive(false);
    }

    private void OnBackToLevelSelectorButton()
    {
        _audioService.PlaySfx(AudioSFXEnum.click);
        
        var command = new BackToLevelSelectorCommand();
        command.Execute();
    }

    private void OnContinueButtonClicked()
    {
        _audioService.PlaySfx(AudioSFXEnum.click);
        
        gameObject.SetActive(false);
    }
}
