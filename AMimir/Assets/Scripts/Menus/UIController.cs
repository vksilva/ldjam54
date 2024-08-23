using Busta.AppCore.Audio;
using Busta.Gameplay;
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
        [SerializeField] private Sprite hintButtonOnSprite;
        [SerializeField] private Sprite hintButtonOffSprite;
        

        private static AudioService _audioService;
        
        private static readonly float hintCooldown = 30f;
        private float currentHintCooldown = 0;

        private void Start()
        {
            uiCanvas.gameObject.SetActive(true);
            hintButtonCooldownImage.fillAmount = 0;
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
                return;
            }

            hintButton.image.sprite = hintButtonOnSprite;
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
                return;
            }
            GameController.Instance.ShowNewHint();
            currentHintCooldown = hintCooldown;
            hintButtonCooldownImage.fillAmount = 1f;
            hintButton.image.sprite = hintButtonOffSprite;
        }

        private void OnPauseButtonClicked()
        {
            _audioService.PlaySfx(AudioSFXEnum.click);
            pausePopUp.Show();
        }
    }
}