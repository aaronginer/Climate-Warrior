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
        
        public readonly float MissionMaxTime;
        public int DeductionsDecisions;
        public int BaseScore;
        public int TimeScoreMax;
        
        private bool _currentGameCompleted;

        protected Mission(string name, int baseScore=0, int timeScoreMax=0, float missionTime=120, bool climateScoreEnabled=false)
        {
            BaseScore = baseScore;
            TimeScoreMax = timeScoreMax;
            MissionMaxTime = missionTime;
            State = new MissionState(name)
            {
                timeLeft = missionTime
            };

            // Instantiate the climate score canvas object for every mission that is not the MissionTree (base mission) 
            if (!climateScoreEnabled) return;
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

        public void UpdateTime()
        {
            if (State.timeLeft <= 0)
            {
                State.timeLeft = 0;
                return;
            }
            State.timeLeft -= Time.deltaTime;
        }
        
        public static Mission LoadMission(string missionName)
        {
            Mission mission = missionName switch
            {
                "Sabotage" => new MissionSabotage(),
                "Flooding" => new MissionSabotage(),
                _ => null
            };

            return mission;
        }
    }
}