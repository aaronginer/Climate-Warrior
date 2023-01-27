using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject startScreen;
    public Text scoreValue;
    public Text timeValue;

    public bool gameRunning = false;
    public int minScore;
    public float timeRemaining = 60;

    private int score;
    private GameEnd gameEnd;


    void Start()
    {
        startScreen.SetActive(true);
        gameRunning = false;
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
        gameEnd.ShowEndScreen(fell);

    }

    void Update()
    {
        if(!gameRunning && Input.GetKey(KeyCode.Space))
        {
            gameRunning = true;
            startScreen.SetActive(false);
            return;
        }
        if (!gameRunning || PauseScript.instance.gamePaused)
            return;
        timeRemaining -= Time.deltaTime;
        timeValue.text = Mathf.CeilToInt(timeRemaining).ToString();

        if (timeRemaining <= 0.0f)
            EndGame(false);
    }  
}
