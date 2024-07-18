using System;
using System.Collections.Generic;
using UnityEngine;

namespace Busta.AppCore.Localization
{
    [CreateAssetMenu(fileName = "language", menuName = "Mimir/Language Dictionary")]
    public class LocalizationData : ScriptableObject
    {
        [Serializable]
        public class LocalizationEntry
        {
            public string Key;
            public string Value;
        }

        public string Name;
        public List<LocalizationEntry> Translations;
    }
}