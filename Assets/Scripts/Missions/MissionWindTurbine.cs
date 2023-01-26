using System;
using Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Missions
{
    public sealed class MissionWindTurbine : Mission
    {
        public MissionWindTurbine() : base("MissionWindTurbine", true)
        {
            BaseScore = 500;
            TimeScoreMax = 1000;
            MissionMaxTime = 420;
            State.timeLeft = MissionMaxTime;
        }

        public enum States
        {
            SearchingForJumpAndRun,
            AfterJumpAndRunCompletedGoToMajor,
            SearchingForAdditionalParts,
            SearchingForWindTurbineBuild,
            MissionComplete,
            MissionFailed,
        }

        private bool _didSpawnParts = false;

        public override string GetCurrentTask()
        {
            switch (State.stateID)
            {
                case (int) States.SearchingForJumpAndRun:
                    return "go north in\nthe forest";
                case (int) States.AfterJumpAndRunCompletedGoToMajor:
                    return "go back to\nthe major";
                case (int) States.SearchingForAdditionalParts:
                    return "search the map\nfor parts";
                case (int) States.SearchingForWindTurbineBuild:
                    return "find the turbine \nplatform and start\nbuilding the turbine";
            }
            return "";
        }

        public override void Setup()
        {
            Debug.Log("SETUP CALLED IN MISSION WIND TURBINE");
            switch (State.stateID)
            {
                case (int) States.SearchingForJumpAndRun:
                    HandleSearchingForJumpAndRun();
                    break;
                case (int) States.SearchingForAdditionalParts:
                    SpawnPartsToCollect();
                    break;
                case (int) States.SearchingForWindTurbineBuild:
                    break;
                case (int) States.MissionComplete:
                    MissionCompleteScript.MissionComplete();
                    GameStateManager.Instance.BaseMission.FinishCurrentMission(true);
                    break;
                case (int) States.MissionFailed:
                    MissionFailedScript.MissionFailed();
                    GameStateManager.Instance.BaseMission.FinishCurrentMission(false);
                    break;
            }
            GameStateManager.Instance.UpdateCurrentTask();
        }
        
        // private void HandleSearchingForJumpAndRun()
        // {
        //     bool isInVillage = SceneManager.GetActiveScene().name == Constants.SceneNames.village;
        //     bool didFinishJnR = GameStateManager.Instance.gameState.playerData.CheckMiniGameCompleted(MiniGame
        //         .jumpAndRunCollectTurbineParts);
        //     if (isInVillage)
        //     {
        //         if (didFinishJnR)
        //         {
        //             Debug.Log("Advance state jump and run completed");
        //             Debug.Log(GameStateManager.Instance.dialogueDisplay);
        //             GameStateManager.Instance.dialogueDisplay.StartNewDialogueForce(
        //                 "Missions/WindTurbine/completedJumpAndRun");  
        //         }
        //         else
        //         {
        //             Debug.Log("Advance state jump and run failed");
        //             Debug.Log(GameStateManager.Instance.dialogueDisplay);
        //             GameStateManager.Instance.dialogueDisplay.StartNewDialogueForce(
        //                 "Missions/WindTurbine/failedJumpAndRun");
        //         }
        //     }
        //     
        // }

        private void HandleSearchingForJumpAndRun()
        {
            bool isInVillage = SceneManager.GetActiveScene().name == Constants.SceneNames.village;
            bool didFinishJnR = GameStateManager.Instance.gameState.playerData.CheckMiniGameCompleted(MiniGame
                .jumpAndRunCollectTurbineParts);
            // TODO: change to if (isInVillage && didFinishJnR)
            if (isInVillage && !didFinishJnR)
            {
                State.stateID = (int)States.AfterJumpAndRunCompletedGoToMajor;
                GameObject majorDialogue = GameObject.Find("StartMayorDialogue");
                DialogueTrigger trigger = majorDialogue.GetComponent<DialogueTrigger>();
                trigger.dialoguePath = "Missions/WindTurbine/completedJumpAndRun";
            }
            else
            {
                // nothing changes, need to complete jnr
            }
            
        }

        private void SpawnPartsToCollect()
        {
            if (_didSpawnParts)
            {
                return;
            }
            _didSpawnParts = true;
        }

        public override void AdvanceState()
        {
            Debug.Log("ADVANCE STATE CALLED IN MISSION WIND TURBINE");
            // Debug.Log($"State.stateID {State.stateID}");
            switch (State.stateID)
            {
                // case (int) States.SearchingForJumpAndRun:
                //     break;
                case (int) States.SearchingForAdditionalParts:
                    SpawnPartsToCollect();
                    break;
                case (int) States.SearchingForWindTurbineBuild:
                    break;
                case (int) States.MissionComplete:
                    MissionCompleteScript.MissionComplete();
                    GameStateManager.Instance.BaseMission.FinishCurrentMission(true);
                    break;
                case (int) States.MissionFailed:
                    MissionFailedScript.MissionFailed();
                    GameStateManager.Instance.BaseMission.FinishCurrentMission(false);
                    break;
            }
            GameStateManager.Instance.UpdateCurrentTask();
        }

        public override void HandleAction(string action)
        {
            if (action == "") return;

            switch (action)
            {
                case "ActionAfterJumpAndRunSearchParts":
                    State.stateID = (int)States.SearchingForAdditionalParts;
                    AdvanceState();
                    break;
            }
            GameStateManager.Instance.UpdateCurrentTask();
        }
    }
}