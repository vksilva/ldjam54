using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menus
{
    public class ResetPopUpController : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button resetButton;
        
        void Start()
        {
            continueButton.onClick.AddListener(OnContinueButtonClicked);
            resetButton.onClick.AddListener(OnResetButton);
            closeButton.onClick.AddListener(OnClose);
            backgroundButton.onClick.AddListener(OnClose);
        }

        private void OnResetButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        private void OnContinueButtonClicked()
        {
            gameObject.SetActive(false);
        }
        
        private void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}
