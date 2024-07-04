using AppCore.Audio;
using AppCore.BackKey;
using UnityEngine;
using UnityEngine.UI;
using Application = AppCore.Application;

namespace Menus
{
    public class CreditsPopUp : MonoBehaviour, IPopUp
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundButton;

        private AudioService _audioService;
        private BackKeyService _backKeyService;

        private void Start()
        {
            closeButton.onClick.AddListener(Hide);
            backgroundButton.onClick.AddListener(Hide);
        }

        private void Awake()
        {
            _audioService = Application.Get<AudioService>();
            _backKeyService = Application.Get<BackKeyService>();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _backKeyService.PushAction(Hide);
        }

        public void Hide()
        {
            _audioService.PlaySfx(AudioSFXEnum.closePopUp);
            _backKeyService.PopAction();
        
            gameObject.SetActive(false);
        }
    }
}
