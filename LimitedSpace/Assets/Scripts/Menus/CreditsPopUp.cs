using AppCore;
using AppCore.Audio;
using UnityEngine;
using UnityEngine.UI;
using Application = AppCore.Application;

namespace Menus
{
    public class CreditsPopUp : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundButton;

        private AudioService _audioService;

        private void Start()
        {
            _audioService = Application.Get<AudioService>();
        
            closeButton.onClick.AddListener(OnClose);
            backgroundButton.onClick.AddListener(OnClose);
        }

        private void OnClose()
        {
            _audioService.PlaySfx(AudioSFXEnum.closePopUp);
        
            gameObject.SetActive(false);
        }
    }
}
