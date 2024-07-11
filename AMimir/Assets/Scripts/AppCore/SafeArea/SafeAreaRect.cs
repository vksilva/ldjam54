using UnityEngine;

namespace AppCore.SafeArea
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaRect : MonoBehaviour
    {
        private RectTransform panel;
        
        private void Awake()
        {
            panel = GetComponent<RectTransform>();
        }

        private void UpdatePanel(Vector2 anchorMin, Vector2 anchorMax)
        {
            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;
        }
    }
}