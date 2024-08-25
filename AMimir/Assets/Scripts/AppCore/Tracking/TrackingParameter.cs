#if !UNITY_WEBGL
using Firebase.Analytics;
#endif

namespace Busta.AppCore.Tracking
{
    public class TrackingParameter
    {
        public TrackingParameter(string name, string value)
        {
#if !UNITY_WEBGL
            Parameter = new Parameter(name, value);
#endif
        }

        public TrackingParameter(string name, long value)
        {
#if !UNITY_WEBGL
            Parameter = new Parameter(name, value);
#endif
        }

        public TrackingParameter(string name, double value)
        {
#if !UNITY_WEBGL
            Parameter = new Parameter(name, value);
#endif
        }

#if !UNITY_WEBGL
        public Parameter Parameter { get; set; }
#endif
    }
}