using UnityEngine;

namespace Busta
{
    public static class DemoHelper
    {
        public static bool IsDemo()
        {
            Debug.Log($"Application.platform {Application.platform}");

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
#if MIMIR_DEMO
                    return true;
#else
                    return false;
#endif
                case RuntimePlatform.WebGLPlayer:
                    return true;
                default:
                    return false;
            }
        }
    }
}