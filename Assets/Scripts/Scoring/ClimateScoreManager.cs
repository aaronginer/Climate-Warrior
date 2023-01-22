using System;
using Cinemachine;
using Missions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Scoring
{
    public class ClimateScoreManager : MonoBehaviour
    {
        public GameObject redBar;
        public GameObject orangeBar;
        private float _totalSeconds;
        private float _catastropheSeconds;
        
        public bool catastropheHappened;

        void Start()
        {
            _totalSeconds = GameStateManager.Instance.CurrentMission.ClimateScoreMaxTime;
            _catastropheSeconds = _totalSeconds / 2;
        }

        void Update()
        {
            Mission currentMission = GameStateManager.Instance.CurrentMission;
            if (currentMission.State.climateScoreSeconds <= 0)
            {
                currentMission.State.climateScoreSeconds = 0;
                return;
            }
            
            var redBarTransform = redBar.transform;
            var orangeBarTransform = orangeBar.transform;

            currentMission.State.climateScoreSeconds -= Time.deltaTime;
            float scaleX = currentMission.State.climateScoreSeconds / _totalSeconds;
            
            redBarTransform.localScale = new Vector3(scaleX, 1, 1);
            orangeBarTransform.localScale = new Vector3(scaleX, 1, 1);

            Debug.Assert(_catastropheSeconds < _totalSeconds);
            if (redBarTransform.localScale.x <= _catastropheSeconds / _totalSeconds
                && !catastropheHappened)
            {
                catastropheHappened = true;
            }
            else if (redBarTransform.localScale.x <= 0)
            {
                Debug.Log("Game over!");
            }
        }
    }
}

