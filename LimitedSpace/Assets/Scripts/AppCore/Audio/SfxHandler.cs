using UnityEngine;

namespace AppCore
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