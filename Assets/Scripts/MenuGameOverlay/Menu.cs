using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
            GameStateManager.GSM.SaveToDisk();
        }
    }       
}
