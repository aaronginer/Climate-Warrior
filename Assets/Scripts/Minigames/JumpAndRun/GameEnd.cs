using Missions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    public string returnToMenu;
    public string restartGame;
    private Button leaveGame;
    private Button retryGame;
    private Button backToVillage;
    private TMPro.TextMeshProUGUI fellText;
    private TMPro.TextMeshProUGUI timeOutText;
    private TMPro.TextMeshProUGUI lostText;
    private TMPro.TextMeshProUGUI wonText;
    private TMPro.TextMeshProUGUI[] textFields;

    private void Awake()
    {
        leaveGame = GameObject.Find("MainMenu").GetComponent<Button>();
        retryGame = GameObject.Find("RestartLevel").GetComponent<Button>();
        backToVillage = GameObject.Find("BackToVillage").GetComponent<Button>();

        textFields = GetComponentsInChildren<TMPro.TextMeshProUGUI>();

        fellText = GameObject.Find("FellOffText").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        timeOutText = GameObject.Find("TimeOutText").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        lostText = GameObject.Find("LostText").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        wonText = GameObject.Find("WonText").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        
        HideButtons();
        HideText();
    }

    void HideButtons()
    {
        leaveGame.gameObject.SetActive(false);
        retryGame.gameObject.SetActive(false);
        backToVillage.gameObject.SetActive(false);
    }

    void HideText()
    {
        fellText.gameObject.SetActive(false);
        timeOutText.gameObject.SetActive(false);
        lostText.gameObject.SetActive(false);
        wonText.gameObject.SetActive(false);
    }

    public void ShowButtonsLost()
    {
        leaveGame.gameObject.SetActive(true);
        retryGame.gameObject.SetActive(true);
    }

    public void BackToMenu()
    {
        if (ScoreBoard.instance.GetScore() >= ScoreBoard.instance.minScore)
        {
            GameStateManager.Instance.gameState.playerData.CompleteMiniGame(MiniGame.jumpAndRunCollectTurbineParts);
        }
        SceneManager.LoadScene(returnToMenu);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(restartGame);
    }

    public void Fell()
    {
        fellText.gameObject.SetActive(true);
    }
    
    public void Won()
    {
        wonText.gameObject.SetActive(true);
        backToVillage.gameObject.SetActive(true);
        GameStateManager.Instance.CurrentMission.State.stateID =
            (int)MissionWindTurbine.States.AfterJumpAndRunCompletedGoBackToMayor;
    }

    public void Lost()
    {
        lostText.gameObject.SetActive(true);
    }

    public void TimeOut()
    {
        timeOutText.gameObject.SetActive(true);
    }

    public void ShowEndScreen(bool fell)
    {
        ScoreBoard.instance.running = false;
        ScoreBoard.instance.timerRunning = false;

        if (ScoreBoard.instance.GetScore() >= ScoreBoard.instance.minScore)
        {
            this.Won();
        }
        else if(ScoreBoard.instance.timeRemaining <= 0f)
        {
            this.TimeOut();
            ShowButtonsLost();
        }
        else if(fell)
        {
            this.Fell();
            ShowButtonsLost();
        }
        else
        {
            this.Lost();
            ShowButtonsLost();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
