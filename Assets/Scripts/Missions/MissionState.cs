using System;
using System.Runtime.CompilerServices;

namespace Missions
{
    [Serializable]
    public class MissionState
    {
        public string missionName;
        public int stateID;
        public float climateScoreSeconds;

        public MissionState(string name, float csSeconds)
        {
            missionName = name;
            stateID = 0;
            climateScoreSeconds = csSeconds;
        }
    }
}