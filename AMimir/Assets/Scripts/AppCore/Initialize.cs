using Busta.AppCore.Audio;
using Busta.AppCore.BackKey;
using Busta.AppCore.Configurations;
using Busta.AppCore.Firebase;
using Busta.AppCore.Localization;
using Busta.AppCore.Review;
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

            Application.Instance.Add(new ConfigurationService()).Init(gameConfigurations);

            var firebaseService = await Application.Instance.Add(new FirebaseService()).Init();

            var trackingService = Application.Instance.Add(new TrackingService()).Init(firebaseService);

            var stateService = Application.Instance.Add(new StateService()).Init();

            Application.Instance.Add(new ReviewService()).Init();

            Application.Instance.Add(new AudioService())
                .Init(gameConfigurations.AudioConfigurations, stateService, applicationGameObject);

            Application.Instance.Add(new LocalizationService())
                .Init(gameConfigurations.LocalizationConfigurations, stateService);

            Application.Instance.Add(new BackKeyService());

            Application.Instance.Add(new SafeAreaService())
                .Init(gameConfigurations.SafeAreaConfigurations);

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