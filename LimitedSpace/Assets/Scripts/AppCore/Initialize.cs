using AppCore.Localization;
using AppCore.State;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppCore
{
    public class Initialize: MonoBehaviour
    {
        [SerializeField] private GameObject servicesContainer;
        [SerializeField] private AudioService audioService;
        [SerializeField] private LocalizationService localizationService;
        
        private StateService stateService;
        
        public static string SceneToStart = null;
        
        private void Start()
        {
            DontDestroyOnLoad(servicesContainer);
            
            stateService = new StateService();
            stateService.Init();
            Application.Instance.Add(stateService);
            
            audioService.Init(stateService);
            Application.Instance.Add(audioService);
            
            localizationService.Init();
            Application.Instance.Add(localizationService);
            
            Application.Instance.Init();

            if (!string.IsNullOrEmpty(SceneToStart))
            {
                SceneManager.LoadScene(SceneToStart);
                return;
            }
            SceneManager.LoadScene(1);
        }
    }
}