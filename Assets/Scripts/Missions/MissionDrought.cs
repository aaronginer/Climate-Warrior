using UnityEngine.SceneManagement;

namespace Missions
{
    public sealed class Drought : Mission
    {
        public Drought() : base("Flooding")
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