using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace MenuGameOverlay
{
    public class Menu : MonoBehaviour
    {
        public GameObject menuContainer;
        public GameObject controlsLayout;
        public TextMeshProUGUI controlsText;

        private bool _active;


    
        public void Start()
        {
            _active = false;
            menuContainer.SetActive(_active);
            controlsLayout.SetActive(_active);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && UIStateManager.UISM.CanOpenMenuOverlay())
            {
                _active = !_active;
                menuContainer.SetActive(_active);
                if (_active == false) {
                    controlsLayout.SetActive(false);
                }
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

        public void Controls()
        {
            controlsText.text = "ESC -> close this menu\nWASD -> walk around\nQ -> open/close quest menu\nE -> open/close inventory\nspace bar -> advance dialogue\nleft click -> choose dialogue option";
            controlsLayout.SetActive(true);

            
        }
    }       
}
