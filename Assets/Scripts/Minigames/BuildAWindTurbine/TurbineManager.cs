using UnityEngine;

public class TurbineManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject startScreen;
    public string wonText;
    public string lostText;
    public string timeOutText;

    public bool gameRunning = false;
    public int minScore;
    public float timeRemaining = 60;

    private int score;
    private GameEnd gameEnd;

    void Start()
    {
        startScreen.SetActive(true);
        gameRunning = false;
        PauseScript.instance.ShowContols();
        PauseScript.instance.gamePaused = true;
        gameEnd = GameObject.Find("EndScreen").GetComponent<GameEnd>();
    }

    // Update is called once per frame

    public void EndGame()
    {
        gameRunning = false;
    }

    void Update()
    {
        if (PauseScript.instance.gamePaused)
            return;
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0.0f)
            EndGame();
    }
}
