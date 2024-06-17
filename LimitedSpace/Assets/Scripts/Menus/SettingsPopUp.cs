using UnityEngine;
using UnityEngine.UI;

public class SettingsPopUp : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button backgroundButton;
    
    // Start is called before the first frame update
    void Start()
    {
        AddListeners();
    }

    private void AddListeners()
    {
        closeButton.onClick.AddListener(Close);
        backgroundButton.onClick.AddListener(Close);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
