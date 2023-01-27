using System.Collections.Generic;
using InventorySystem;
using Items;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Missions
{
    public sealed class MissionWindTurbine : Mission
    {
        public MissionWindTurbine() : base("WindTurbine", true)
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
            SetupAdditionalParts,
            SearchingForAdditionalParts,
            AllPartsCollectedGoBackToMayor,
            SearchingForWindTurbinePlatform,
            WindTurbineBuilt,
            MissionComplete,
            MissionFailed,
        }

        private const int NUM_PARTS_SPAWNED = 6;

        private List<Vector3> _items = new ();
        
        public override string GetCurrentTask()
        {
            switch (State.stateID)
            {
                case (int) States.SearchingForJumpAndRun:
                    return "go north in\nthe forest";
                case (int) States.AfterJumpAndRunCompletedGoBackToMayor:
                    return "go back to\nthe mayor";
                case (int) States.SearchingForAdditionalParts:
                    return GetSearchingPartsString();
                case (int) States.AllPartsCollectedGoBackToMayor:
                    return "all parts found\ngo back to mayor";
                case (int) States.SearchingForWindTurbinePlatform:
                    return "find the turbine \nplatform and start\nbuilding the turbine";
                case (int) States.WindTurbineBuilt:
                    return "go back to the \nmayor";
            }
            return "";
        }

        private string GetSearchingPartsString()
        {
            return $"search the map\nfor parts\nfound {NUM_PARTS_SPAWNED-_items.Count} of {NUM_PARTS_SPAWNED}";
        }

        public override void Setup()
        {
            Debug.Log("SETUP CALLED IN MISSION WIND TURBINE");
            switch (State.stateID)
            {
                case (int) States.SearchingForJumpAndRun:
                    InstantiateDialogueTriggerFromPrefab("Missions/WindTurbine/Triggers/", "TurbineBlocked");
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "Missions/WindTurbine/beforeJumpAndRun");
                    InstantiateSceneTriggerFromPrefab("Missions/WindTurbine/Triggers/", "StartJmpNRunMinigame");
                    break;
                case (int) States.AfterJumpAndRunCompletedGoBackToMayor:
                    InstantiateDialogueTriggerFromPrefab("Missions/WindTurbine/Triggers/", "TurbineBlocked");
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "Missions/WindTurbine/completedJumpAndRun");
                    break;
                case (int) States.SearchingForAdditionalParts:
                    InstantiateDialogueTriggerFromPrefab("Missions/WindTurbine/Triggers/", "TurbineBlocked");
                    RespawnPartsToCollect();
                    break;
                case (int) States.AllPartsCollectedGoBackToMayor:
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "Missions/WindTurbine/afterPartsCollected");
                    break;
                case (int) States.SearchingForWindTurbinePlatform:
                    InstantiateSceneTriggerFromPrefab("Missions/WindTurbine/Triggers/", "StartTurbineMinigame");
                    break;
                case (int) States.WindTurbineBuilt:
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "Missions/WindTurbine/completedFirstMission");
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
            switch (State.stateID)
            {
                case (int) States.SearchingForJumpAndRun:
                    InstantiateDialogueTriggerFromPrefab("Missions/WindTurbine/Triggers/", "TurbineBlocked");
                    InstantiateSceneTriggerFromPrefab("Missions/WindTurbine/Triggers/", "StartJmpNRunMinigame");
                    break;
                case (int) States.SetupAdditionalParts:
                    SpawnPartsToCollect();
                    State.stateID = (int)States.SearchingForAdditionalParts;
                    break;
                case (int) States.AllPartsCollectedGoBackToMayor:
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "Missions/WindTurbine/afterPartsCollected");
                    break;
                case (int) States.SearchingForWindTurbinePlatform:
                    InstantiateSceneTriggerFromPrefab("Missions/WindTurbine/Triggers/", "StartTurbineMinigame");
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

        void SpawnCableItem(Vector3 position)
        {
            _items.Add(position);
            InventoryDisplay.SpawnItem(position, ItemType.Cable);
        }

        private void CollectPart(Vector3 position)
        {
            _items.Remove(position);
            if (_items.Count == 0)
            {
                State.stateID = (int)States.AllPartsCollectedGoBackToMayor;
                AdvanceState();
            }
        }

        private void SpawnPartsToCollect()
        {
            ItemPickup.ItemPickedUp += (position, type) =>
            {
                CollectPart(position);
                GameStateManager.Instance.UpdateCurrentTask();
            };
            
            SpawnCableItem(new Vector3(0.8f, 1.8f, 0));
            SpawnCableItem(new Vector3(2.0f, 2.95f, 0));
            SpawnCableItem(new Vector3(-1.0f, 3.0f, 0));
            SpawnCableItem(new Vector3(3.666f, 3.0f, 0));
            SpawnCableItem(new Vector3(8.21f, 2.0f, 0));
            SpawnCableItem(new Vector3(7.612f, 3.8f, 0));
        }

        private void RespawnPartsToCollect()
        {
            if (SceneManager.GetActiveScene().name != "Village") return;
            // parts state is not saved to file, so if there are no parts and the game is in this state, just respawn all of them
            if (_items.Count == 0)
            {
                State.stateID = (int)States.SetupAdditionalParts;
                AdvanceState();
                return;
            }

            List<Vector3> itemsCpy = new List<Vector3>(_items);
            _items.Clear();
            for (int i = 0; i < itemsCpy.Count; i++)
            {
                SpawnCableItem(itemsCpy[i]);
            }
        }

        public override void HandleAction(string action)
        {
            if (action == "") return;

            switch (action)
            {
                case "ActionAfterJumpAndRunSearchParts":
                    State.stateID = (int)States.SetupAdditionalParts;
                    AdvanceState();
                    break;
                case "StartFindingWindTurbinePlatform":
                    State.stateID = (int)States.SearchingForWindTurbinePlatform;
                    AdvanceState();
                    break;
                case "MissionWindTurbineComplete":
                    State.stateID = (int)States.MissionComplete;
                    AdvanceState();
                    break;
            }
            GameStateManager.Instance.UpdateCurrentTask();
        }
    }
}