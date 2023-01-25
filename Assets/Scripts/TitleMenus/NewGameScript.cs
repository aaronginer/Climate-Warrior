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

            SceneManager.LoadScene(Constants.SceneNames.village, LoadSceneMode.Single);
            
            GameStateManager.Instance.BaseMission = new BaseMission();
            PersistentCanvasScript.SpawnPersistentCanvas();
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(Constants.SceneNames.mainMenu, LoadSceneMode.Single);
        }
    }

}
