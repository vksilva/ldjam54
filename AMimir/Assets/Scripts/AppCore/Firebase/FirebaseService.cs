using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

namespace Busta.AppCore.Firebase
{
    public class FirebaseService
    {
        private bool Initialized { get; set; }
        private FirebaseApp firebaseApp;
        
        public async Task Init()
        {
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
        }

        public void LogEvent(string name, params Parameter[] parameters)
        {
            if (Initialized)
            {
                FirebaseAnalytics.LogEvent(name, parameters);
                Debug.Log($"Firebase: [{name}]");
            }
        }
    }
}
