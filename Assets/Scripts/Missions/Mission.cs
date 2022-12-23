using System;
using System.Collections.Generic;
using System.Linq;
using Dialogue;
using Triggers;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Missions
{
    public class Mission : IMission
    {
        public MissionState State;

        private static readonly string[] Scenes = new[]
        {
            "Village",
            "HydroPlantUpper",
            "HydroPlantLower",
            "River"
        };
        
        protected Mission(string name)
        {
            State = new MissionState(name);
        }

        public virtual void Setup() {}
        public virtual void AdvanceState() {}
        public virtual void HandleAction(string action) {}

        protected void InstantiateDialogueTriggerFromPrefab(string path, string name)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            var obj = Resources.Load(path + name) as GameObject;
            if (obj == null) return;

            DialogueTrigger triggerScript = obj.GetComponent<DialogueTrigger>();
            if (triggerScript is null) return;

            if (sceneName != triggerScript.scene) return;
            
            var parent = GameObject.Find("DialogueTriggers");
            
            var newObj = Object.Instantiate(obj, parent.transform);
            newObj.name = name;
        }
        
        protected void InstantiateSceneTriggerFromPrefab(string path, string name)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            var obj = Resources.Load(path + name) as GameObject;
            
            if (obj == null) return;
            
            SceneTrigger triggerScript = obj.GetComponent<SceneTrigger>();
            if (triggerScript == null) return;

            if (sceneName != triggerScript.scene) return;

            
            var parent = GameObject.Find("SceneTriggers");
            
            var newObj = Object.Instantiate(obj, parent.transform);
            newObj.name = name;
        }
        
        public static Mission LoadMission(MissionState state)
        {
            if (state == null) return null;

            Mission mission = state.missionName switch
            {
                "Sabotage" => new MissionSabotage(),
                _ => null
            };

            if (mission != null) mission.State = state;
            
            return mission;
        }
    }
}