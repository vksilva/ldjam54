using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Application = Busta.AppCore.Application;

namespace Busta.Tutorial
{
    public class LevelSelectionTutorialController : MonoBehaviour
    {
        [SerializeField] private GameObject tutorialPopUp;
        [SerializeField] private CanvasGroup dialogueCanvas;
        [SerializeField] private CanvasGroup highlightCanvas;
        
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
            tutorialPopUp.SetActive(true);
            dialogueCanvas.gameObject.SetActive(true);
            dialogueCanvas.alpha = 0;

            await dialogueCanvas.DOFade(1, 1).AsyncWaitForCompletion();

            await Task.Delay(1000);
            
            await dialogueCanvas.DOFade(0, 1).AsyncWaitForCompletion();
            
            tutorialPopUp.SetActive(false);
            
            //...
            
            // load tutorial scene
        }
    }
}