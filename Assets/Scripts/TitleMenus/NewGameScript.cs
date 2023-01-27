using Missions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TitleMenus
{
    public class NewGameScript : MonoBehaviour
    {
        public GameObject nameTextField;
        public Toggle toggleMale;
        public Toggle toggleFemale;
        public Toggle toggleDiverse;
        private TextMeshProUGUI _nameTextField;
        // Start is called before the first frame update
        void Start()
        {
            _nameTextField = nameTextField.GetComponent<TextMeshProUGUI>();
        }

        public void StartGame()
        {
            Time.timeScale = 1;
            GameStateManager.Instance.gameState.playerData.name = _nameTextField.text;
            GameStateManager.Instance.gameState.playerData.gender = toggleMale.isOn ? PlayerData.Gender.Male : 
                    toggleFemale.isOn ? PlayerData.Gender.Female : PlayerData.Gender.Diverse;
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
