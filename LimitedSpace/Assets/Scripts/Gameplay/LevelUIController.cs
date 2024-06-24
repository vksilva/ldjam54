using System;
using Menus;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class LevelUIController : MonoBehaviour
    {
        [SerializeField] private PausePopUpController pausePopUp;
        [SerializeField] private UIController uiController;
        [SerializeField] private EndGameController endGameController;
        [SerializeField] private ResetPopUpController resetPopUpController;
        
        public static LevelUIController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    
        private void Start()
        {
            uiController.gameObject.SetActive(true);

            pausePopUp.gameObject.SetActive(false);
            endGameController.gameObject.SetActive(false);
            resetPopUpController.gameObject.SetActive(false);
        }

        public void ShowEndGameCanvas()
        {
            endGameController.gameObject.SetActive(true);
        }
    }
}
