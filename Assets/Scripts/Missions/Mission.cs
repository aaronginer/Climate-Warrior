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
        public string name;
        
        public float MissionMaxTime;
        public int DeductionsDecisions;
        public int BaseScore;
        public int TimeScoreMax;
        public string _description;
        
        private bool _currentGameCompleted;

        protected Mission(string name, bool climateScoreEnabled=false)
        {
            this.name = name;
            State = new MissionState(name);

            // Instantiate the climate score canvas object for every mission that is not the MissionTree (base mission) 
            if (!climateScoreEnabled) return;
            var climateScorePrefab = Resources.Load("ClimateScore") as GameObject;
            var persistentCanvas = GameObject.Find("PersistentCanvas");

            ClimateScoreObject = Object.Instantiate(climateScorePrefab, persistentCanvas.transform);
        }

        public virtual void Setup() {}
        public virtual void AdvanceState() {}
        public virtual void HandleAction(string action) {}

        public virtual string GetCurrentTask()
        {
            return "";
        }

        protected void InstantiateDialogueTriggerFromPrefab(string path, string name, string dialoguePath = null)
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
            if (dialoguePath != null)
            {
                newObj.GetComponent<DialogueTrigger>().dialoguePath = dialoguePath;
            }
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
                "WindTurbine" => new MissionWindTurbine(),
                "Sabotage" => new MissionSabotage(),
                "Flooding" => new MissionFlooding(),
                _ => null
            };

            return mission;
        }
    }
}