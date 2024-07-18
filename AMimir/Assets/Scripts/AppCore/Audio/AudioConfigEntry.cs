using System;
using UnityEngine;

namespace Busta.AppCore.Audio
{
    [Serializable]
    public struct AudioConfigEntry<T>
    {
        public T name;
        public AudioClip audioClip;
    }
}