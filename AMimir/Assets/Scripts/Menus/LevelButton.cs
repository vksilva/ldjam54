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

        public void Setup(string text, Sprite sprite, Color textColor, bool isCompleted, bool interactable,
            UnityAction action)
        {
            buttonText.text = text;
            buttonText.color = textColor;
            checkMarkImage.gameObject.SetActive(isCompleted);
            button.onClick.AddListener(action);
            button.image.sprite = sprite;
            button.interactable = interactable;
        }
    }
}