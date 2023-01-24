using UnityEngine.SceneManagement;

namespace Missions
{
    public sealed class MissionFlooding : Mission
    {
        public MissionFlooding() : base("Flooding")
        {}

        public enum States
        {
            Init,
        }

        public override void Setup()
        {
            switch (State.stateID)
            {
                case (int) States.Init:
                    break;
            }
        }
        
        public override void AdvanceState()
        {
            switch (State.stateID)
            {
                case (int) States.Init:
                    break;
            }
        }

        public override void HandleAction(string action)
        {
            if (action == "") return;

            switch (action)
            {
                
            }
        }
    }
}