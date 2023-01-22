using System;
using UnityEngine;

namespace Scoring
{
    public class ScoreScript : MonoBehaviour
    {
        private void Awake()
        {
            GetComponentInChildren<AnimateCounterScript>().StartAnimate(GameStateManager.Instance.gameState.score);
        }
    }
}