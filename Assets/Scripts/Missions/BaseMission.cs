using System.Collections.Generic;
using HighScore;
using Scoring;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Missions
{
    public sealed class BaseMission : Mission
    {
        public BaseMission() : base("")
        {
            State.missions = new List<string>(new[]
            {
                "WindTurbine",
                "Sabotage",
                "SolarPanel",
                
                // TODO: add missions here 
            });
            State.completedMissions = new List<string>();
        }

        public enum States
        {
            Init,
            PrepareMission,
            MissionActive,
            GameFinished,
        }

        private string CurrentOrNextMissionName()
        {
            return State.missions[0];
        }

        public override void Setup()
        {
            // (temporary)
            if (State.missions.Count == 0) State.stateID = (int) States.GameFinished;
            
            switch (State.stateID)
            {
                case (int) States.Init:
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "mayorgreetings");
                    break;
                case (int) States.PrepareMission:
                    SpawnMayorDialogue();
                    break;
                case (int) States.MissionActive:
                    break;
                case (int) States.GameFinished:
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "gamefinished");
                    break;
            }
            GameStateManager.Instance.ShowCurrentTask();
        }
        
        // this is the message before a mission
        // based on State.missions array
        public override string GetCurrentTask()
        {
            Debug.Log("finished");
            return State.stateID == (int)States.GameFinished ? "The mayor wants to talk to you." : "";
        }
        
        public override void AdvanceState()
        {
            // (temporary)
            if (State.missions.Count == 0) State.stateID = (int) States.GameFinished;
            
            switch (State.stateID)
            {
                case (int) States.PrepareMission:
                    SpawnMayorDialogue();
                    break;
                case (int) States.MissionActive:
                    GameStateManager.Instance.StartMission(LoadMission(State.missions[0]));
                    break;
                case (int) States.GameFinished:
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "gamefinished");
                    break;
            }

            Debug.Log("State: " + (State.stateID == (int) States.GameFinished));
            GameStateManager.Instance.ShowCurrentTask();
        }

        public override void HandleAction(string action)
        {
            if (action == "") return;
            Debug.Log(action);
            switch (action)
            {
                case "InitDone":
                    State.stateID = (int) States.PrepareMission;
                    AdvanceState();
                    break;
                case "MissionWindTurbine":
                    PushMission("Wind Turbine");
                    GameStateManager.Instance.StartMission(new MissionWindTurbine());
                    State.stateID = (int) States.MissionActive;
                    break;
                case "MissionSolarPanel":
                    PushMission("Solar Panel");
                    GameStateManager.Instance.StartMission(new MissionSolarPanel());
                    State.stateID = (int) States.MissionActive;
                    break;
                case "MissionSabotage":
                    PushMission("Sabotage");
                    GameStateManager.Instance.StartMission(new MissionSabotage());
                    State.stateID = (int) States.MissionActive;
                    break;
                case "MissionFlooding":
                    PushMission("Flooding");
                    GameStateManager.Instance.StartMission(new MissionFlooding());
                    State.stateID = (int) States.MissionActive;
                    break;
                case "StartNextMission":
                    State.stateID = (int)States.MissionActive;
                    AdvanceState();
                    break;
                case "Penalty25":
                    ScoreScript.Penalty(-25);
                    break;
                case "Penalty200":
                    ScoreScript.Penalty(-200);
                    break;
                case "RespawnIfNotAccepted":
                    if (State.stateID != (int) States.MissionActive) SpawnMayorDialogue();
                    break;
                case "GameFinished":
                    PersistentCanvasScript.DestroyPersistentCanvas();
                    HighScoresManager.SaveHighScore(GameStateManager.Instance.gameState.playerData.name, GameStateManager.Instance.gameState.score, "overallhighscores");
                    SceneManager.LoadScene(Constants.SceneNames.credits);
                    break;
            }
            GameStateManager.Instance.ShowCurrentTask();
        }

        private void SpawnMayorDialogue()
        {
            if (State.missions.Count == 0) return;
            string missionName = State.missions[0];

            switch (missionName)
            {
                // Spawn the starting mayor dialogue
                case "WindTurbine":
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "Missions/WindTurbine/turbine_1");
                    break;
                case "SolarPanel":
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue", 
                        "Missions/SolarPanel/solarpanel_1");
                    break;
                case "Sabotage":
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "Missions/Sabotage/sabotage_1");
                    break;
                case "Flooding":
                    InstantiateDialogueTriggerFromPrefab("Missions/", "StartMayorDialogue",
                        "Missions/Flooding/flooding_1");
                    break;
                case "Drought":
                    break;
            }
        }

        public bool IsMissionCompleted(string missionName)
        {
            return State.completedMissions.Contains(missionName);
        }
        
        public void FinishCurrentMission(bool complete)
        {
            Debug.Assert(State.stateID == (int)States.MissionActive);

            if (complete)
            {
                string mission = GameStateManager.Instance.CurrentMission.Name;
                State.completedMissions.Add(mission);
                State.missions.Remove(mission);
            }
            
            GameStateManager.Instance.gameState.playerData.inventory.CleanInventory();
            
            // if catastrophe -> go to sidequest
            State.stateID = State.missions.Count == 0 ? (int) States.GameFinished : (int) States.PrepareMission;
            AdvanceState();
        }

        // TODO: define mission "tree" in base mission
        public void PushMission(string missionName)
        {
            State.missions.Insert(0, missionName);
        }
    }
}