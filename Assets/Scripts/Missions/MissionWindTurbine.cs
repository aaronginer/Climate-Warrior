using System.Collections.Generic;
using InventorySystem;
using Items;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Missions
{
    public sealed class MissionWindTurbine : Mission
    {
        public MissionWindTurbine() : base("Wind Turbine", true)
        {
            BaseScore = 500;
            TimeScoreMax = 1000;
            MissionMaxTime = 480;
            State.timeLeft = MissionMaxTime;
            _description = "The city is planning on building a wind turbine, but resources are still missing. Help the city get all the parts they need.";
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
            Debug.Log("State id: " + State.stateID);
            switch (State.stateID)
            {
                case (int) States.SearchingForJumpAndRun:
                    return "Go north to the forest.";
                case (int) States.AfterJumpAndRunCompletedGoBackToMayor:
                    return "Talk to the mayor.";
                case (int) States.SearchingForAdditionalParts:
                    return GetSearchingPartsString();
                case (int) States.AllPartsCollectedGoBackToMayor:
                    return "All parts found. Return to the mayor.";
                case (int) States.SearchingForWindTurbinePlatform:
                    return "Find the wind turbine platform and start building it.";
                case (int) States.WindTurbineBuilt:
                    return "Talk to the mayor.";
            }
            return "";
        }

        private string GetSearchingPartsString()
        {
            return $"Search the map for parts. \nFound {NUM_PARTS_SPAWNED-_items.Count} of {NUM_PARTS_SPAWNED}.";
        }

        public override void Setup()
        {
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
                    ItemPickup.ClearEventList();
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
            GameStateManager.Instance.ShowCurrentTask();
        }
        
        public override void AdvanceState()
        {
            Debug.Log(GameStateManager.Instance.currentTaskDisplay);
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
                    ItemPickup.ClearEventList();
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
            GameStateManager.Instance.ShowCurrentTask();
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
            ItemPickup.MissionItemPickUp += (position, type) =>
            {
                CollectPart(position);
                GameStateManager.Instance.ShowCurrentTask();
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
            GameStateManager.Instance.ShowCurrentTask();
        }
    }
}