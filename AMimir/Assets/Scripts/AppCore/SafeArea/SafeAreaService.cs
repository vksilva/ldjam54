using UnityEngine;
using UnityEngine.Events;

namespace AppCore.SafeArea
{
    public class SafeAreaService : MonoBehaviour
    {
        [SerializeField] private bool useDebugSafeArea;
        [SerializeField] private Rect debugSafeArea;

        private readonly UnityEvent<Vector2, Vector2> updateSafeAreaEvent = new ();
        private Rect SafeAreaRect => useDebugSafeArea ? debugSafeArea : Screen.safeArea;

        private Vector2 anchorMin;
        private Vector2 anchorMax;
        private Rect previousSafeArea;

        public void RegisterSafeArea(UnityAction<Vector2, Vector2> onUpdate)
        {
            updateSafeAreaEvent.AddListener(onUpdate);
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
            
            anchorMin = SafeAreaRect.position;
            anchorMax = SafeAreaRect.position + SafeAreaRect.size;
            
            // Normalize from 0 to 1
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
        }
    }
}