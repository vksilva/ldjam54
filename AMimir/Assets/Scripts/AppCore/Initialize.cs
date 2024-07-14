using System.Threading.Tasks;
using AppCore.Audio;
using AppCore.BackKey;
using AppCore.Firebase;
using AppCore.Localization;
using AppCore.SafeArea;
using AppCore.State;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppCore
{
    public class Initialize : MonoBehaviour
    {
        [SerializeField] private GameObject servicesContainer;
        [SerializeField] private AudioService audioService;
        [SerializeField] private LocalizationService localizationService;
        [SerializeField] private BackKeyService backKeyService;
        [SerializeField] private SafeAreaService safeAreaService;

        public static string SceneToStart = null;

        private async void Start()
        {
            DontDestroyOnLoad(servicesContainer);

            var firebaseService = new FirebaseService();
            await firebaseService.Init();
            Application.Instance.Add(firebaseService);

            var trackingService = new TrackingService();
            trackingService.Init(firebaseService);
            Application.Instance.Add(trackingService);

            var stateService = new StateService();
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

            trackingService.TrackApplicationStart();
            SceneManager.LoadScene(1);
        }
    }
}