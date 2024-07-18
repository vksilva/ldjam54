using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Busta.Menus
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text buttonText;
        [SerializeField] private Image checkMarkImage;
        [SerializeField] private Sprite[] buttonImage;
        [SerializeField] private Color[] textColor;

        public void Setup(string text, int difficulty, bool isCompleted, UnityAction action)
        {
            var index = Mathf.Clamp(difficulty, 1, textColor.Length) - 1;
            buttonText.text = text;
            buttonText.color = textColor[index];
            checkMarkImage.gameObject.SetActive(isCompleted);
            button.onClick.AddListener(action);
            button.image.sprite = buttonImage[index];
        }
    }
}