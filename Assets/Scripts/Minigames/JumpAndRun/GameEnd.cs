using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    public string returnToMenu;
    public string restartGame;
    private Button[] buttons;
    private TMPro.TextMeshProUGUI fellText;
    private TMPro.TextMeshProUGUI timeOutText;
    private TMPro.TextMeshProUGUI lostText;
    private TMPro.TextMeshProUGUI wonText;
    private TMPro.TextMeshProUGUI[] textFields;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
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
        foreach(var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    void HideText()
    {
        fellText.gameObject.SetActive(false);
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

    public void BackToMenu()
    {
        if (ScoreBoard.instance.GetScore() >= ScoreBoard.instance.minScore)
        {
            GameStateManager.Instance.gameState.playerData.CompleteMission(MiniGame.jumpAndRunCollectTurbineParts);
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
        ShowButtons();

        if (ScoreBoard.instance.GetScore() >= ScoreBoard.instance.minScore)
        {
            this.Won();
        }
        else if(ScoreBoard.instance.timeRemaining <= 0f)
        {
            this.TimeOut();
        }
        else if(fell)
        {
            this.Fell();
        }
        else
        {
            this.Lost();
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
