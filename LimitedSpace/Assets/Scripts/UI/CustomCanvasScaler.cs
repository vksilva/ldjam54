using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public class CustomCanvasScaler : CanvasScaler
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            uiScaleMode = ScaleMode.ScaleWithScreenSize;
            referenceResolution = new Vector2(1080, 1920);
            screenMatchMode = ScreenMatchMode.MatchWidthOrHeight;
            matchWidthOrHeight = 1; // Height
            referencePixelsPerUnit = 100;
        }
    }
}