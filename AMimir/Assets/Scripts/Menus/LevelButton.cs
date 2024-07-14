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
        [SerializeField] private Sprite[] buttonImage;
        [SerializeField] private Color[] textColor;

        public void Setup(string text, int difficulty, bool isCompleted, UnityAction action)
        {
            buttonText.text = text;
            buttonText.color = textColor[difficulty - 1];
            checkMarkImage.gameObject.SetActive(isCompleted);
            button.onClick.AddListener(action);
            button.image.sprite = buttonImage[difficulty-1];
        }
    
    }
}
