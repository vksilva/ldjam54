using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Application = Busta.AppCore.Application;

namespace Busta.Tutorial
{
    public class GameplayTutorialController : BaseTutorialController
    {
        [SerializeField] private GameObject cat1;
        [SerializeField] private GameObject cat2;
        [SerializeField] private GameObject cat3;
        [SerializeField] private GameObject cat4;
        
        [SerializeField] private GameObject catFrom1;
        [SerializeField] private GameObject catFrom2;
        [SerializeField] private GameObject catFrom3;
        [SerializeField] private GameObject catFrom4;
        
        [SerializeField] private GameObject catTo1;
        [SerializeField] private GameObject catTo2;
        [SerializeField] private GameObject catTo3;
        [SerializeField] private GameObject catTo4;
        
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
            SetUpTutorial();

            // Show cat and dialogue box
            await dialogueCanvas.DOFade(1, 0.5f).AsyncWaitForCompletion();
            await ShowText("Let's put the cats on the bed.");
            await WaitForTap(backgroundButton);
            await ShowText("I'll move the first one for you.");
            await WaitForTap(backgroundButton);
            await dialogueCanvas.DOFade(0, 0.5f).AsyncWaitForCompletion();
            catFrom1.SetActive(true);
            catTo1.SetActive(true);
            await cat1.transform.DOMove(catTo1.transform.position, 1).AsyncWaitForCompletion();
            catFrom1.SetActive(false);
            catTo1.SetActive(false);
            catFrom2.SetActive(true);
            catTo2.SetActive(true);
            tutorialPopUp.SetActive(false);
            

            // Hide cat and show piece movement
            catAvatar.gameObject.SetActive(false);
            dialogueText.text = string.Empty;
        }

        protected override void SetUpTutorial()
        {
            base.SetUpTutorial();
            
            catFrom1.SetActive(false);
            catFrom2.SetActive(false);
            catFrom3.SetActive(false);
            catFrom4.SetActive(false);
            catTo1.SetActive(false);
            catTo2.SetActive(false);
            catTo3.SetActive(false);
            catTo4.SetActive(false);
        }
    }
}