using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Catastrophes
{
    public class Rain : MonoBehaviour
    {
        public GameObject grid;
        public GameObject canvas;
        private bool _active;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            _active = GameStateManager.Instance.gameState.catastropheState.state == CatastropheState.States.Flooding;
        }

        public void ToggleRain()
        {
            _active = !_active;
            SetActive();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SetActive();
        }
        
        private void SetActive()
        {
            grid.SetActive(_active);
            canvas.SetActive(_active);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}

