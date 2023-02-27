using Catastrophes;
using Missions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scoring
{
    public class ClimateScoreManager : MonoBehaviour
    {
        public GameObject frontBar;
        public GameObject backBar;
        public TextMeshProUGUI degreesText;
        private float _totalSeconds;
        private float _catastropheSeconds;

        private const float MaxDegrees = 4.0f;
        
        public bool catastropheHappened;

        private MissionState _currentMissionState;
        private Transform _front;
        private Transform _back;

        private void Start()
        {
            _front = frontBar.transform;
            _back = backBar.transform;
            _currentMissionState = GameStateManager.Instance.CurrentMission.State;
            _totalSeconds = GameStateManager.Instance.CurrentMission.MissionMaxTime;
            UpdateBars();
            _catastropheSeconds = _totalSeconds / 2;
        }

        void Update()
        {
            UpdateBars();

            Debug.Assert(_catastropheSeconds < _totalSeconds);
            if (_currentMissionState.timeLeft <= _totalSeconds / 2
                && !catastropheHappened)
            {
                catastropheHappened = true;
                GameObject.Find("Flooding")?.GetComponent<FloodingScript>().ToggleRain(true);
                GameStateManager.Instance.GetSoundScript().SoundThunder();
                GameStateManager.Instance.BaseMission.PushMission("Flooding");
                GameStateManager.Instance.gameState.catastropheState.state = CatastropheState.States.Flooding;
            }
            else if (_currentMissionState.timeLeft <= 0)
            {
                if (_currentMissionState.missionName == "Sabotage")
                {
                    _currentMissionState.stateID = (int)MissionSabotage.States.MissionFailed;
                }
                else if (_currentMissionState.missionName == "WindTurbine")
                {
                    _currentMissionState.stateID = (int)MissionWindTurbine.States.MissionFailed;
                }
                else if (_currentMissionState.missionName == "SolarPanel")
                {
                    _currentMissionState.stateID = (int)MissionSolarPanel.States.MissionFailed;
                }

                SceneManager.LoadScene(GameStateManager.Instance.gameState.playerData.sceneName, LoadSceneMode.Single);
            }
        }

        void UpdateBars()
        {
            float scaleX = 1 - (_currentMissionState.timeLeft / _totalSeconds);
            
            _front.localScale = new Vector3(scaleX, 1, 1);
            _front.GetComponent<Image>().color =
                Color.Lerp(new Color(0.04707184f, 0.7075472f, 0, 1), new Color(0.5283019f, 0, 0, 1), scaleX);
            _back.localScale = new Vector3(scaleX, 1, 1);
            _back.GetComponent<Image>().color =
                Color.Lerp(new Color(0.1369746f, 0.8301887f, 0, 1), new Color(0.6415094f, 0.08979349f, 0, 1), scaleX);
            
            float degreesCurrent = scaleX * MaxDegrees;
            degreesText.text = "+" + degreesCurrent.ToString("0.#") + " °C"; 
        }
    }
}

