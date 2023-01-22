using System;
using System.Runtime.CompilerServices;

namespace Missions
{
    [Serializable]
    public class MissionState
    {
        public string missionName;
        public int stateID;

        public MissionState(string name)
        {
            missionName = name;
            stateID = 0;
        }
    }
}