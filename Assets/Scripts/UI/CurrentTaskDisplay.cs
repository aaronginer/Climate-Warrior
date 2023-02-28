using Missions;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CurrentTaskDisplay : MonoBehaviour
    {
        public TextMeshProUGUI currentMission;
        public TextMeshProUGUI currentTask;
        private void Start()
        {
            GameStateManager.Instance.currentTaskDisplay = this;
            ShowCurrentTask();
        }

        private void OnDestroy()
        {
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
        }
    }
}