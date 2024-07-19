using Busta.AppCore.Firebase;
using Firebase.Analytics;

namespace Busta.AppCore.Tracking
{
    public class TrackingService
    {
        private FirebaseService firebaseService;

        public TrackingService Init(FirebaseService firebase)
        {
            firebaseService = firebase;
            return this;
        }

        public void TrackApplicationStart()
        {
            firebaseService.LogEvent(TrackingEvents.ApplicationStart);
        }

        public void TrackLevelStarted(string level)
        {
            Parameter[] param = { new(TrackingParameters.Level, level) };
            firebaseService.LogEvent(TrackingEvents.LevelStarted, param);
        }

        public void TrackLevelEnded(string level, int moves, int failedMoves, float timeSeconds, string result)
        {
            Parameter[] param =
            {
                new(TrackingParameters.Level, level),
                new(TrackingParameters.Moves, moves),
                new(TrackingParameters.FailedMoves, failedMoves),
                new(TrackingParameters.TimeSeconds, timeSeconds),
                new(TrackingParameters.Result, result),
            };
            firebaseService.LogEvent(TrackingEvents.LevelEnded, param);
        }

        public void TrackPausedGame(string level)
        {
            Parameter[] param = { new(TrackingParameters.Level, level) };
            firebaseService.LogEvent(TrackingEvents.LevelPaused, param);
        }

        public void TrackRestartedGame(string level, int moves, int failedMoves, float timeSeconds)
        {
            Parameter[] param =
            {
                new(TrackingParameters.Level, level),
                new(TrackingParameters.Moves, moves),
                new(TrackingParameters.FailedMoves, failedMoves),
                new(TrackingParameters.TimeSeconds, timeSeconds)
            };
            firebaseService.LogEvent(TrackingEvents.LevelRestarted, param);
        }

        public void TrackOpenSettings()
        {
            firebaseService.LogEvent(TrackingEvents.SettingsOpened);
        }

        public void TrackOpenCredits()
        {
            firebaseService.LogEvent(TrackingEvents.CreditsOpened);
        }

        public void TrackChangeLanguage(string language)
        {
            Parameter[] param = { new(TrackingParameters.Language, language) };
            firebaseService.LogEvent(TrackingEvents.LanguageChanged, param);
        }
    }
}