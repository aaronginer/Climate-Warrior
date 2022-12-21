namespace Missions
{
    public class MissionState
    {
        public string Name;
        public int State;

        public MissionState(string name)
        {
            Name = name;
            State = 0;
        }
    }
}