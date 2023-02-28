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
            _description = "Trouble at the hydropower plant. There seem to be several mysterious issues. Help the electricians fix the problems, otherwise the city will have to reactivate the old coal-fired power plant...";
        }

        public enum States
        {
            Init,
            ServerCrashed,
            ServerFixed,
            PipesFixed,
            TalkedToGuard,
            GotCorrectTape,
            GotIncorrectTape,
            DeliveredCorrectTape,
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
                    InstantiateDialogueTriggerFromPrefab("Missions/Sabotage/Triggers/", "SabotageGuardDialogue");
                    break;
                case (int) States.TalkedToGuard:
                    InstantiateSceneTriggerFromPrefab("Missions/Sabotage/Triggers/", "ViewSecurityFootage");
                    break;
                case (int) States.GotCorrectTape:
                    InstantiateDialogueTriggerFromPrefab("Missions/Sabotage/Triggers/", "PoliceCorrectDiskDialogue");
                    break;
                case (int) States.GotIncorrectTape:
                    InstantiateDialogueTriggerFromPrefab("Missions/Sabotage/Triggers/", "PoliceWrongDiskDialogue");
                    break;
                case (int) States.DeliveredCorrectTape:
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
                    return "Find the hydro-power plant.";
                case (int) States.ServerCrashed:
                    return "Try to restart the server.";
                case (int) States.ServerFixed:
                    return "Have a look around. Maybe you can find other issues.";
                case (int) States.PipesFixed:
                    return "Talk to the security guard.";
                case (int) States.TalkedToGuard:
                    return "Review the security footage.";
                case (int) States.GotCorrectTape:
                    return "Deliver the disk to the police.";
                case (int) States.GotIncorrectTape:
                    return "Deliver the disk to the police.";
                case (int) States.DeliveredCorrectTape:
                    return "Talk to the mayor.";
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
                case (int) States.DeliveredCorrectTape:
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "Missions/Sabotage/sabotage_2");
                    break;
                case (int) States.MissionComplete:
                    MissionCompleteScript.MissionComplete();
                    GameStateManager.Instance.BaseMission.FinishCurrentMission(true);
                    break;
            }
            GameStateManager.Instance.ShowCurrentTask();
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
                case "TalkedToGuard":
                    State.stateID = (int)States.TalkedToGuard;
                    break;
                case "CorrectDisk":
                    GameStateManager.Instance.inventoryDisplay.CleanInventory();
                    State.stateID = (int)States.DeliveredCorrectTape;
                    AdvanceState();
                    break;
                case "WrongDisk":
                    GameStateManager.Instance.inventoryDisplay.CleanInventory();
                    State.stateID = (int)States.TalkedToGuard;
                    AdvanceState();
                    break;
            }
            GameStateManager.Instance.ShowCurrentTask();
        }
    }
}