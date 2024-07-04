using AppCore;
using AppCore.Audio;
using AppCore.BackKey;
using Menus;
using UnityEngine;
using UnityEngine.UI;
using Application = AppCore.Application;

public class PausePopUpController : MonoBehaviour, IPopUp
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button backToLevelSelectorButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button backgroundButton;
    
    private static AudioService _audioService;
    private static BackKeyService _backKeyService;
    
    void Awake()
    {
        AddListeners();

        GetServices();
    }

    private static void GetServices()
    {
        _audioService = Application.Get<AudioService>();
        _backKeyService = Application.Get<BackKeyService>();
    }

    private void AddListeners()
    {
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        backToLevelSelectorButton.onClick.AddListener(OnBackToLevelSelectorButton);
        closeButton.onClick.AddListener(OnClose);
        backgroundButton.onClick.AddListener(OnClose);
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
        Hide();
    }

    private void OnClose()
    {
        _audioService.PlaySfx(AudioSFXEnum.click);
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _backKeyService.PushAction(Hide);
    }

    public void Hide()
    {
        _backKeyService.PopAction();
        
        gameObject.SetActive(false);
    }
}
