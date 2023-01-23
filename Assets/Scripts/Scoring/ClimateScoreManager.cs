using System;
using Missions;
using TMPro;
using UnityEngine;
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
            _totalSeconds = GameStateManager.Instance.CurrentMission.ClimateScoreMaxTime;
            UpdateBars();
            _catastropheSeconds = _totalSeconds / 2;
        }

        void Update()
        {
            if (_currentMissionState.timeLeft <= 0)
            {
                _currentMissionState.timeLeft = 0;
                return;
            }
            
            UpdateBars();

            Debug.Assert(_catastropheSeconds < _totalSeconds);
            if (_currentMissionState.timeLeft <= 0
                && !catastropheHappened)
            {
                catastropheHappened = true;
            }
            else if (_front.localScale.x <= 0)
            {
                Debug.Log("Game over!");
            }
        }

        void UpdateBars()
        {
            _currentMissionState.timeLeft -= Time.deltaTime;
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

