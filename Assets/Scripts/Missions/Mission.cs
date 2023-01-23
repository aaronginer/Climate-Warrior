using Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Missions
{
    public class Mission : IMission
    {
        public MissionState State;
        public readonly GameObject ClimateScoreObject;
        public readonly float ClimateScoreMaxTime;
        private bool _currentGameCompleted;

        // mission tree a just another mission? would give benefit of not having to implement a new system
        
        protected Mission(string name, bool climateScoreEnabled=false, float climateScoreTime=120)
        {
            ClimateScoreMaxTime = climateScoreTime;
            State = new MissionState(name);

            // Instantiate the climate score canvas object for every mission that is not the MissionTree (base mission) 
            if (!climateScoreEnabled) return;

            State.timeLeft = climateScoreTime;
            
            var climateScorePrefab = Resources.Load("ClimateScore") as GameObject;
            var persistentCanvas = GameObject.Find("PersistentCanvas");

            ClimateScoreObject = Object.Instantiate(climateScorePrefab, persistentCanvas.transform);
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

        public void CompleteCurrentGame()
        {
            _currentGameCompleted = true;
        }

        public bool IsCurrentGameCompleted()
        {
            bool completed = _currentGameCompleted;
            _currentGameCompleted = false;
            return completed;
        }
        
        public static Mission LoadMission(string missionName)
        {
            Mission mission = missionName switch
            {
                "Sabotage" => new MissionSabotage(),
                _ => null
            };

            return mission;
        }
    }
}