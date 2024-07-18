using Busta.AppCore.Audio;
using Busta.AppCore.BackKey;
using Busta.AppCore.Configurations;
using Busta.AppCore.Firebase;
using Busta.AppCore.Localization;
using Busta.AppCore.SafeArea;
using Busta.AppCore.State;
using Busta.AppCore.Tracking;
using Busta.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Busta.AppCore
{
    public class Initialize : MonoBehaviour
    {
        [SerializeField] private GameConfigurations gameConfigurations;

        public static string SceneToStart = null;

        private async void Start()
        {
            var applicationGameObject = new GameObject("Application");
            DontDestroyOnLoad(applicationGameObject);

            var firebaseService = new FirebaseService();
            await firebaseService.Init();
            Application.Instance.Add(firebaseService);

            var trackingService = new TrackingService();
            trackingService.Init(firebaseService);
            Application.Instance.Add(trackingService);

            var stateService = new StateService();
            stateService.Init();
            Application.Instance.Add(stateService);

            var audioService = new AudioService();
            audioService.Init(gameConfigurations.AudioConfigurations, stateService, applicationGameObject);
            Application.Instance.Add(audioService);

            var localizationService = new LocalizationService();
            localizationService.Init(gameConfigurations.LocalizationConfigurations, stateService);
            Application.Instance.Add(localizationService);

            Application.Instance.Add(new BackKeyService());

            var safeAreaService = new SafeAreaService();
            safeAreaService.Init(gameConfigurations.SafeAreaConfigurations);
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