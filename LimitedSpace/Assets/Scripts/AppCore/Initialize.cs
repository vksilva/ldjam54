using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppCore
{
    public class Initialize: MonoBehaviour
    {
        [SerializeField] private GameObject servicesContainer;
        [SerializeField] private AudioService audioService;
        
        private void Start()
        {
            DontDestroyOnLoad(servicesContainer);
            
            audioService.Init();
            Application.Instance.Add(audioService);

            SceneManager.LoadScene(1);
        }
    }
}