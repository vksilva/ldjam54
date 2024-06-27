using AppCore;
using UnityEngine;
using UnityEngine.UI;
using Application = AppCore.Application;

public class SettingsPopUp : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button backgroundButton;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle musicToggle;

    private AudioService _audioService;

    private void Start()
    {
        AddListeners();
        _audioService = Application.Instance.Get<AudioService>();
    }

    private void AddListeners()
    {
        closeButton.onClick.AddListener(OnClose);
        backgroundButton.onClick.AddListener(OnClose);
        soundToggle.onValueChanged.AddListener(OnSoundToggled);
        musicToggle.onValueChanged.AddListener(OnMusicToggled);
    }

    private void OnMusicToggled(bool isOn)
    {
        _audioService.PlaySfx("click");
        
        _audioService.SetMusicOff(!isOn);
        Debug.Log($"Music is {isOn}");
    }

    private void OnSoundToggled(bool isOn)
    {
        _audioService.PlaySfx("click");
        
        _audioService.SetSfxOff(!isOn);
        Debug.Log($"Sound is {isOn}");
    }

    private void OnClose()
    {
        _audioService.PlaySfx("closePopUp");
        
        gameObject.SetActive(false);
    }
}
