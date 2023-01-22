using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Sabotage,
            Final,
        }

        public override void Setup()
        {
            switch (State.stateID)
            {
                case (int) States.Init:
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
                    State.stateID = (int) States.Sabotage;
                    _missionActive = true;
                    break;
            }
        }
    }
}