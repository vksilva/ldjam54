using UnityEngine;

namespace Busta
{
    public class DemoElement : MonoBehaviour
    {
        void Awake()
        {
            if (DemoHelper.IsDemo())
            {
                gameObject.SetActive(false);
            }
        }
    }
}
