using System.Collections.Generic;
using UnityEngine;

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
                    State.missions.Insert(0, "Sabotage");
                    GameStateManager.Instance.StartMission(new MissionSabotage());
                    State.stateID = (int) States.MissionActive;
                    break;
                case "MissionFlooding":
                    State.missions.Insert(0, "Flooding");
                    GameStateManager.Instance.StartMission(new MissionFlooding());
                    State.stateID = (int) States.MissionActive;
                    break;
                case "StartNextMission":
                    State.stateID = (int)States.MissionActive;
                    AdvanceState();
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
    }
}