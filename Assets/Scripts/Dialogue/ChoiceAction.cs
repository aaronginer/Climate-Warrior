using UnityEngine;

namespace Dialogue
{
    public class ChoiceAction : MonoBehaviour
    {
        public string action;

        public void HandleAction()
        {
            GameStateManager.Instance.BaseMission.HandleAction(action);
            GameStateManager.Instance.CurrentMission?.HandleAction(action);
        }
    }
   
}