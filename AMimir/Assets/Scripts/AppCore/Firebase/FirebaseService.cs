using System.Linq;
using System.Threading.Tasks;
using Busta.AppCore.Tracking;
using UnityEngine;
#if !UNITY_WEBGL
using Firebase;
using Firebase.Analytics;
#endif

namespace Busta.AppCore.Firebase
{
    public class FirebaseService
    {
        private bool Initialized { get; set; }
#if !UNITY_WEBGL
        private FirebaseApp firebaseApp;
#endif

        public async Task<FirebaseService> Init()
        {
#if !UNITY_WEBGL
            var result = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (result == DependencyStatus.Available)
            {
                firebaseApp = FirebaseApp.DefaultInstance;
                Initialized = true;
            }
            else
            {
                Initialized = false;
                Debug.Log("Firebase Failed");
            }
#endif
            return this;
        }

        public void LogEvent(string name, params TrackingParameter[] parameters)
        {
#if !UNITY_WEBGL
            if (Initialized)
            {
                FirebaseAnalytics.LogEvent(name, parameters.Select(p => p.Parameter).ToArray());
            }
#endif
        }
    }
}