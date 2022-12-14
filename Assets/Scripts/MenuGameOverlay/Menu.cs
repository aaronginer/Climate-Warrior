using System;
using System.Collections;
using System.Collections.Generic;
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
            GameStateManager.Instance.SaveToDisk();
            SceneManager.LoadScene("MainMenu");
        }
    }       
}
