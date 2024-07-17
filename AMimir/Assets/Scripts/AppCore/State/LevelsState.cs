using System;
using System.Collections.Generic;

namespace AppCore.State
{
    [Serializable]
    public class LevelsState
    {
        public List<string> winLevels = new ();
        public string lastPlayedLevel;
    }
}