using Items;
using InventorySystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Missions
{
    public sealed class MissionSolarPanel : Mission
    {
        public MissionSolarPanel() : base("SolarPanel", true)
        {
            BaseScore = 500;
            TimeScoreMax = 1000;
            MissionMaxTime = 300;
            State.timeLeft = MissionMaxTime;
            _description = "Find the borken solar panel and fix the pipes";
        }
         
        public enum States
        {
            SetupAdditionalParts,
            SearchForLockpick,
            FoundLockpickGoBackToMayor,
            SearchingForSolarPanel,
            FixingPipes,
            PipesFixed,
            MissionComplete,
            MissionFailed,
        }

        private const int NUM_PARTS_SPAWNED = 5;
        private Vector3 RESCALE_LOCKPICK = new Vector3(0.25f, 0.25f, 0.0f);
        private List<(Vector3, ItemType)> _items = new List<(Vector3, ItemType)>();

        public override void Setup()
        {
            Debug.Log("In MissionSolarPanel Setup()");
            switch (State.stateID)
            {
                case (int) States.SearchForLockpick:
                    ResapwnPartsToCollect();
                    break;
                case (int) States.FoundLockpickGoBackToMayor:
                    ItemPickup.ClearEventList();
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "Missions/SolarPanel/afterPartsCollected");
                    break;
                case (int) States.SearchingForSolarPanel:
                    InstantiateSceneTriggerFromPrefab("Missions/SolarPanel/Triggers/", "StartLockPickScene");
                    break;
                case (int)States.FixingPipes:
                    InstantiateSceneTriggerFromPrefab("Missions/SolarPanel/Triggers/", "StartSolarPipesScene");
                    break;
                case (int)States.PipesFixed:
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "Missions/SolarPanel/solarPipesFixed");
                    break;
                case (int)States.MissionComplete:
                    MissionCompleteScript.MissionComplete();
                    GameStateManager.Instance.BaseMission.FinishCurrentMission(true);
                    break;
                case (int)States.MissionFailed:
                    MissionFailedScript.MissionFailed();
                    GameStateManager.Instance.BaseMission.FinishCurrentMission(false);
                    break;
            }
            GameStateManager.Instance.ShowCurrentTask();
        }

        public override string GetCurrentTask()
        {
            switch (State.stateID)
            {
                case (int) States.SearchForLockpick:
                    return GetSearchingPartsString();
                case (int) States.FoundLockpickGoBackToMayor:
                    return "Go back to the mayor.";
                case (int) States.SearchingForSolarPanel:
                    return "Find the house with the broken solar panel.";
                case (int)States.FixingPipes:
                    return "Fix the pipe system of the broken solar panel.";
                case (int)States.PipesFixed:
                    return "Talk to the mayor.";
            }
            return "";
        }

        public override void AdvanceState()
        {
            switch (State.stateID)
            {
                case (int)States.SetupAdditionalParts:
                    SpawnPartsToCollect();
                    State.stateID = (int)States.SearchForLockpick;
                    break;
                case (int)States.FoundLockpickGoBackToMayor:
                    ItemPickup.ClearEventList();
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "Missions/SolarPanel/afterPartsCollected");
                    break;
                case (int)States.SearchingForSolarPanel:
                    InstantiateSceneTriggerFromPrefab("Missions/SolarPanel/Triggers/", "StartLockPickScene");
                    break;
                case (int)States.FixingPipes:
                    InstantiateSceneTriggerFromPrefab("Missions/SolarPanel/Triggers/", "SolarPipes");
                    break;
                case (int)States.PipesFixed:
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "Missions/SolarPanel/solarPipesFixed");
                    break;
                case (int)States.MissionComplete:
                    MissionCompleteScript.MissionComplete();
                    GameStateManager.Instance.BaseMission.FinishCurrentMission(true);
                    break;
                case (int)States.MissionFailed:
                    MissionFailedScript.MissionFailed();
                    GameStateManager.Instance.BaseMission.FinishCurrentMission(false);
                    break;

            }
            GameStateManager.Instance.ShowCurrentTask();
        }

        public override void HandleAction(string action)
        {
            if (action == "") return;

            switch (action)
            {
                case "StartSearchSolarPanel":
                    State.stateID = (int)States.SearchingForSolarPanel;
                    AdvanceState();
                    break;
                case "MissionSolarPanelComplete":
                    State.stateID = (int)States.MissionComplete;
                    AdvanceState();
                    break;
            }
            GameStateManager.Instance.ShowCurrentTask();
        }

        private string GetSearchingPartsString()
        {
            return $"Search the map for lockpicks. \nFound {NUM_PARTS_SPAWNED - _items.Count} of {NUM_PARTS_SPAWNED}.";
        }

        private void CollectPart(Vector3 position, ItemType item)
        {
            _items.Remove((position, item));
            if (_items.Count == 0)
            {
                State.stateID = (int)States.FoundLockpickGoBackToMayor;
                AdvanceState();
            }
        }

        private void SpawnItem(Vector3 position, ItemType itemType)
        {
            _items.Add((position, itemType));
            InventoryDisplay.SpawnItem(position, itemType);

            GameObject item = GameObject.Find(((int)itemType).ToString());//.GetComponent<SpriteRenderer>();
            item.transform.localScale = RESCALE_LOCKPICK;

        }

        private void SpawnPartsToCollect()
        {
            ItemPickup.MissionItemPickUp += (position, type) =>
            {
                CollectPart(position, type);
                GameStateManager.Instance.ShowCurrentTask();
            };

            SpawnItem(new Vector3(4.9f, 2.33f, 0), ItemType.LockPick1);
            SpawnItem(new Vector3(8.293f, 1.656f, 0), ItemType.LockPick2);
            SpawnItem(new Vector3(3.132f, 0.67f, 0), ItemType.LockPick3);
            SpawnItem(new Vector3(1.95f, 3.75f, 0), ItemType.LockPick4);
            SpawnItem(new Vector3(0.8f, 1.8f, 0), ItemType.LockPick5);

        }


        private void ResapwnPartsToCollect()
        {
            if (SceneManager.GetActiveScene().name != "Village") return;
            // parts state is not saved to file, so if there are no parts and the game is in this state, just respawn all of them
            if (_items.Count == 0)
            {
                State.stateID = (int)States.SetupAdditionalParts;
                AdvanceState();
                return;
            }

            List<(Vector3, ItemType)> itemsCpy = new List<(Vector3, ItemType)>(_items);
            _items.Clear();
            for (int i = 0; i < itemsCpy.Count; i++)
            {
                SpawnItem(itemsCpy[i].Item1, itemsCpy[i].Item2);
            }
        }
    }

}