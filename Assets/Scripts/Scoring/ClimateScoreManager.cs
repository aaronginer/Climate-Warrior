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
        public float secondsLeft;
        private float _catastropheSeconds;
        
        public bool catastropheHappened;

        void Start()
        {
            _totalSeconds = GameStateManager.Instance.CurrentMission.ClimateScoreMaxTime;
            secondsLeft = _totalSeconds;
            _catastropheSeconds = _totalSeconds / 2;
        }

        void Update()
        {
            Mission currentMission = GameStateManager.Instance.CurrentMission;
            if (secondsLeft <= 0)
            {
                secondsLeft = 0;
                return;
            }
            
            var redBarTransform = redBar.transform;
            var orangeBarTransform = orangeBar.transform;

            secondsLeft -= Time.deltaTime;
            float scaleX = secondsLeft / _totalSeconds;
            
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

