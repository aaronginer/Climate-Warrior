using System.Collections.Generic;
using Dialogue;
using UnityEngine;

namespace Missions
{
    public class Mission : IMission
    {
        public MissionState State;
        
        
        protected readonly List<string> ManualDialogueTriggers = new ();
        protected readonly List<string> AutomaticDialogueTriggers = new ();

        protected Mission(string name)
        {
            State = new MissionState(name);
        }

        public virtual bool IsManualDialogueTrigger(GameObject obj)
        {
            return ManualDialogueTriggers.Contains(obj.name);
        }

        public virtual bool IsAutomaticDialogueTrigger(GameObject obj)
        {
            return AutomaticDialogueTriggers.Contains(obj.name);
        }
        
        public virtual void HandleAutomaticDialogueTriggers(GameObject obj, DialogueDisplay display) {}
        public virtual void HandleManualDialogueTriggers(GameObject obj, DialogueDisplay display) {}
        public virtual void HandleSceneTriggers(GameObject obj) {}
    }
}