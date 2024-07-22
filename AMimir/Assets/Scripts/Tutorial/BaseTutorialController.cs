using System.Threading.Tasks;
using Busta.Extensions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Busta.Tutorial
{
    public class BaseTutorialController : MonoBehaviour
    {
        [SerializeField] protected GameObject tutorialPopUp;
        [SerializeField] protected Image highlight;
        [SerializeField] protected CanvasGroup dialogueCanvas;
        [SerializeField] protected CanvasGroup dialogueBox;
        [SerializeField] protected TMP_Text dialogueText;
        [SerializeField] protected Image catAvatar;
        [SerializeField] protected Button backgroundButton;
        [SerializeField] protected Button highlightButton;
        
        protected async Task ShowText(string text)
        {
            dialogueText.text = text;
            dialogueText.ForceMeshUpdate();
            dialogueText.maxVisibleCharacters = 0;

            await DOVirtual.Int(0, dialogueText.text.Length, 1f, value => { dialogueText.maxVisibleCharacters = value; })
                .AsyncWaitForCompletion();
        }
        
        protected async Task WaitForTap(Button target)
        {
            var buttonClicked = false;

            void onButtonClicked()
            {
                buttonClicked = true;
            }
            
            target.onClick.AddListener(onButtonClicked);
            await Tasks.WaitUntil(() => buttonClicked);
            target.onClick.RemoveListener(onButtonClicked);
        }
        
        protected async Task ShowHighlight(Graphic target)
        {
            highlight.gameObject.SetActive(true);
            highlightButton.gameObject.SetActive(true);
            var highlightTransform = highlight.rectTransform;
            var highlightButtonTr = highlightButton.image.rectTransform;
            var targetTransform = target.rectTransform;
            highlightButtonTr.pivot = highlightTransform.pivot = targetTransform.pivot;
            highlightButtonTr.sizeDelta = highlightTransform.sizeDelta = targetTransform.sizeDelta;
            highlightButtonTr.localScale = highlightTransform.localScale = targetTransform.localScale;
            highlightButtonTr.position = highlightTransform.position = targetTransform.position;
            highlightTransform.localScale = Vector3.zero;
            await highlightTransform.DOScale(targetTransform.localScale, 0.3f).AsyncWaitForCompletion();
        }
        
        protected virtual void SetUpTutorial()
        {
            tutorialPopUp.SetActive(true);
            dialogueCanvas.gameObject.SetActive(true);
            dialogueCanvas.alpha = 0;
            dialogueText.text = string.Empty;
            highlight.gameObject.SetActive(false);
            highlightButton.gameObject.SetActive(false);
        }
    }
}