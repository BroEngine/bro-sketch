using UnityEngine;
using UnityEngine.UI;

namespace Bro.Client.UI
{
    public static class UIExtension
    {
        public static float GetScaledScreenWidth(this CanvasScaler canvasScaler)
        {
            var scale = canvasScaler.GetCurrentScale();
            var screenWidth = Screen.width;
            return screenWidth / scale;
        } 
        
        public static float GetScaledScreenHeight(this CanvasScaler canvasScaler)
        {
            var scale = canvasScaler.GetCurrentScale();
            var screenHeight = Screen.height;
            return screenHeight / scale;
        }
        
        public static float GetCurrentScale(this CanvasScaler canvasScaler)
        {
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;
            var scalerReferenceResolution = canvasScaler.referenceResolution;
            var widthScale = screenWidth / scalerReferenceResolution.x;
            var heightScale = screenHeight / scalerReferenceResolution.y;
            var matchWidthOrHeight = canvasScaler.matchWidthOrHeight;
            
            switch (canvasScaler.screenMatchMode)
            {
                case CanvasScaler.ScreenMatchMode.MatchWidthOrHeight:
                    return Mathf.Pow(widthScale, 1f - matchWidthOrHeight) * Mathf.Pow(heightScale, matchWidthOrHeight);
                case CanvasScaler.ScreenMatchMode.Expand:
                    return Mathf.Min(widthScale, heightScale);
                case CanvasScaler.ScreenMatchMode.Shrink:
                    return Mathf.Max(widthScale, heightScale);
                default:
                    return 1.0f;
            }
        }
    }
}