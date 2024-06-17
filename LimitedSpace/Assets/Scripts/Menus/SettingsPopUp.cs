using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsPopUp : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button backgroundButton;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle musicToggle;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        AddListeners();
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
        Debug.Log($"Music is {isOn}");
    }

    private void OnSoundToggled(bool isOn)
    {
        Debug.Log($"Sound is {isOn}");
    }

    private void OnClose()
    {
        gameObject.SetActive(false);
    }
}
