using System;
using UnityEngine;

namespace AppCore
{
    [Serializable]
    public struct AudioConfigEntry<T>
    {
        public T name;
        public AudioClip audioClip;
    }
}