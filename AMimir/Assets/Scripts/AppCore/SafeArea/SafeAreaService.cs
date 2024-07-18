using Busta.AppCore.Configurations;
using UnityEngine;
using UnityEngine.Events;

namespace Busta.AppCore.SafeArea
{
    public class SafeAreaService
    {
        private readonly UnityEvent<Rect> updateSafeAreaEvent = new ();
        private Rect SafeAreaRect => configurations.UseDebugSafeArea ? configurations.DebugSafeArea : Screen.safeArea;

        private Rect previousSafeArea;
        private Rect anchor;

        private SafeAreaConfigurations configurations;

        public void Init(SafeAreaConfigurations config)
        {
            this.configurations = config;
        }

        public void RegisterSafeArea(UnityAction<Rect> onUpdate)
        {
            updateSafeAreaEvent.AddListener(onUpdate);
            updateSafeAreaEvent?.Invoke(anchor);
        }

        public void UnregisterSafeArea(UnityAction<Rect> onUpdate)
        {
            updateSafeAreaEvent.RemoveListener(onUpdate);
        }

        public Rect GetSafeArea()
        {
            return anchor;
        }
        
        private void Update()
        {
            UpdateSafeArea();
        }

        private void UpdateSafeArea()
        {
            if (SafeAreaRect == previousSafeArea)
            {
                return;
            }

            previousSafeArea = SafeAreaRect;
            
            anchor.min = SafeAreaRect.position;
            anchor.max = SafeAreaRect.position + SafeAreaRect.size;
            
            // Normalize from 0 to 1
            anchor.xMin /= Screen.width;
            anchor.yMin /= Screen.height;
            anchor.xMax /= Screen.width;
            anchor.yMax /= Screen.height;

            updateSafeAreaEvent?.Invoke(anchor);
        }
    }
}