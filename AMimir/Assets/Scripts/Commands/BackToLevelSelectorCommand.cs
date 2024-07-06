using UnityEngine.SceneManagement;

public class BackToLevelSelectorCommand
{
    public void Execute()
    {
        SceneManager.LoadScene("LevelSelector");
    }
}
