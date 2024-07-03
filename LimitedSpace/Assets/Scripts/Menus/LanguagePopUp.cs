using AppCore;
using AppCore.Audio;
using AppCore.Localization;
using AppCore.State;
using UnityEngine;
using UnityEngine.UI;
using Application = AppCore.Application;

public class LanguagePopUp : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button backgroundButton;
    [SerializeField] private Button pt_brLanguageButton;
    [SerializeField] private Button en_usLanguageButton;
    
    
    
    private StateService _stateService;
    private AudioService _audioService;
    private LocalizationService _localizationService;
    
    private void Start()
    {
        GetServices();
        AddListeners();
    }

    private void GetServices()
    {
        _stateService = Application.Get<StateService>();
        _audioService = Application.Get<AudioService>();
        _localizationService = Application.Get<LocalizationService>();
    }

    private void AddListeners()
    {
        closeButton.onClick.AddListener(OnClose);
        backgroundButton.onClick.AddListener(OnClose);
        pt_brLanguageButton.onClick.AddListener(()=>OnChangeLanguage(LanguagesEnum.pt_br));
        en_usLanguageButton.onClick.AddListener(()=>OnChangeLanguage(LanguagesEnum.en_us));
    }

    private void OnChangeLanguage(LanguagesEnum language)
    {
        _localizationService.SetCurrentLanguage(language);
    }

    private void OnClose()
    {
        _audioService.PlaySfx(AudioSFXEnum.closePopUp);
        
        gameObject.SetActive(false);
    }
}
