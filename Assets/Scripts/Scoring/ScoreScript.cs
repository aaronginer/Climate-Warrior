using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scoring
{
    public class ScoreScript : MonoBehaviour
    {
        public static ScoreScript Instance = null;

        private void Start()
        {
            AddScore(GameStateManager.Instance.gameState.score);
            Instance = this;
        }

        private void AddScore(int score)
        {
            GetComponentInChildren<AnimateCounterScript>().StartAnimate(score);
        }
        
        public static void Penalty(int penalty)
        {
            if (Instance == null) return;
            Instance.AddScore(penalty);
        }
    }
}