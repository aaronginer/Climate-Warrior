namespace Missions
{
    public sealed class MissionSabotage : Mission
    {
        public MissionSabotage() : base("Sabotage",true, 120)
        {}

        public enum States
        {
            Init,
            NotAccepted,
            NotAcceptedNextDialogue,
            NotAcceptedAgain,
            NotAcceptedAgainDialogue,
            Accepted,
            ServerCrashed,
            ServerFixed,
            MissionComplete,
            MissionFailed,
        }

        public override void Setup()
        {
            switch (State.stateID)
            {
                case (int) States.Init:
                    InstantiateDialogueTriggerFromPrefab("Missions/Sabotage/Triggers/", "Sabotage1Dialogue");
                    break;
                case (int) States.NotAcceptedNextDialogue:
                    // rewind to NotAccepted if saved in this state
                    State.stateID = (int)States.NotAccepted;
                    AdvanceState();
                    break;
                case (int) States.NotAcceptedAgainDialogue:
                    // rewind to NotAcceptedAgain if saved in this state
                    State.stateID = (int)States.NotAcceptedAgain;
                    AdvanceState();
                    break;
                case (int) States.Accepted:
                    InstantiateDialogueTriggerFromPrefab("Missions/Sabotage/Triggers/", "Panel1Sign");
                    break;
                case (int) States.ServerCrashed:
                    InstantiateSceneTriggerFromPrefab("Missions/Sabotage/Triggers/", "InspectServers");
                    break;
                case (int) States.ServerFixed:
                    State.stateID = (int) States.MissionComplete;
                    AdvanceState();
                    break;
            }
        }
        
        public override void AdvanceState()
        {
            switch (State.stateID)
            {
                case (int) States.Init:
                    InstantiateDialogueTriggerFromPrefab("Missions/Sabotage/Triggers/", "Sabotage1Dialogue");
                    break;
                case (int) States.NotAccepted:
                    GameStateManager.Instance.SetMissionAdvanceTimer(5);
                    State.stateID = (int) States.NotAcceptedNextDialogue;
                    break;
                case (int) States.NotAcceptedNextDialogue:
                    // dialoguedisplay could be invalid at this point
                    GameStateManager.Instance.dialogueDisplay.StartNewDialogue("Missions/Sabotage/sabotage_2");
                    break;
                case (int) States.NotAcceptedAgain:
                    GameStateManager.Instance.SetMissionAdvanceTimer(5);
                    State.stateID = (int) States.NotAcceptedAgainDialogue;
                    break;
                case (int) States.NotAcceptedAgainDialogue:
                    // dialoguedisplay could be invalid at this point
                    GameStateManager.Instance.dialogueDisplay.StartNewDialogue("Missions/Sabotage/sabotage_3");
                    break;
                case (int) States.Accepted:
                    InstantiateDialogueTriggerFromPrefab("Missions/Sabotage/Triggers/", "Panel1Sign");
                    break;
                case (int) States.ServerCrashed:
                    InstantiateSceneTriggerFromPrefab("Missions/Sabotage/Triggers/", "InspectServers");
                    break;  
                case (int) States.MissionComplete:
                    MissionCompleteScript.MissionComplete();
                    GameStateManager.Instance.BaseMission.FinishMission(true);
                    break;
            }
        }

        public override void HandleAction(string action)
        {
            if (action == "") return;

            switch (action)
            {
                case "Start":
                    State.stateID = (int) States.Accepted;
                    AdvanceState();
                    break;
                case "Delay":
                    // some negative environmental impact
                    State.stateID = (int) States.NotAccepted;
                    AdvanceState();
                    break;
                case "DelayAgain":
                    // some more negative impact
                    State.stateID = (int) States.NotAcceptedAgain;
                    AdvanceState();
                    break;
                case "Server":
                    State.stateID = (int)States.ServerCrashed;
                    AdvanceState();
                    break;
            }
        }
    }
}