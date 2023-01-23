using System;
using System.Collections.Generic;

namespace Missions
{
    [Serializable]
    public class MissionState
    {
        public string missionName;
        public int stateID;
        public float timeLeft; // for normal mission only
        public List<string> missions; // for base mission only

        public MissionState(string name)
        {
            missionName = name;
            stateID = 0;
        }
    }
}