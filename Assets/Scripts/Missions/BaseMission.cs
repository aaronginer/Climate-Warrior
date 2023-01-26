using System;
using System.Collections.Generic;
using Scoring;
using UnityEngine;
using UnityEngine.Assertions;

namespace Missions
{
    public sealed class BaseMission : Mission
    {
        public BaseMission() : base("BaseMission")
        {
            State.missions = new List<string>(new[]
            {
                "Sabotage",
                "WindTurbine",
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
            switch (State.stateID)
            {
                case (int) States.PrepareMission:
                    SpawnMayorDialogue();
                    break;
                case (int) States.MissionActive:
                    break;
                case (int) States.GameFinished:
                    Debug.Log("Finished the game");
                    break;
            }
        }
        
        // this is the message before a mission
        // based on State.missions array
        public string GetCurrentTaskBeforeMission()
        {
            if (State.missions.Count == 0) return "";
            string nextMissionName = CurrentOrNextMissionName();
            switch (nextMissionName)
            {
                case "WindTurbine":
                    return "find the major";
                case "Sabotage":
                    return "find sabotage mission";
                case "Flooding":
                    return "find flooding mission";
                case "Drought":
                    return "find flooding mission";
            }
            return "";
        }
        
        public override void AdvanceState()
        {
            switch (State.stateID)
            {
                case (int) States.PrepareMission:
                    SpawnMayorDialogue();
                    break;
                case (int) States.MissionActive:
                    GameStateManager.Instance.StartMission(LoadMission(State.missions[0]));
                    break;
                case (int) States.GameFinished:
                    Debug.Log("Finished the game");
                    break;
            }
            GameStateManager.Instance.UpdateCurrentTask();
        }

        public override void HandleAction(string action)
        {
            if (action == "") return;
            
            switch (action)
            {
                case "MissionWindTurbine":
                    PushMission("WindTurbine");
                    GameStateManager.Instance.StartMission(new MissionWindTurbine());
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
                    InstantiateDialogueTriggerFromPrefab("Missions/WindTurbine/Triggers/", "WindTurbine1Dialogue");
                    break;
                case "Sabotage":
                    InstantiateDialogueTriggerFromPrefab("Missions/Sabotage/Triggers/", "Sabotage1Dialogue");
                    break;
                case "Flooding":
                    InstantiateDialogueTriggerFromPrefab("Missions/Flooding/Triggers/", "Flooding1Dialogue");
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
                State.completedMissions.Add(State.missions[0]);
                State.missions.RemoveAt(0);
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