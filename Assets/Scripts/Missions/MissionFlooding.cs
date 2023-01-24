using Missions.Flooding;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Missions
{
    public sealed class MissionFlooding : Mission
    {
        private FloodingGameScript _gameScript;
        
        public MissionFlooding() : base("Flooding", -250, 250)
        {}

        public enum States
        {
            Init,
            InGame,
            GrandmaSaved,
            MissionComplete,
            MissionFailed
        }

        public override void Setup()
        {
            switch (State.stateID)
            {
                case (int) States.Init:
                    InstantiateSceneTriggerFromPrefab("Missions/Flooding/Triggers/", "FloodingStart");
                    break;
                case (int) States.MissionComplete:
                    MissionCompleteScript.MissionComplete();
                    GameStateManager.Instance.BaseMission.FinishMission(true);
                    break;
                case (int) States.MissionFailed:
                    break;
            }
        }
        
        public override void AdvanceState()
        {
            switch (State.stateID)
            {
                case (int) States.Init:
                    InstantiateSceneTriggerFromPrefab("Missions/Flooding/Triggers/", "FloodingStart");
                    break;
                case (int) States.GrandmaSaved:
                    InstantiateDialogueTriggerFromPrefab("Missions/Flooding/Triggers/", "GrandmaDialogue2");
                    break;
            }
        }

        public override void HandleAction(string action)
        {
            if (action == "") return;

            switch (action)
            {
                case "Karma50":
                    DeductionsDecisions -= 25;
                    break;
                case "GrandmaDialogueFinished":
                    GameObject.Find("Controller").GetComponent<FloodingGameScript>().Phase2();
                    break;
                case "GrandmaRescued":
                    State.stateID = (int)States.MissionComplete;
                    SceneManager.LoadScene("Village");
                    break;
            }
        }
    }
}