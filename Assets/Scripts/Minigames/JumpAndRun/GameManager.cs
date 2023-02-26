using UnityEngine;
using UnityEngine.UI;
using Missions;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Text scoreValue;
    public Text timeValue;

    public int minScore;
    public float timeRemaining = 60;

    public string wonText;
    public string lostText;
    public string fellText;
    public string timeOutText;

    public bool gameRunning = true;

    private int score;
    private GameEnd gameEnd;

    void Start()
    {
        PauseScript.instance.gamePaused = true;
        PauseScript.instance.ShowContols();
        
        gameEnd = GameObject.Find("EndScreen").GetComponent<GameEnd>();
    }

    // Update is called once per frame

    public void AddScore(int addScore)
    {
        score += addScore;
        scoreValue.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }

    public void EndGame(bool fell)
    {
        gameRunning = false;
        if (timeRemaining <= 0f)
        {
            gameEnd.DiplayEndView(timeOutText);
            gameEnd.ShowButtonsLost();
        }
        else if (fell)
        {
            gameEnd.DiplayEndView(fellText);
            gameEnd.ShowButtonsLost();
        }
        else if (GetScore() >= minScore)
        {
            gameEnd.DiplayEndView(wonText);
            gameEnd.ShowButtonWon();
            this.Won();
        }
        else
        {
            gameEnd.DiplayEndView(lostText);
            gameEnd.ShowButtonsLost();
        }
    }

    public void Won()
    {

        if (GetScore() >= minScore)
        {
            GameStateManager.Instance.CurrentMission.State.stateID =
                (int)MissionWindTurbine.States.AfterJumpAndRunCompletedGoBackToMayor;
            GameStateManager.Instance.gameState.playerData.CompleteMiniGame(MiniGame.jumpAndRunCollectTurbineParts);
        }
    }


    void Update()
    {
        if (PauseScript.instance.gamePaused)
            return;
        if (!gameRunning)
            return;
        timeRemaining -= Time.deltaTime;
        timeValue.text = Mathf.CeilToInt(timeRemaining).ToString();

        if (timeRemaining <= 0.0f)
            EndGame(false);
    }  
}
