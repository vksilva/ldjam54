using System;
using UnityEngine;

namespace AppCore.Localization
{
    [Serializable]
    public struct LocalizedConfigEntry<T>
    {
        public T name;
        public TextAsset language;
    }
}