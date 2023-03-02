using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TitleMenus
{
    public class MainMenuScript : MonoBehaviour
    {
        private void Start()
        {
            Time.timeScale = 1.0f;
        }

        public void NewGame()
        {
            SceneManager.LoadScene(Constants.SceneNames.newGame, LoadSceneMode.Single);
        }

        public void LoadGame()
        {
            SceneManager.LoadScene(Constants.SceneNames.loadGame, LoadSceneMode.Single);
        }

        public void Leaderboard()
        {
            SceneManager.LoadScene("Leaderboard", LoadSceneMode.Single);
        }
        
        public void OpenCredits()
        {
            SceneManager.LoadScene(Constants.SceneNames.credits, LoadSceneMode.Single);
        }
    }   
}
