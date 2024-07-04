using AppCore;
using AppCore.Audio;
using AppCore.BackKey;
using AppCore.Localization;
using AppCore.State;
using Menus;
using UnityEngine;
using UnityEngine.UI;
using Application = AppCore.Application;

public class LanguagePopUp : MonoBehaviour, IPopUp
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button backgroundButton;
    [SerializeField] private Button pt_brLanguageButton;
    [SerializeField] private Button en_usLanguageButton;
    
    private AudioService _audioService;
    private LocalizationService _localizationService;
    private BackKeyService _backKeyService;
    
    private void Awake()
    {
        GetServices();
        AddListeners();
    }

    private void GetServices()
    {
        _audioService = Application.Get<AudioService>();
        _localizationService = Application.Get<LocalizationService>();
        _backKeyService = Application.Get<BackKeyService>();
    }

    private void AddListeners()
    {
        closeButton.onClick.AddListener(Hide);
        backgroundButton.onClick.AddListener(Hide);
        pt_brLanguageButton.onClick.AddListener(()=>OnChangeLanguage(LanguagesEnum.pt_br));
        en_usLanguageButton.onClick.AddListener(()=>OnChangeLanguage(LanguagesEnum.en_us));
    }

    private void OnChangeLanguage(LanguagesEnum language)
    {
        _localizationService.SetCurrentLanguage(language);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _backKeyService.PushAction(Hide);
    }

    public void Hide()
    {
        _audioService.PlaySfx(AudioSFXEnum.closePopUp);
        _backKeyService.PopAction();
        
        gameObject.SetActive(false);
    }
}
