using System.Threading.Tasks;
using Busta.Extensions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Application = Busta.AppCore.Application;

namespace Busta.Tutorial
{
    public class LevelSelectionTutorialController : MonoBehaviour
    {
        [SerializeField] private GameObject tutorialPopUp;
        [SerializeField] private Image highlight;
        [SerializeField] private CanvasGroup dialogueCanvas;
        [SerializeField] private CanvasGroup dialogueBox;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private Image catAvatar;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button highlightButton;

        public const string TUTORIAL_LEVEL = "world_01_level_01";

        public async void Start()
        {
            if (!Application.Initialized)
            {
                return;
            }

            tutorialPopUp.SetActive(false);
            // check state service for tutorial complete
            // if(tutorialExecuted) return;

            await Tutorial();
        }

        public async Task Tutorial()
        {
            // Initial tutorial setup
            tutorialPopUp.SetActive(true);
            dialogueCanvas.gameObject.SetActive(true);
            dialogueCanvas.alpha = 0;
            dialogueText.text = string.Empty;
            highlight.gameObject.SetActive(false);
            highlightButton.gameObject.SetActive(false);

            // Show cat and dialogue box
            await dialogueCanvas.DOFade(1, 0.5f).AsyncWaitForCompletion();
            await ShowText("Welcome to \"A Mimir\", the cat puzzle game!");
            await WaitForTap(backgroundButton);
            await ShowText("My cat friends are sleepy and want to sleep in their bed.");
            await WaitForTap(backgroundButton);
            await dialogueBox.DOFade(0, 0.5f).AsyncWaitForCompletion();

            // Hide cat and show highlight
            catAvatar.gameObject.SetActive(false);
            var tutorialLevelButton = GameObject.Find(TUTORIAL_LEVEL).GetComponent<Button>();
            await ShowHighlight(tutorialLevelButton.image);
            dialogueText.text = string.Empty;
            await dialogueBox.DOFade(1, 0.5f).AsyncWaitForCompletion();
            await ShowText("Click the level button to start a level.");
            await WaitForTap(highlightButton);

            SceneManager.LoadScene(TUTORIAL_LEVEL);
        }

        private async Task WaitForTap(Button target)
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
        
        private async Task ShowHighlight(Graphic target)
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

        private async Task ShowText(string text)
        {
            dialogueText.text = text;
            dialogueText.ForceMeshUpdate();
            dialogueText.maxVisibleCharacters = 0;

            await DOVirtual.Int(0, dialogueText.text.Length, 1f, value => { dialogueText.maxVisibleCharacters = value; })
                .AsyncWaitForCompletion();
        }
    }
}