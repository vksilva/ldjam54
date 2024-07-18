using Busta.AppCore.Audio;
using Busta.AppCore.BackKey;
using UnityEngine;
using UnityEngine.UI;
using Application = Busta.AppCore.Application;

namespace Busta.Menus
{
    public class CloseGamePopUp : MonoBehaviour, IPopUp
    {
        [SerializeField] private Button confirmCloseGameButton;
        [SerializeField] private Button declineCloseGameButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button closePopUpButton;
        

        private static BackKeyService _backKeyService;
        private static AudioService _audioSource;

        public void Awake()
        {
            _audioSource = Application.Get<AudioService>();
            _backKeyService = Application.Get<BackKeyService>();
            
            declineCloseGameButton.onClick.AddListener(OnClose);
            backgroundButton.onClick.AddListener(OnClose);
            closePopUpButton.onClick.AddListener(OnClose);
            confirmCloseGameButton.onClick.AddListener(OnCloseGame);
        }

        private void OnCloseGame()
        {
            UnityEngine.Application.Quit();
        }

        private void OnClose()
        {
            _audioSource.PlaySfx(AudioSFXEnum.click);
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _backKeyService.PushAction(Hide);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _backKeyService.PopAction();
        }
    }
}
