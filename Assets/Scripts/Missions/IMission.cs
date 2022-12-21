using Dialogue;
using UnityEngine;

namespace Missions
{
    public interface IMission
    {
        public bool IsManualDialogueTrigger(GameObject obj);
        public bool IsAutomaticDialogueTrigger(GameObject obj);
        public void HandleManualDialogueTriggers(GameObject obj, DialogueDisplay display);
        public void HandleAutomaticDialogueTriggers(GameObject obj, DialogueDisplay display);
        public void HandleSceneTriggers(GameObject obj);
    }
}