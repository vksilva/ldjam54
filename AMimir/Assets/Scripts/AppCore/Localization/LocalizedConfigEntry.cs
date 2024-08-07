using System;
using UnityEngine;

namespace Busta.AppCore.Localization
{
    [Serializable]
    public struct LocalizedConfigEntry<T>
    {
        public T name;
        public TextAsset language;
    }
}