using System;
using UnityEngine;

namespace Gameplay
{
    public class LevelUIController : MonoBehaviour
    {
        [SerializeField] private PausePopUpController pausePopUp;
        [SerializeField] private UIController uiController;
        [SerializeField] private EngGameController engGameController;
        public static LevelUIController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    
        private void Start()
        {
            uiController.gameObject.SetActive(true);

            pausePopUp.gameObject.SetActive(false);
            engGameController.gameObject.SetActive(false);
        }

        public void ShowEndGameCanvas()
        {
            engGameController.gameObject.SetActive(true);
        }
    }
}
