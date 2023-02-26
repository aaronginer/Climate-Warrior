using System.Collections.Generic;
using Scoring;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Missions
{
    public sealed class BaseMission : Mission
    {
        public BaseMission() : base("BaseMission")
        {
            State.missions = new List<string>(new[]
            {
                "SolarPanel",
                "WindTurbine",
                "Sabotage",
                
                
                // TODO: add missions here 
            });
            State.completedMissions = new List<string>();
        }

        public enum States
        {
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
        }
        
        // this is the message before a mission
        // based on State.missions array
        public string GetCurrentTaskBeforeMission()
        {
            if (State.missions.Count == 0) return "The mayor wants to\n talk to you";
            string nextMissionName = CurrentOrNextMissionName();
            switch (nextMissionName)
            {
                case "WindTurbine":
                    return "find the mayor";
                case "SolarPanel":
                    return "talk to the mayor";
                case "Sabotage":
                    return "talk to the mayor";
                case "Flooding":
                    return "talk to the mayor";
                case "Drought":
                    return "not implemented";
            }
            return "";
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
            GameStateManager.Instance.UpdateCurrentTask();
        }

        public override void HandleAction(string action)
        {
            if (action == "") return;
            Debug.Log(action);
            switch (action)
            {
                case "MissionWindTurbine":
                    PushMission("WindTurbine");
                    GameStateManager.Instance.StartMission(new MissionWindTurbine());
                    State.stateID = (int) States.MissionActive;
                    break;
                case "MissionSolarPanel":
                    PushMission("SolarPanel");
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
                    SceneManager.LoadScene(Constants.SceneNames.credits);
                    break;
            }
            GameStateManager.Instance.UpdateCurrentTask();
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
                string mission = GameStateManager.Instance.CurrentMission.name;
                State.completedMissions.Add(mission);
                State.missions.Remove(mission);
            }

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