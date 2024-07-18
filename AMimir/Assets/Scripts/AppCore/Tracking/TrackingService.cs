using Busta.AppCore.Firebase;
using Firebase.Analytics;

namespace Busta.AppCore.Tracking
{
    public class TrackingService
    {
        private FirebaseService firebaseService;

        public void Init(FirebaseService firebase)
        {
            firebaseService = firebase;
        }

        public void TrackApplicationStart()
        {
            firebaseService.LogEvent(TrackingEvents.ApplicationStart);
        }

        public void TrackLevelStarted(int level)
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
    }
}