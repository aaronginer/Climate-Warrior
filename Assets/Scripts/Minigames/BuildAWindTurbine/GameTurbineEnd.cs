using Items;
using Missions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTurbineEnd : MonoBehaviour
{   

    private TMPro.TextMeshProUGUI timeOutText;
    private TMPro.TextMeshProUGUI lostText;
    private TMPro.TextMeshProUGUI wonText;


    public GameObject endViewContainer;
    public TMPro.TextMeshProUGUI endViewText;
    public string timeOutTest;
    public string lostTest;
    public string wonTest;


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
        //timeOutText = GameObject.Find("TimeOutText").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        //lostText = GameObject.Find("LostText").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        //wonText = GameObject.Find("WinningText").GetComponentInChildren<TMPro.TextMeshProUGUI>();

        endViewContainer.SetActive(false);
        HideButtons();
        //HideText();
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
        if (GameStateManager.Instance.gameState.playerData.CheckMiniGameCompleted(MiniGame.buildAWindTurbine))
        {
            GameStateManager.Instance.gameState.playerData.inventory.AddItem(ItemType.Turbine);
        }
        SceneManager.LoadScene(Constants.SceneNames.village);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(Constants.SceneNames.miniGameBuildAWindTurbine);
    }
    
    public void Won()
    {
        //wonText.gameObject.SetActive(true);
        //wonLevelButton.gameObject.SetActive(true);
        endViewContainer.SetActive(true);
        endViewText.text = wonTest;
        wonLevelButton.gameObject.SetActive(true);

        GameStateManager.Instance.gameState.playerData.CompleteMiniGame(MiniGame.buildAWindTurbine);
        GameStateManager.Instance.CurrentMission.State.stateID = (int)MissionWindTurbine.States.WindTurbineBuilt;
    }

    public void Lost()
    {
        endViewContainer.SetActive(true);
        endViewText.text = lostTest;
        // lostText.gameObject.SetActive(true);

        backToButton.gameObject.SetActive(true);
        restartLevelButton.gameObject.SetActive(true);
    }

    public void TimeOut()
    {
        endViewContainer.SetActive(true);
        endViewText.text = timeOutTest;
        //timeOutText.gameObject.SetActive(true);

        backToButton.gameObject.SetActive(true);
        restartLevelButton.gameObject.SetActive(true);
        
    }
}
