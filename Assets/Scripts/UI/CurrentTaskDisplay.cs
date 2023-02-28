using Missions;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CurrentTaskDisplay : MonoBehaviour
    {
        public TextMeshProUGUI currentMission;
        public TextMeshProUGUI currentTask;
        private void Awake()
        {
            Debug.Log("Set currenttaskDisplay");
            GameStateManager.Instance.currentTaskDisplay = this;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Debug.Log("Nulled currenttaskdisplay");
            GameStateManager.Instance.currentTaskDisplay = null;
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

            currentMission.text = m.Name;
            currentTask.text = m.GetCurrentTask();
            
            if (currentTask.text != "") Enable();
        }
    }
}