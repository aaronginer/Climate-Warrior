
using Dialogue;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Missions
{
    public class MissionSabotage : Mission
    {
        public MissionSabotage(string name) : base(name)
        {
            ManualDialogueTriggers.Add("Panel1Sign");
        }

        public override void HandleAutomaticDialogueTriggers(GameObject obj, DialogueDisplay display)
        {
            if (!AutomaticDialogueTriggers.Contains(obj.name)) return;
            
            switch (obj.name)
            {
                    
            }
        }
        
        public override void HandleManualDialogueTriggers(GameObject obj, DialogueDisplay display)
        {
            if (!ManualDialogueTriggers.Contains(obj.name)) return;
            
            switch (obj.name)
            {
                case "Panel1Sign":
                {
                    display.StartNewDialogue("Missions/Sabotage/panel_1");
                    Object.Destroy(obj);
                    break;
                }
            }
        }

        public override void HandleSceneTriggers(GameObject obj)
        {
            switch (obj.name)
            {
                case "HydroPlantLowerFloor":
                    GameStateManager.Instance.gameState.playerData.position = new Vector3(2.01200008f, -0.746999979f, 0);
                    SceneManager.LoadScene("HydroPlantLower", LoadSceneMode.Single);
                    break;
                case "HydroPlantUpperFloor":
                    GameStateManager.Instance.gameState.playerData.position = new Vector3(1.00800002f, -0.425000012f, 0);
                    SceneManager.LoadScene("HydroPlantUpper", LoadSceneMode.Single);
                    break;
                case "HydroPlantEnter":
                    GameStateManager.Instance.gameState.playerData.position = new Vector3(1.35099995f, 0.504000008f, 0);
                    SceneManager.LoadScene("HydroPlantUpper", LoadSceneMode.Single);
                    break;
                case "HydroPlantExit":
                    GameStateManager.Instance.gameState.playerData.position = new Vector3(1.36699998f,1.301f,0);
                    SceneManager.LoadScene("River", LoadSceneMode.Single);
                    break; 
            }
        }
    }
}