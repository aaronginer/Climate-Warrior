using UnityEngine;

namespace Dialogue
{
    public class ChoiceAction : MonoBehaviour
    {
        public string action;

        public void HandleAction()
        {
            GameStateManager.Instance.CurrentMission?.HandleAction(action);
        }
    }
   
}