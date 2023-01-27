using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Missions
{
    public sealed class MissionSabotage : Mission
    {
        public MissionSabotage() : base("Sabotage", true)
        {
            BaseScore = 500;
            TimeScoreMax = 1000;
            MissionMaxTime = 300;
            State.timeLeft = MissionMaxTime;
            _description = "Placeholder sabotage mission description";
        }

        public enum States
        {
            Init,
            ServerCrashed,
            ServerFixed,
            PipesFixed,
            MissionComplete,
            MissionFailed,
        }

        public override void Setup()
        {
            switch (State.stateID)
            {
                case (int) States.Init:
                    InstantiateDialogueTriggerFromPrefab("Missions/Sabotage/Triggers/", "Panel1Sign");
                    break;
                case (int) States.ServerCrashed:
                    InstantiateSceneTriggerFromPrefab("Missions/Sabotage/Triggers/", "InspectServers");
                    break;
                case (int) States.ServerFixed:
                    InstantiateSceneTriggerFromPrefab("Missions/Sabotage/Triggers/", "PipesStart");
                    break;
                case (int) States.PipesFixed:
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "Missions/Sabotage/sabotage_2");
                    break;
                case (int) States.MissionFailed:
                    MissionFailedScript.MissionFailed();
                    GameStateManager.Instance.BaseMission.FinishCurrentMission(false);
                    break;
            }
        }
        
        public override string GetCurrentTask()
        {
            switch (State.stateID)
            {
                case (int) States.Init:
                    return "go to the powerplant";
                case (int) States.ServerCrashed:
                    return "fix the server";
                case (int) States.ServerFixed:
                    return "have a look around \nmaybe you can find \nother issues";
                case (int) States.PipesFixed:
                    return "talk to the mayor";
            }
            return "";
        }
        
        public override void AdvanceState()
        {
            switch (State.stateID)
            {
                case (int) States.Init:
                    InstantiateDialogueTriggerFromPrefab("Missions/Sabotage/Triggers/", "Panel1Sign");
                    break;
                case (int) States.ServerCrashed:
                    InstantiateSceneTriggerFromPrefab("Missions/Sabotage/Triggers/", "InspectServers");
                    break;
                case (int) States.ServerFixed:
                    InstantiateSceneTriggerFromPrefab("Missions/Sabotage/Triggers/", "PipesStart");
                    break;
                case (int) States.MissionComplete:
                    MissionCompleteScript.MissionComplete();
                    GameStateManager.Instance.BaseMission.FinishCurrentMission(true);
                    break;
            }
            GameStateManager.Instance.UpdateCurrentTask();
        }

        public override void HandleAction(string action)
        {
            if (action == "") return;

            switch (action)
            {
                case "Server":
                    State.stateID = (int)States.ServerCrashed;
                    AdvanceState();
                    break;
                case "SabotageMissionFinished":
                    State.stateID = (int)States.MissionComplete;
                    AdvanceState();
                    break;
            }
            GameStateManager.Instance.UpdateCurrentTask();
        }
    }
}