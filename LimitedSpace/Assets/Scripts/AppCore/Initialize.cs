using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppCore
{
    public class Initialize: MonoBehaviour
    {
        [SerializeField] private GameObject servicesContainer;
        [SerializeField] private AudioService audioService;
        
        private StateService stateService;
        
        private void Start()
        {
            DontDestroyOnLoad(servicesContainer);
            
            stateService = new StateService();
            stateService.Init();
            Application.Instance.Add(stateService);
            
            audioService.Init(stateService);
            Application.Instance.Add(audioService);

            SceneManager.LoadScene(1);
        }
    }
}