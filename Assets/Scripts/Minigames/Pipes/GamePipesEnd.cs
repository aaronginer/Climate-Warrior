using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePipesEnd : MonoBehaviour
{
    private TMPro.TextMeshProUGUI timeOutText;
    private TMPro.TextMeshProUGUI lostText;
    private TMPro.TextMeshProUGUI wonText;
    
    private Button backToButton;
    private Button restartLevelButton;
    private Button wonLevelButton;

    private void Awake()
    {
        backToButton = GameObject.Find("BackToButton").GetComponent<Button>();
        backToButton.onClick.AddListener(BackToButtonClick);
        restartLevelButton = GameObject.Find("RestartLevelButton").GetComponent<Button>();
        restartLevelButton.onClick.AddListener(RestartGame);
        wonLevelButton = GameObject.Find("WonButton").GetComponent<Button>();
        wonLevelButton.onClick.AddListener(BackToButtonClick);

        timeOutText = GameObject.Find("TimeOutText").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        lostText = GameObject.Find("LostText").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        wonText = GameObject.Find("WinningText").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        
        HideButtons();
        HideText();
    }
    
    

    void HideButtons()
    {
        backToButton.gameObject.SetActive(false);
        restartLevelButton.gameObject.SetActive(false);
        wonLevelButton.gameObject.SetActive(false);
    }

    void HideText()
    {
        timeOutText.gameObject.SetActive(false);
        lostText.gameObject.SetActive(false);
        wonText.gameObject.SetActive(false);
    }
    public void BackToButtonClick()
    {
        SceneManager.LoadScene(Constants.SceneNames.village);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(Constants.SceneNames.pipes);
    }
    
    public void Won()
    {
        wonText.gameObject.SetActive(true);
        wonLevelButton.gameObject.SetActive(true);
        GameStateManager.Instance.gameState.playerData.CompleteMission(MiniGame.pipes);
    }

    public void Lost()
    {
        backToButton.gameObject.SetActive(true);
        restartLevelButton.gameObject.SetActive(true);
        lostText.gameObject.SetActive(true);
    }

    public void TimeOut()
    {
        backToButton.gameObject.SetActive(true);
        restartLevelButton.gameObject.SetActive(true);
        timeOutText.gameObject.SetActive(true);
    }
}
