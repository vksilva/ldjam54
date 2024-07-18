using System;
using System.Collections.Generic;
using UnityEngine;

namespace Busta.AppCore
{
    public class Application
    {
        private static Application _instance;

        public static Application Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Application();
                }

                return _instance;
            }
        }
        
        public static bool Initialized { get; private set; }

        private readonly Dictionary<Type, object> services = new ();

        public void Init()
        {
            Initialized = true;
        }
        
        public void Add<T>(T service)
        {
            services.Add(typeof(T), service);
        }

        public static T Get<T>()
        {
            if (Instance.services.TryGetValue(typeof(T), out var service))
            {
                return (T) service;
            }

            Debug.LogWarning($"Service {typeof(T).Name} not initialized.");
            return default;
        }
    }
}