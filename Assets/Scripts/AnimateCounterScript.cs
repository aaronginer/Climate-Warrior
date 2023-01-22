using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class AnimateCounterScript : MonoBehaviour
{
    private int _startValue = 0;
    private int _countTo;
    private const float Seconds = 1f;
    private float _secondsLeft;

    private TextMeshProUGUI _text;
    private bool _animate;
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.text = "" + _startValue;
    }

    private void Update()
    {
        if (!_animate)
        {
            return;
        }

        _secondsLeft -= Time.deltaTime;

        int scoreDisplay = _startValue + (int) ((1 - _secondsLeft / Seconds) * _countTo);
        int max = _startValue + _countTo;

        scoreDisplay = (_secondsLeft < 0 ? max : scoreDisplay);
        
        _text.text = "" + scoreDisplay;

        if (scoreDisplay == max)
        {
            _startValue += _countTo;
            _animate = false;
        }
    }

    public void StartAnimate(int countTo)
    {
        StartCoroutine(YieldLoop(countTo));
    }

    IEnumerator YieldLoop(int countTo)
    {
        // if for some reason another animation is currently going on, this waits for it to finish
        while (_animate)
        {
            yield return new WaitForSeconds(0);
        }

        _countTo = countTo;
        _secondsLeft = Seconds;
        _animate = true;
    }
}
