using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Busta.AppCore.Configurations;
using Busta.AppCore.Localization;
using UnityEngine;
using Application = Busta.AppCore.Application;

namespace Busta.Menus
{
    public abstract class LevelUtils
    {
        public static string GetLevelName(int world, int level)
        {
            return $"world_{world:D2}_level_{level:D2}";
        }

        public static string GetLevelName(string name)
        {
            var nameList = name.Split("_");
            var worldNumber = int.Parse(nameList[1]);
            var levelNumber = int.Parse(nameList[3]);

            var worldConfig = Application.Get<ConfigurationService>().Configs.WorldConfigurations;
            var world = worldConfig.worlds.First(world => world.number == worldNumber).name;

            return $"{Application.Get<LocalizationService>().GetTranslatedText(world)} {levelNumber:D2}";
        }
    }
}