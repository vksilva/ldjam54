using System;
using System.Collections.Generic;

namespace AppCore
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

        private readonly Dictionary<Type, object> services = new ();

        public void Add<T>(T service)
        {
            services.Add(typeof(T), service);
        }

        public T Get<T>()
        {
            if (services.TryGetValue(typeof(T), out var service))
            {
                return (T) service;
            }

            throw new Exception($"Service not initialized {nameof(T)}");
        }
    }
}