namespace AppCore
{
    public static class TrackingEvents
    {
        public const string ApplicationStart = "app_start";

        public const string LevelStarted = "level_start";
        public const string LevelEnded = "level_ended";
    }

    public static class TrackingParameters
    {
        public static string Level = "level";
    }
}