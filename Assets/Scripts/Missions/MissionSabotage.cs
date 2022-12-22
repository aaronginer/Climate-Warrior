
using System;
using System.Collections.Generic;
using Dialogue;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Missions
{
    public sealed class MissionSabotage : Mission
    {
        public MissionSabotage() : base("Sabotage")
        {
            ManualDialogueTriggers["Village"].Add("Sabotage1Dialogue");
            ManualDialogueTriggers["Village"].Add("Sabotage2Dialogue");
            ManualDialogueTriggers["HydroPlantUpper"].Add("Panel1Sign");
        }

        private enum States
        {
            Init,
            NotAccepted,
            NotAcceptedNextDialogue,
            NotAcceptedAgain,
            NotAcceptedAgainDialogue,
            Accepted,
        }
        /*
         * Mission States
         * 0: Start: Mayor dialogue 1, nothing else
         * 1: Mission not accepted on first try: Mayor dialogue 2, nothing else
         * 2: Mission accepted: Active Panel1
         */

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
                    GameStateManager.Instance.SetMissionAdvanceTimer(1);
                    State.stateID = (int) States.NotAcceptedNextDialogue;
                    break;
                case (int) States.NotAcceptedNextDialogue:
                    // dialoguedisplay could be invalid at this point
                    GameStateManager.Instance.dialogueDisplay.StartNewDialogue("Missions/Sabotage/sabotage_2");
                    break;
                case (int) States.NotAcceptedAgain:
                    GameStateManager.Instance.SetMissionAdvanceTimer(1);
                    State.stateID = (int) States.NotAcceptedAgainDialogue;
                    break;
                case (int) States.NotAcceptedAgainDialogue:
                    // dialoguedisplay could be invalid at this point
                    GameStateManager.Instance.dialogueDisplay.StartNewDialogue("Missions/Sabotage/sabotage_3");
                    break;
                case (int) States.Accepted:
                    InstantiateDialogueTriggerFromPrefab("Missions/Sabotage/Triggers/", "Panel1Sign");
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
            }
        }

        public override void HandleAutomaticDialogueTriggers(GameObject obj, DialogueDisplay display)
        {
            if (!IsAutomaticDialogueTrigger(obj)) return;
            
            switch (obj.name)
            {
                    
            }
        }
        
        public override void HandleManualDialogueTriggers(GameObject obj, DialogueDisplay display)
        {
            if (!IsManualDialogueTrigger(obj)) return;
            
            switch (obj.name)
            {
                case "Panel1Sign":
                {
                    display.StartNewDialogue("Missions/Sabotage/panel_1");
                    Object.Destroy(obj);
                    State.stateID = 3;
                    AdvanceState();
                    break;
                }
                case "Sabotage1Dialogue":
                {
                    display.StartNewDialogue("Missions/Sabotage/sabotage_1");
                    Object.Destroy(obj);
                    break;
                }
                case "Sabotage2Dialogue":
                {
                    display.StartNewDialogue("Missions/Sabotage/sabotage_2");
                    Object.Destroy(obj);
                    State.stateID = 2;
                    SceneManager.LoadScene("HydroPlantUpper");
                    //Setup();
                    break;
                }
            }
        }

        public override void HandleSceneTriggers(GameObject obj)
        {
            switch (obj.name)
            {
                case "HydroPlantLowerFloor":
                    GameStateManager.Instance.gameState.playerData.position = new Vector3(2.01200008f, -0.746999979f, 0);
                    SceneManager.LoadScene("HydroPlantLower", LoadSceneMode.Single);
                    break;
                case "HydroPlantUpperFloor":
                    GameStateManager.Instance.gameState.playerData.position = new Vector3(1.00800002f, -0.425000012f, 0);
                    SceneManager.LoadScene("HydroPlantUpper", LoadSceneMode.Single);
                    break;
                case "HydroPlantEnter":
                    GameStateManager.Instance.gameState.playerData.position = new Vector3(1.35099995f, 0.504000008f, 0);
                    SceneManager.LoadScene("HydroPlantUpper", LoadSceneMode.Single);
                    break;
                case "HydroPlantExit":
                    GameStateManager.Instance.gameState.playerData.position = new Vector3(1.36699998f,1.301f,0);
                    SceneManager.LoadScene("River", LoadSceneMode.Single);
                    break; 
            }
        }
    }
}