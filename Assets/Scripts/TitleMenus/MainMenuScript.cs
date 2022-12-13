using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TitleMenus
{
    public class MainMenuScript : MonoBehaviour
    {
        public void NewGame()
        {
            SceneManager.LoadScene("NewGame", LoadSceneMode.Single);
        }

        public void LoadGame()
        {
            SceneManager.LoadScene("LoadGame", LoadSceneMode.Single);
        }
    }   
}
