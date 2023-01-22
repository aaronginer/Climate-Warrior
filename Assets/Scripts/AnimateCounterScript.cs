using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimateCounterScript : MonoBehaviour
{
    private int _countTo;
    private const float Seconds = 1f;
    private float _secondsLeft;

    private TextMeshProUGUI _text;
    private bool _animate;
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.text = "0";
    }

    private void Update()
    {
        if (!_animate)
        {
            return;
        }

        _secondsLeft -= Time.deltaTime;

        int scoreDisplay = (int) ((1 - _secondsLeft / Seconds) * _countTo);

        _text.text = "" + (_secondsLeft < 0 ? _countTo : scoreDisplay);

        if (scoreDisplay == _countTo)
        {
            _animate = false;
        }
    }

    public void StartAnimate(int countTo)
    {
        _countTo = countTo;
        _secondsLeft = Seconds;
        _animate = true;
    }
}
