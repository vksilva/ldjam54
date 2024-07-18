namespace Busta.Menus
{
    public abstract class LevelUtils
    {
        public static string GetLevelName(int world, int level)
        {
            return $"world_{world:D2}_level_{level:D2}";
        }
    }
}