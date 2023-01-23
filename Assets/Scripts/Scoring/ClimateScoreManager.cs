using System;
using Cinemachine;
using Missions;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Scoring
{
    public class ClimateScoreManager : MonoBehaviour
    {
        public GameObject frontBar;
        public GameObject backBar;
        public TextMeshProUGUI degreesText;
        private float _totalSeconds;
        public float secondsLeft;
        private float _catastropheSeconds;

        private const float MaxDegrees = 4.0f;
        
        public bool catastropheHappened;

        void Start()
        {
            _totalSeconds = GameStateManager.Instance.CurrentMission.ClimateScoreMaxTime;
            secondsLeft = _totalSeconds;
            _catastropheSeconds = _totalSeconds / 2;
        }

        void Update()
        {
            if (secondsLeft <= 0)
            {
                secondsLeft = 0;
                return;
            }
            
            var frontBarTransform = frontBar.transform;
            var backBarTransform = backBar.transform;

            secondsLeft -= Time.deltaTime;
            float scaleX = 1 - (secondsLeft / _totalSeconds);
            
            frontBarTransform.localScale = new Vector3(scaleX, 1, 1);
            frontBarTransform.GetComponent<Image>().color =
                Color.Lerp(new Color(0.04707184f, 0.7075472f, 0, 1), new Color(0.5283019f, 0, 0, 1), scaleX);
            backBarTransform.localScale = new Vector3(scaleX, 1, 1);
            backBarTransform.GetComponent<Image>().color =
                Color.Lerp(new Color(0.1369746f, 0.8301887f, 0, 1), new Color(0.6415094f, 0.08979349f, 0, 1), scaleX);
            
            float degreesCurrent = scaleX * MaxDegrees;
            degreesText.text = "+" + degreesCurrent.ToString("0.#") + " °C"; 

            Debug.Assert(_catastropheSeconds < _totalSeconds);
            if (frontBarTransform.localScale.x <= _catastropheSeconds / _totalSeconds
                && !catastropheHappened)
            {
                catastropheHappened = true;
            }
            else if (frontBarTransform.localScale.x <= 0)
            {
                Debug.Log("Game over!");
            }
        }
    }
}

