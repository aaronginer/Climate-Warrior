using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TitleMenus
{
    public class NewGameScript : MonoBehaviour
    {
        public GameObject nameTextField;
        private TextMeshProUGUI _nameTextField;
        // Start is called before the first frame update
        void Start()
        {
            _nameTextField = nameTextField.GetComponentInChildren<TextMeshProUGUI>();
        }

        public void StartGame()
        {
            GameStateManager.GSM.gameState.playerData = new PlayerData(_nameTextField.text);
            GameStateManager.GSM.SaveToDisk();

            SceneManager.LoadScene("Scene1", LoadSceneMode.Single);
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

}
