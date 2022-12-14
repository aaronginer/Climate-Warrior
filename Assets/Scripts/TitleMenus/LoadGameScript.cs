using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Missions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TitleMenus
{
    public class LoadGameScript : MonoBehaviour
    {
        public GameObject savesButtonsContainer;
        public int savesButtonsMargin;

        public string selectedSave = "";
        public TextMeshProUGUI selectedSaveText;

        private string _persistentPath = "";
        // Start is called before the first frame update
        void Start()
        {
            _persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;

            ListSaves();
        }
        
        private void ListSaves()
        {
            var saveFilesPaths = Directory.GetFiles(_persistentPath).Where(f => f.Contains("cw_save_")).ToArray();

            for (int i = 0; i < saveFilesPaths.Length; i++)
            {
                string[] split = saveFilesPaths[i].Split(Path.AltDirectorySeparatorChar);
                var saveName = split[^1].Replace(".json", "").Replace("cw_save_", "");

                GameObject button = Instantiate(Resources.Load("SelectSaveButton"), 
                    savesButtonsContainer.transform) as GameObject;

                if (button == null) continue;
                
                button.GetComponentInChildren<TextMeshProUGUI>().text = saveName;
                button.GetComponent<RectTransform>().localPosition += new Vector3(0, -i*savesButtonsMargin, 0);

                savesButtonsContainer.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 40);
            }
        }

        public void LoadGame()
        {
            if (selectedSave == "")
            {
                return;
            }
            
            GameStateManager.Instance.LoadFromDisk(selectedSave);
            GameStateManager.Instance.LoadMission();
            SceneManager.LoadScene(GameStateManager.Instance.gameState.playerData.sceneName);
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(Constants.SceneNames.mainMenu, LoadSceneMode.Single);
        }
    }

}