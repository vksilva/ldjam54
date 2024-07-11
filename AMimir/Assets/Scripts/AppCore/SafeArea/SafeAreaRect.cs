using UnityEngine;

namespace AppCore.SafeArea
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaRect : MonoBehaviour
    {
        private RectTransform panel;
        private SafeAreaService safeAreaService;
        
        private void Awake()
        {
            panel = GetComponent<RectTransform>();

            if (!Application.Initialized)
            {
                return;
            }

            safeAreaService = Application.Get<SafeAreaService>();
            safeAreaService.RegisterSafeArea(UpdatePanel);
            UpdatePanel(safeAreaService.GetSafeArea());
        }

        private void OnDestroy()
        {
            Application.Get<SafeAreaService>()?.UnregisterSafeArea(UpdatePanel);
        }

        private void UpdatePanel(Rect anchor)
        {
            panel.anchorMin = anchor.min;
            panel.anchorMax = anchor.max;
        }
    }
}