using System;
using UnityEngine;

namespace AppCore
{
    [Serializable]
    public struct AudioConfigEntry
    {
        public string name;
        public AudioClip audioClip;
    }
}