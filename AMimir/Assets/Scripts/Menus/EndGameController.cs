using Busta.AppCore.Audio;
using Busta.Commands;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Application = Busta.AppCore.Application;

namespace Busta.Menus
{
    public class EndGameController : MonoBehaviour
    {
        [SerializeField] private Button backToLevelSelectorButton;
        [SerializeField] private Button nextLevelButton;

        private string nextLevel;

        private void Start()
        {
            gameObject.SetActive(true);
            backToLevelSelectorButton.onClick.AddListener(OnBackToLevelSelectorButton);
            nextLevelButton.onClick.AddListener(OnNextLevelButton);

            nextLevel = LevelsList.GetNextLevel(SceneManager.GetActiveScene().name);
            if (nextLevel == null)
            {
                LastLevelEndGame();
            }
        }

        private void OnNextLevelButton()
        {
            Application.Get<AudioService>().PlaySfx(AudioSFXEnum.click);
            if (nextLevel != null)
            {
                SceneManager.LoadScene(nextLevel);
            }
        }

        private void OnBackToLevelSelectorButton()
        {
            Application.Get<AudioService>().PlaySfx(AudioSFXEnum.click);

            var command = new BackToLevelSelectorCommand();
            command.Execute();
        }

        private void LastLevelEndGame()
        {
            nextLevelButton.gameObject.SetActive(false);
        }
    }
}