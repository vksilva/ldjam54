using System.Collections.Generic;
using Busta.AppCore.Audio;
using UnityEngine;
using UnityEngine.Serialization;

namespace Busta.AppCore.Configurations
{
    [CreateAssetMenu(fileName = "GameConfigs", menuName = "Mimir/GameConfigs")]
    public class GameConfigurations : ScriptableObject
    {
        public AudioConfigurations AudioConfigurations;
        public LocalizationConfigurations LocalizationConfigurations;
        public SafeAreaConfigurations SafeAreaConfigurations;
    }
}