using System.Collections.Generic;

namespace Busta.Menus
{
    public static class LevelsList
    {
        public static List<string> levels = new();

        public static string GetNextLevel(string currentLevel)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                if (levels[i] != currentLevel)
                {
                    continue;
                }

                return i+1 < levels.Count ? levels[i + 1] : null;
            }
            return null;
        }
    }
}