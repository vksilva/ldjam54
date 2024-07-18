using UnityEngine;

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