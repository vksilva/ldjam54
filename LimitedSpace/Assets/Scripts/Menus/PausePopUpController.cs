using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePopUpController : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button backToLevelSelectorButton;
    
    void Start()
    {
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        backToLevelSelectorButton.onClick.AddListener(OnBackToLevelSelectorButton);
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
