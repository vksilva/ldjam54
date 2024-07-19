namespace Busta.AppCore.Tracking
{
    public static class TrackingEvents
    {
        public const string ApplicationStart = "app_start";

        public const string LevelStarted = "level_start";
        public const string LevelEnded = "level_ended";
        public const string LevelPaused = "level_paused";
        public const string LevelRestarted = "level_restarted";

        public const string SettingsOpened = "settings_opened";
        public const string CreditsOpened = "credits_opened";
        public const string LanguageChanged = "language_changed";
    }

    public static class TrackingParameters
    {
        public const string Level = "level";
        public const string Moves = "moves";
        public const string FailedMoves = "failed_moves";
        public const string TimeSeconds = "time_s";
        public const string Result = "result";
        public const string Language = "language";
    }
}