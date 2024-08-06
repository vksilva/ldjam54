using Busta.AppCore.Audio;
using Busta.Commands;
using UnityEngine;
using UnityEngine.UI;
using Application = Busta.AppCore.Application;

namespace Busta.Menus
{
    public class EndGameController : MonoBehaviour
    {
        [SerializeField] private Button backToLevelSelectorButton;

        private void Start()
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