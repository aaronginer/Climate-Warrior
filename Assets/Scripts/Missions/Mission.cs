using System;
using System.Collections.Generic;
using System.Linq;
using Dialogue;
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
        
        protected readonly Dictionary<string, List<string>> ManualDialogueTriggers = new ();
        protected readonly Dictionary<string, List<string>> AutomaticDialogueTriggers = new ();

        protected readonly Dictionary<string, List<string>> SceneTriggers = new();

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

            foreach (var scene in Scenes)
            {
                ManualDialogueTriggers.Add(scene, new List<string>());
                AutomaticDialogueTriggers.Add(scene, new List<string>());
                SceneTriggers.Add(scene, new List<string>());
            }
        }

        public bool IsManualDialogueTrigger(GameObject obj)
        {
            return ManualDialogueTriggers[SceneManager.GetActiveScene().name].Contains(obj.name);
        }

        public bool IsAutomaticDialogueTrigger(GameObject obj)
        {
            return AutomaticDialogueTriggers[SceneManager.GetActiveScene().name].Contains(obj.name);
        }
        
        public virtual void Setup() {}
        public virtual void AdvanceState() {}
        public virtual void HandleAction(string action) {}
        
        public virtual void HandleAutomaticDialogueTriggers(GameObject obj, DialogueDisplay display) {}
        public virtual void HandleManualDialogueTriggers(GameObject obj, DialogueDisplay display) {}
        public virtual void HandleSceneTriggers(GameObject obj) {}

        protected void InstantiateDialogueTriggerFromPrefab(string path, string name)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            if (!AutomaticDialogueTriggers.ContainsKey(sceneName) &&
                !ManualDialogueTriggers.ContainsKey(sceneName)) return;
            // only instantiate if current scene is correct
            if (!AutomaticDialogueTriggers[sceneName].Contains(name)
                && !ManualDialogueTriggers[sceneName].Contains(name)) return;
            
            var parent = GameObject.Find("DialogueTriggers");
            var obj = Resources.Load(path + name);
            
            var newObj = Object.Instantiate(obj, parent.transform);
            newObj.name = name;
        }
        
        protected void InstantiateSceneTriggerFromPrefab(string path, string name)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            if (!SceneTriggers.ContainsKey(sceneName)) return;
            
            // only instantiate if current scene is correct
            if (!SceneTriggers[sceneName].Contains(name)) return;
            
            var parent = GameObject.Find("SceneTriggers");
            var obj = Resources.Load(path + name);
            
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