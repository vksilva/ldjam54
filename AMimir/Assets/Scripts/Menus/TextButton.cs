using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Busta.Menus
{
    public class TextButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text text;

        public void Init(string text, UnityAction buttonAction)
        {
            this.text.text = text;
            button.onClick.AddListener(buttonAction);
        }
    }
}