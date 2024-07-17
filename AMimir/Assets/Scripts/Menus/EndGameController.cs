using AppCore.Audio;
using UnityEngine;
using UnityEngine.UI;
using Application = AppCore.Application;

namespace Menus
{
    public class EndGameController : MonoBehaviour
    {
        [SerializeField] private Button backToLevelSelectorButton;
    
        void Start()
        {
            gameObject.SetActive(true);
            backToLevelSelectorButton.onClick.AddListener(OnBackToLevelSelectorButton);
        }

        private void OnBackToLevelSelectorButton()
        {
            Application.Get<AudioService>().PlaySfx(AudioSFXEnum.closePopUp);
        
            var command = new BackToLevelSelectorCommand();
            command.Execute();
        }
    }
}
