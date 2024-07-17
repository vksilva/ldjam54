using TMPro;
using UnityEngine;

namespace Menus
{
    public class VersionLabel : MonoBehaviour
    {
        private void Start()
        {
            var label = gameObject.GetComponent<TMP_Text>();
            label.text = Application.version;
        }
    }
}
