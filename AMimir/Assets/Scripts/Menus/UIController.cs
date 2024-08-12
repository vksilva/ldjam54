using System;
using System.Globalization;
using Busta.AppCore.Audio;
using Busta.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Application = Busta.AppCore.Application;

namespace Busta.Menus
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private Button pauseButton;
        [SerializeField] private PausePopUpController pausePopUp;
        [SerializeField] private Button hintButton;
        [SerializeField] private Image hintButtonCooldownImage;

        private static AudioService _audioService;
        
        private static readonly float hintCooldown = 60f;
        private float currentHintCooldown = 0;

        private void Start()
        {
            uiCanvas.gameObject.SetActive(true);
            GetServices();
            AddListeners();
        }

        private void Update()
        {
            if (currentHintCooldown > 0)
            {
                currentHintCooldown -= Time.deltaTime;
                currentHintCooldown = Mathf.Max(0, currentHintCooldown);
                hintButtonCooldownImage.fillAmount = currentHintCooldown / hintCooldown;
            }
        }

        private static void GetServices()
        {
            _audioService = Application.Get<AudioService>();
        }

        private void AddListeners()
        {
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
            hintButton.onClick.AddListener(OnHintButtonClicked);
        }

        private void OnHintButtonClicked()
        {
            if (currentHintCooldown > 0)
            {
                Debug.Log($"Sem hint irmão.");
                Debug.Log($"Tempo para proxima hint: {currentHintCooldown}");
                return;
            }
            GameController.Instance.ShowNewHint();
            currentHintCooldown = hintCooldown;
            hintButtonCooldownImage.fillAmount = 1f;
        }

        private void OnPauseButtonClicked()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            pausePopUp.Show();
        }
    }
}