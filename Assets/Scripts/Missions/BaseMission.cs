namespace Missions
{
    public sealed class BaseMission : Mission
    {
        private bool _missionActive;
        public BaseMission() : base("BaseMission")
        {}

        public enum States
        {
            Init,
            SabotageNext,
            SabotageStarted,
            Final,
        }

        public override void Setup()
        {
            switch (State.stateID)
            {
                case (int) States.Init:
                    break;
                case (int) States.SabotageStarted:
                    break;
                case (int) States.Final:
                    break;
            }
        }
        
        public override void AdvanceState()
        {
            switch (State.stateID)
            {
                case (int)States.Init:
                    break;
                case (int)States.Final:
                    break;
            }
        }

        public override void HandleAction(string action)
        {
            if (action == "") return;

            switch (action)
            {
                case "MissionSabotage":
                    GameStateManager.Instance.StartMission(new MissionSabotage());
                    State.stateID = (int) States.SabotageStarted;
                    _missionActive = true;
                    break;
            }
        }
    }
}