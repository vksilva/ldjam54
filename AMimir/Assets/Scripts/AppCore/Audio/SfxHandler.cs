using UnityEngine;

namespace Busta.AppCore.Audio
{
    public struct SfxHandler
    {
        private AudioSource _audioSource;

        public SfxHandler(AudioSource source = null)
        {
            _audioSource = source;
        }

        public void Stop()
        {
            if (_audioSource)
            {
                _audioSource.Stop();
            }
        }
    }
}