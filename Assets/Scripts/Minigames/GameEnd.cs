using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    // Start is called before the first frame update

    public string returnTo;
    public string restartGame;

    public GameObject endViewContainer;
    public TMPro.TextMeshProUGUI endViewText;

    private Button leaveGame;
    private Button retryGame;
    private Button backToVillage;

    void Awake()
    {
        leaveGame = GameObject.Find("MainMenu").GetComponent<Button>();
        retryGame = GameObject.Find("RestartLevel").GetComponent<Button>();
        backToVillage = GameObject.Find("BackToVillage").GetComponent<Button>();
        endViewContainer.SetActive(false);
        HideButtons();
    }


    void HideButtons()
    {
        leaveGame.gameObject.SetActive(false);
        retryGame.gameObject.SetActive(false);
        backToVillage.gameObject.SetActive(false);
    }


    public void ShowButtonsLost()
    {
        leaveGame.gameObject.SetActive(true);
        retryGame.gameObject.SetActive(true);
    }

    public void ShowButtonWon()
    {
        backToVillage.gameObject.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(returnTo);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(restartGame);
    }

    public void DiplayEndView(string displayText)
    {
        endViewText.text = displayText.Replace('\\', '\n');
        endViewContainer.SetActive(true);
    }
}
