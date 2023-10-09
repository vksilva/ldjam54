using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private Button _level01;
    [SerializeField] private Button _level02;

    private void Start()
    {
        _level01.onClick.AddListener(()=>LoadLevel(1));
        _level02.onClick.AddListener(()=>LoadLevel(2));
    }

    private void LoadLevel(int levelId)
    {
        SceneManager.LoadScene($"Bed{levelId:D2}Scene");
        Debug.Log($"Load Bed{levelId:D2}Scene level");
    }
}
