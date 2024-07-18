using System;
using Busta.Menus;
using UnityEngine;

namespace Busta.AppCore.Configurations
{
    [Serializable]
    public class WorldConfigurations
    {
        // Each entry value is how many levels each world have
        public WorldData[] worlds;
    }
}