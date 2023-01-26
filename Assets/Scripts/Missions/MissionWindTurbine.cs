using System;
using InventorySystem;
using Items;
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
            AfterJumpAndRunCompletedGoBackToMayor,
            SearchingForAdditionalParts,
            AllPartsCollectedGoBackToMayor,
            SearchingForWindTurbinePlatform,
            MissionComplete,
            MissionFailed,
        }

        private bool _didSpawnParts = false;
        private const int NUM_PARTS_SPAWNED = 6;
        private int _numPartsCollected = 0;
        
        public override string GetCurrentTask()
        {
            switch (State.stateID)
            {
                case (int) States.SearchingForJumpAndRun:
                    return "go north in\nthe forest";
                case (int) States.AfterJumpAndRunCompletedGoBackToMayor:
                    return "go back to\nthe major";
                case (int) States.SearchingForAdditionalParts:
                    return GetSearchingPartsString();
                case (int) States.AllPartsCollectedGoBackToMayor:
                    return "all parts found\ngo back to major";
                case (int) States.SearchingForWindTurbinePlatform:
                    return "find the turbine \nplatform and start\nbuilding the turbine";
            }
            return "";
        }

        private int CountCollectedCables()
        {
            return GameStateManager.Instance.gameState.playerData.inventory.CountInventoryItem(ItemType.Cable);
        }

        private string GetSearchingPartsString()
        {
            int amountCollectedCables = CountCollectedCables();
            return $"search the map\nfor parts\nfound {amountCollectedCables} of {NUM_PARTS_SPAWNED}";
        }

        public override void Setup()
        {
            Debug.Log("SETUP CALLED IN MISSION WIND TURBINE");
            switch (State.stateID)
            {
                case (int) States.SearchingForJumpAndRun:
                    SetupVillageInSearchingForJumpAndRun();
                    break;
                case (int) States.SearchingForAdditionalParts:
                    SpawnPartsToCollect();
                    break;
                case (int) States.SearchingForWindTurbinePlatform:
                    SetupVillageInSearchingForWindTurbinePlatform();
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
        
        public override void AdvanceState()
        {
            Debug.Log("ADVANCE STATE CALLED IN MISSION WIND TURBINE");
            // InventoryDisplay.SpawnItem(new Vector3(0.332f, 0f , 0), ItemType.Cable);
            // Debug.Log($"State.stateID {State.stateID}");
            switch (State.stateID)
            {
                case (int) States.SearchingForJumpAndRun:
                    GameStateManager.Instance.SetMayorDialogPath("Missions/WindTurbine/beforeJumpAndRun");
                    break;
                case (int) States.SearchingForAdditionalParts:
                    SpawnPartsToCollect();
                    break;
                case (int) States.AllPartsCollectedGoBackToMayor:
                    GameStateManager.Instance.SetMayorDialogPath("Missions/WindTurbine/afterPartsCollected");
                    break;
                case (int) States.SearchingForWindTurbinePlatform:
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
        
        private void SetupVillageInSearchingForJumpAndRun()
        {
            bool isInVillage = SceneManager.GetActiveScene().name == Constants.SceneNames.village;
            bool didFinishJnR = GameStateManager.Instance.gameState.playerData.CheckMiniGameCompleted(MiniGame
                .jumpAndRunCollectTurbineParts);
            if (isInVillage && didFinishJnR)
            {
                State.stateID = (int)States.AfterJumpAndRunCompletedGoBackToMayor;
                GameStateManager.Instance.SetMayorDialogPath("Missions/WindTurbine/completedJumpAndRun");
            }
        }
        
        private void SetupVillageInSearchingForWindTurbinePlatform()
        {
            bool isInVillage = SceneManager.GetActiveScene().name == Constants.SceneNames.village;
            bool didFinishBuildingTurbine = GameStateManager.Instance.gameState.playerData.CheckMiniGameCompleted(MiniGame
                .buildAWindTurbine);
            if (isInVillage && didFinishBuildingTurbine)
            {
                State.stateID = (int)States.MissionComplete;
                GameStateManager.Instance.SetMayorDialogPath("Missions/WindTurbine/completedFirstMission");
            }
        }

        void SpawnCableItem(Vector3 position)
        {
            InventoryDisplay.SpawnItem(position, ItemType.Cable);
        }

        private void CheckIfAllPartsCollected()
        {
            if (CountCollectedCables() >= NUM_PARTS_SPAWNED)
            {
                State.stateID = (int)States.AllPartsCollectedGoBackToMayor;
                AdvanceState();
            }
        }

        private void SpawnPartsToCollect()
        {
            if (_didSpawnParts)
            {
                return;
            }
            _didSpawnParts = true;

            InventoryDisplay.itemPickedUp += (ItemType type) =>
            {
                CheckIfAllPartsCollected();
                GameStateManager.Instance.UpdateCurrentTask();
                
            };
            
            SpawnCableItem(new Vector3(0.8f, 1.8f, 0));
            SpawnCableItem(new Vector3(2.0f, 2.95f, 0));
            SpawnCableItem(new Vector3(-1.0f, 3.0f, 0));
            SpawnCableItem(new Vector3(3.666f, 3.0f, 0));
            SpawnCableItem(new Vector3(8.21f, 2.0f, 0));
            SpawnCableItem(new Vector3(7.612f, 3.8f, 0));
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
                case "StartFindingWindTurbinePlatform":
                    State.stateID = (int)States.SearchingForWindTurbinePlatform;
                    AdvanceState();
                    break;
            }
            GameStateManager.Instance.UpdateCurrentTask();
        }
    }
}