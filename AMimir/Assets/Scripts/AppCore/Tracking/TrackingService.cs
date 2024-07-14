using AppCore.Firebase;
using Firebase.Analytics;

namespace AppCore
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

        public void TrackLevelEnded(int level, int moves, float timeSeconds, string result)
        {
            Parameter[] param =
            {
                new(TrackingParameters.Level, level),
                new(TrackingParameters.Level, moves),
                new(TrackingParameters.Level, timeSeconds),
                new(TrackingParameters.Level, result),
            };
            firebaseService.LogEvent(TrackingEvents.LevelEnded, param);
        }
    }
}