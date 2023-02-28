using Missions;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CurrentTaskDisplay : MonoBehaviour
    {
        public TextMeshProUGUI currentTask;
        private void Awake()
        {
            Debug.Log("Set currenttaskDisplay");
            GameStateManager.Instance.currentTaskDisplay = this;
            gameObject.SetActive(false);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void ShowCurrentTask()
        {
            Mission m = GameStateManager.Instance.CurrentMission == null
                ? GameStateManager.Instance.BaseMission
                : GameStateManager.Instance.CurrentMission;

            currentTask.text = m.GetCurrentTask();
            
            if (currentTask.text != "") Enable();
        }
    }
}