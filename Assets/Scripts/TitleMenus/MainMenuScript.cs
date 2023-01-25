using UnityEngine;
using UnityEngine.SceneManagement;

namespace TitleMenus
{
    public class MainMenuScript : MonoBehaviour
    {
        public void NewGame()
        {
            SceneManager.LoadScene(Constants.SceneNames.newGame, LoadSceneMode.Single);
        }

        public void LoadGame()
        {
            SceneManager.LoadScene(Constants.SceneNames.loadGame, LoadSceneMode.Single);
        }

        public void OpenCredits()
        {
            SceneManager.LoadScene("Credits", LoadSceneMode.Single);
        }
    }   
}
