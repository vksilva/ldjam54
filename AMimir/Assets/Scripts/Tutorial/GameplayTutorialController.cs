using System.Threading.Tasks;
using Busta.AppCore.State;
using Busta.Extensions;
using Busta.Gameplay;
using DG.Tweening;
using UnityEngine;
using Application = Busta.AppCore.Application;

namespace Busta.Tutorial
{
    public class GameplayTutorialController : BaseTutorialController
    {
        [SerializeField] private GameObject tutorialPaw;
        
        [SerializeField] private PieceMovement cat1;
        [SerializeField] private PieceMovement cat2;
        [SerializeField] private PieceMovement cat3;
        [SerializeField] private PieceMovement cat4;
        
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
            stateService = Application.Get<StateService>();
            if (stateService.gameState.settingsState.seenTutorial)
            {
                SetUpTutorial();
                return;
            }
            
            FindObjectOfType<GameController>().SetBeforeEndgameAction(BeforeEndgame);
            
            await Tutorial();
        }
        
        public async Task Tutorial()
        {
            // Initial tutorial setup
            SetUpTutorial();
            tutorialPopUp.SetActive(true);
            cat2.SetObstacle(true);
            cat3.SetObstacle(true);
            cat4.SetObstacle(true);

            // Show cat and dialogue box
            await dialogueCanvas.DOFade(1, 0.5f).AsyncWaitForCompletion();
            await ShowText("Let's put the cats on the bed.");
            await WaitForTap(backgroundButton);
            await ShowText("I'll move the first one for you.");
            await WaitForTap(backgroundButton);
            await dialogueCanvas.DOFade(0, 0.5f).AsyncWaitForCompletion();
            
            // Show highlight behind cat
            catFrom1.SetActive(true);
            catTo1.SetActive(true);
            await Tasks.WaitForSeconds(0.5f);
            
            // Show paw and drag cat
            tutorialPaw.SetActive(true);
            var initialPosition = cat1.transform.position + new Vector3(1, 3);
            var grabPosition = initialPosition + new Vector3(0, -1);
            var finalPosition = catTo1.transform.position + new Vector3(1, 2);
            var leavePosition = finalPosition + new Vector3(1, 1);
            tutorialPaw.transform.position = initialPosition;
            
            // move pat down to grab the cat
            await tutorialPaw.transform.DOMove(grabPosition, 1).AsyncWaitForCompletion();
            await Tasks.WaitForSeconds(0.3f);
            
            // take the cat to the final position
            tutorialPaw.transform.DOMove(finalPosition, 1);
            await cat1.transform.DOMove(catTo1.transform.position, 1).AsyncWaitForCompletion();
            
            // leave away
            await tutorialPaw.transform.DOMove(leavePosition, 1).AsyncWaitForCompletion();
            tutorialPaw.SetActive(false);
            catFrom1.SetActive(false);
            catTo1.SetActive(false);
            cat1.SetObstacle(true);
            
            // Ask player to move more pieces
            ClearText();
            await dialogueCanvas.DOFade(1, 0.5f).AsyncWaitForCompletion();
            await ShowText("Now it's your turn. Move the cats to the highlighted area.");
            await WaitForTap(backgroundButton);
            await dialogueCanvas.DOFade(0, 0.5f).AsyncWaitForCompletion();
            tutorialPopUp.SetActive(false);
            
            // Wait for piece 2
            catFrom2.SetActive(true);
            catTo2.SetActive(true);
            cat2.SetObstacle(false);
            tutorialPopUp.SetActive(false);
            await WaitForCatLockedInPosition(cat2, catTo2);
            cat2.SetObstacle(true);
            catFrom2.SetActive(false);
            catTo2.SetActive(false);
            
            // Wait for piece 3
            catFrom3.SetActive(true);
            catTo3.SetActive(true);
            cat3.SetObstacle(false);
            await WaitForCatLockedInPosition(cat3, catTo3);
            cat3.SetObstacle(true);
            catFrom3.SetActive(false);
            catTo3.SetActive(false);
            
            // Wait for piece 4
            catFrom4.SetActive(true);
            catTo4.SetActive(true);
            cat4.SetObstacle(false);
            await WaitForCatLockedInPosition(cat4, catTo4);
            cat4.SetObstacle(true);
            catFrom4.SetActive(false);
            catTo4.SetActive(false);
            
            // Set tutorialExecuted
            stateService.gameState.settingsState.seenTutorial = true;
            stateService.Save();
        }

        private async Task BeforeEndgame()
        {
            // Celebrate!
            tutorialPopUp.SetActive(true);
            ClearText();
            await dialogueCanvas.DOFade(1, 0.5f).AsyncWaitForCompletion();
            await ShowText("Awesome! Now all cats are sleeping in the bed!");
            await WaitForTap(backgroundButton);
            await dialogueCanvas.DOFade(0, 0.5f).AsyncWaitForCompletion();
            tutorialPopUp.SetActive(false);
            
            // Set tutorialExecuted
            stateService.gameState.settingsState.seenTutorial = true;
            stateService.Save();
        }

        private static async Task WaitForCatLockedInPosition(PieceMovement cat, GameObject target)
        {
            var catTransform = cat.transform;
            var targetPosition = target.transform.position;
            await Tasks.WaitUntil(() => !cat.IsDragging && catTransform.position == targetPosition);
        }
        
        protected override void SetUpTutorial()
        {
            base.SetUpTutorial();
            
            tutorialPaw.SetActive(false);
            
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