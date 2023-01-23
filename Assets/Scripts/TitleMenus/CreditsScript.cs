using UnityEngine;
using UnityEngine.SceneManagement;

namespace TitleMenus
{
    public class CreditsScript : MonoBehaviour
    {
        public void BackToMainMenu()
        {
            SceneManager.LoadScene(Constants.SceneNames.mainMenu, LoadSceneMode.Single);
        }
    }

}
