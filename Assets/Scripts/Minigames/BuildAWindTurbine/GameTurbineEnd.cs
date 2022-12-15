using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTurbineEnd : MonoBehaviour
{
    private Button[] buttons;
    private TMPro.TextMeshProUGUI timeOutText;
    private TMPro.TextMeshProUGUI lostText;
    private TMPro.TextMeshProUGUI wonText;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        
        Button backToButton = GameObject.Find("BackToButton").GetComponent<Button>();
        backToButton.onClick.AddListener(BackToButtonClick);
        Button restartLevelButton = GameObject.Find("RestartLevelButton").GetComponent<Button>();
        restartLevelButton.onClick.AddListener(RestartGame);

        timeOutText = GameObject.Find("TimeOutText").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        lostText = GameObject.Find("LostText").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        wonText = GameObject.Find("WonText").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        wonText = GameObject.Find("WonText").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        wonText = GameObject.Find("WonText").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        
        HideButtons();
        HideText();
    }
    
    

    void HideButtons()
    {
        foreach(var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    void HideText()
    {
        timeOutText.gameObject.SetActive(false);
        lostText.gameObject.SetActive(false);
        wonText.gameObject.SetActive(false);
    }

    public void ShowButtons()
    {
        foreach(var button in buttons)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void BackToButtonClick()
    {
        SceneManager.LoadScene(Constants.SceneNames.village);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(Constants.SceneNames.miniGameBuildAWindTurbine);
    }
    
    public void Won()
    {
        ShowButtons();
        GameStateManager.GSM.gameState.playerData.completeMission(MiniGame.buildAWindTurbine);
        wonText.gameObject.SetActive(true);
    }

    public void Lost()
    {
        ShowButtons();
        lostText.gameObject.SetActive(true);
    }

    public void TimeOut()
    {
        ShowButtons();
        timeOutText.gameObject.SetActive(true);
    }
}
