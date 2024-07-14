using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

namespace AppCore.Firebase
{
    public class FirebaseService
    {
        public bool Initialized { get; private set; }
        private FirebaseApp firebaseApp;
        
        public async Task Init()
        {
            var result = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (result == DependencyStatus.Available)
            {
                firebaseApp = FirebaseApp.DefaultInstance;
                Initialized = true;
                Debug.Log("Firebase Initialized");
            }
            else
            {
                Initialized = false;
                Debug.Log("Firebase Failed");
            }
        }

        public void LogEvent(string name, params Parameter[] parameters)
        {
            FirebaseAnalytics.LogEvent(name, parameters);
        }
    }
}
