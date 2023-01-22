using Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Missions
{
    public class Mission : IMission
    {
        public MissionState State;
        public readonly float ClimateScoreMaxTime;
        private bool _currentGameCompleted;

        // mission tree a just another mission? would give benefit of not having to implement a new system
        
        protected Mission(string name, float climateScoreTime)
        {
            ClimateScoreMaxTime = climateScoreTime;
            State = new MissionState(name, climateScoreTime);
            
            // Instantiate the climate score object
            var climateScorePrefab = Resources.Load("Missions/ClimateScore") as GameObject;
            Debug.Assert(climateScorePrefab);
            Object.Instantiate(climateScorePrefab, GameObject.Find("Canvas").transform);
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
            AdvanceState();
        }

        public bool IsCurrentGameCompleted()
        {
            bool completed = _currentGameCompleted;
            _currentGameCompleted = false;
            return completed;
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