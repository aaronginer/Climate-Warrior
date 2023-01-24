using System;
using UnityEngine;

namespace Scoring
{
    public class ScoreScript : MonoBehaviour
    {
        private void Start()
        {
            GetComponentInChildren<AnimateCounterScript>().StartAnimate(GameStateManager.Instance.gameState.score);
        }
    }
}