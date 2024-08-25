using UnityEngine;

namespace Busta.AppCore
{
    public class DemoElement : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(DemoHelper.IsDemo());
        }
    }
}