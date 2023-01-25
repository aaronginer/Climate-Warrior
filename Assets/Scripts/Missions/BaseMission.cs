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
                "Sabotage",
            });
        }

        public enum States
        {
            PrepareMission,
            MissionActive,
            SideQuest,
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
                case (int) States.SideQuest:
                    break;
            }
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
                case (int) States.SideQuest:
                    break;
            }
        }

        public override void HandleAction(string action)
        {
            if (action == "") return;
            
            switch (action)
            {
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
        }

        private void SpawnMayorDialogue()
        {
            Debug.Assert(State.missions.Count != 0);
            string missionName = State.missions[0];

            switch (missionName)
            {
                // Spawn the starting mayor dialogue
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
        
        public void FinishMission(bool complete)
        {
            Debug.Assert(State.stateID == (int)States.MissionActive);

            if (complete)
            {
                State.missions.RemoveAt(0);
            }

            // if catastrophe -> go to sidequest
            State.stateID = (int) States.PrepareMission;
            AdvanceState();
        }

        public void PushMission(string missionName)
        {
            State.missions.Insert(0, missionName);
        }
    }
}