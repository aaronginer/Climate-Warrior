using System;
using UnityEngine;

namespace Triggers
{
    public class Trigger : MonoBehaviour 
    {
        public string scene; // the scene the trigger is positioned in
        public bool manual; // is the trigger a manual trigger?
        public string text; // text to be displayed when trigger is manual
        public string requiredMission; // requirement of current mission
        public int requiredMissionState; // requirement of mission in state
        public bool isReverseCheckForMissionState; // all other states are allowed

        private bool ChecksForMissionAndState()
        {
            return !String.IsNullOrEmpty(requiredMission) && requiredMissionState >= 0;
        }

        public bool ShouldBlockTriggerByRequiredMission()
        {
            bool checksForMissionAndState = ChecksForMissionAndState();
            if (!checksForMissionAndState)
            {
                return false;
            }
            bool isInMissionAndState = checksForMissionAndState &&
                                       GameStateManager.Instance.CheckIfMissionAndState(requiredMission,
                                           requiredMissionState);
            bool shouldBlock = checksForMissionAndState &&
                   (isReverseCheckForMissionState ? isInMissionAndState : !isInMissionAndState);

            return shouldBlock;
        }
        
    }
}