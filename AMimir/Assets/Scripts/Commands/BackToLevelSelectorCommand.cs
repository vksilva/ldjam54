using Busta.Menus;
using UnityEngine.SceneManagement;

namespace Busta.Commands
{
    public class BackToLevelSelectorCommand
    {
        public void Execute()
        {
            LevelSelectorController.IsCommingFromLevel = true;
            SceneManager.LoadScene("LevelSelector");
        }
    }
}
