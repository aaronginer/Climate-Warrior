using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;

namespace ClimateScore
{
    public class ClimateScoreManager : MonoBehaviour
    {
        public GameObject redBar;
        public GameObject orangeBar;
        private float _totalSeconds;
        private float _catastropheSeconds;
        
        private bool _catastropheHappened;
    
        void Start()
        {
            _totalSeconds = GameStateManager.Instance.CurrentMission.ClimateScoreMaxTime;
            _catastropheSeconds = _totalSeconds / 2;
        }

        void Update()
        {
            var redBarTransform = redBar.transform;
            var orangeBarTransform = orangeBar.transform;

            GameStateManager.Instance.CurrentMission.State.climateScoreSeconds -= Time.deltaTime;
            float scaleX = GameStateManager.Instance.CurrentMission.State.climateScoreSeconds / _totalSeconds;
            
            redBarTransform.localScale = new Vector3(scaleX, 1, 1);
            orangeBarTransform.localScale = new Vector3(scaleX, 1, 1);

            Debug.Assert(_catastropheSeconds < _totalSeconds);
            if (redBarTransform.localScale.x <= _catastropheSeconds / _totalSeconds
                && !_catastropheHappened)
            {
                _catastropheHappened = true;
                
            }
            else if (redBarTransform.localScale.x <= 0)
            {
                Debug.Log("Game over!");
            }
        }
    }
}

