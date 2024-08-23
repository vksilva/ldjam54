using System;
using UnityEngine;
using UnityEngine.Events;

namespace Busta.AppCore
{
    public class LifecycleService : MonoBehaviour
    {
        public UnityEvent OnUpdate = new();
        
        private void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}