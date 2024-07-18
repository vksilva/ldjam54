using UnityEngine.SceneManagement;

namespace Busta.Commands
{
    public class BackToLevelSelectorCommand
    {
        public void Execute()
        {
            SceneManager.LoadScene("LevelSelector");
        }
    }
}
