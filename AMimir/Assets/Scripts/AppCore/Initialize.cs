using AppCore.Audio;
using AppCore.BackKey;
using AppCore.Localization;
using AppCore.SafeArea;
using AppCore.State;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppCore
{
    public class Initialize: MonoBehaviour
    {
        [SerializeField] private GameObject servicesContainer;
        [SerializeField] private AudioService audioService;
        [SerializeField] private LocalizationService localizationService;
        [SerializeField] private BackKeyService backKeyService;
        [SerializeField] private SafeAreaService safeAreaService;
        
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
            
            localizationService.Init(stateService);
            Application.Instance.Add(localizationService);
            
            Application.Instance.Add(backKeyService);
            
            Application.Instance.Add(safeAreaService);
            
            Application.Instance.Init();

            if (!SceneToStart.IsNullOrEmpty())
            {
                SceneManager.LoadScene(SceneToStart);
                return;
            }
            SceneManager.LoadScene(1);
        }
    }
}