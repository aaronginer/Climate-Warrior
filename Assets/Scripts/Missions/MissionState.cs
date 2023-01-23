using System;
using System.IO;

namespace Missions
{
    [Serializable]
    public class MissionState
    {
        public string missionName;
        public int stateID;
        public float timeLeft;

        public MissionState(string name)
        {
            missionName = name;
            stateID = 0;
        }
    }
}