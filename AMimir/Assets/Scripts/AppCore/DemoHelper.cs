using UnityEngine;

namespace Busta
{
    public static class DemoHelper
    {
        public static bool IsDemo()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return true;
            }

            return false;
        }
    }
}