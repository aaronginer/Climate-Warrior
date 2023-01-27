using Missions;
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
            _nameTextField = nameTextField.GetComponent<TextMeshProUGUI>();
        }

        public void StartGame()
        {
            GameStateManager.Instance.gameState = new GameState
            {
                playerData = new PlayerData(_nameTextField.text)
            };
            GameStateManager.Instance.SaveToDisk();

            PersistentCanvasScript.SpawnPersistentCanvas();
            
            SceneManager.LoadScene(Constants.SceneNames.openingcutscene, LoadSceneMode.Single);
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(Constants.SceneNames.mainMenu, LoadSceneMode.Single);
        }
    }

}
