using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuGameOverlay
{
    public class Menu : MonoBehaviour
    {
        public GameObject menuContainer;

        private bool _active;
    
        public void Start()
        {
            _active = false;
            menuContainer.SetActive(_active);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _active = !_active;
                menuContainer.SetActive(_active);
                // Time.timeScale = _active ? 0 : 1;
            }
        }

        public void SaveGameState()
        {
            GameStateManager.Instance.SaveToDisk();
        }

        public void ExitGame()
        {
            GameStateManager.Instance.SaveToDisk();
            Application.Quit();
        }

        public void BackToMenu()
        {
            PersistentCanvasScript.DestroyPersistentCanvas();
            GameStateManager.Instance.SaveToDisk();
            GameStateManager.Destroy();
            SceneManager.LoadScene("_preload");
        }
    }       
}
