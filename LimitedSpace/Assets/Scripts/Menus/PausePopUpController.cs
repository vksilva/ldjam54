using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePopUpController : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button backToLevelSelectorButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button backgroundButton;
    
    
    
    void Start()
    {
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        backToLevelSelectorButton.onClick.AddListener(OnBackToLevelSelectorButton);
        closeButton.onClick.AddListener(OnClose);
        backgroundButton.onClick.AddListener(OnClose);
    }

    private void OnClose()
    {
        gameObject.SetActive(false);
    }

    private void OnBackToLevelSelectorButton()
    {
        var command = new BackToLevelSelectorCommand();
        command.Execute();
    }

    private void OnContinueButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
