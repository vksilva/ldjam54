using TMPro;
using UnityEngine;

namespace Busta.Menus
{
    public class WorldInformationContainer : MonoBehaviour
    {
        [SerializeField] private TMP_Text progress;
        [SerializeField] private TMP_Text name;

        public void SetValues(string name, string progress)
        {
            this.name.text = name;
            this.progress.text = progress;
        }
    }
}
