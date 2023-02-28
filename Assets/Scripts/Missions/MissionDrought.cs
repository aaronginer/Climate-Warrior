namespace Missions
{
    public sealed class Drought : Mission
    {
        public Drought() : base("Drought")
        {
            _description = "Many areas surrounding the city are flooded. Help people in need get to safety.";
        }

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