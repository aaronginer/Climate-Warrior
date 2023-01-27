using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    public string returnToMenu;
    public string restartGame;

    public GameObject endViewContainer;
    public TMPro.TextMeshProUGUI endViewText;
    private Button leaveGame;
    private Button retryGame;
    private Button backToVillage;

    /*private TMPro.TextMeshProUGUI fellText;
    private TMPro.TextMeshProUGUI timeOutText;
    private TMPro.TextMeshProUGUI lostText;
    private TMPro.TextMeshProUGUI wonText;*/

    public string fellText;
    public string timeOutText;
    public string lostText;
    public string wonText;


    private GameManager manager;


    private void Awake()
    {
        manager = GameObject.Find("Overlay").GetComponent<GameManager>();


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

    public void BackToMenu()
    {
        SceneManager.LoadScene(returnToMenu);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(restartGame);
    }

    public void Fell()
    {
        endViewText.text = fellText;
    }
    
    public void Won()
    {
        endViewText.text = wonText;
        backToVillage.gameObject.SetActive(true);
        if (manager.GetScore() >= manager.minScore)
        {
            GameStateManager.Instance.gameState.playerData.CompleteMiniGame(MiniGame.jumpAndRunCollectTurbineParts);
        }
    }

    public void Lost()
    {
        endViewText.text = lostText;
    }

    public void TimeOut()
    {
        endViewText.text = timeOutText;
    }

    public void ShowEndScreen(bool fell)
    {
        endViewContainer.SetActive(true);
        if(manager.timeRemaining <= 0f)
        {
            this.TimeOut();
            ShowButtonsLost();
        }
        else if(fell)
        {
            this.Fell();
            ShowButtonsLost();
        }
        else if (manager.GetScore() >= manager.minScore)
        {
            this.Won();
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
