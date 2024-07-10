using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Menus
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text buttonText ;
        [SerializeField] private Image checkMarkImage;

        public void Setup(string text, bool isCompleted, UnityAction action)
        {
            buttonText.text = text;
            checkMarkImage.gameObject.SetActive(isCompleted);
            button.onClick.AddListener(action);
        }
    
    }
}
