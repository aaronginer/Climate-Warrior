using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public static ScoreBoard instance;
    public bool timerRunning = true;
    public int minScore = 0;

    public float timeRemaining;
    public bool running = true;
    private int score;
    private PlayerMove player;
    private TMPro.TextMeshProUGUI timeText;
    private TMPro.TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            player = GameObject.Find("Player").GetComponent<PlayerMove>();
            timeText = GameObject.Find("TimeValue").GetComponent<TMPro.TextMeshProUGUI>();
            scoreText = GameObject.Find("ScoreValue").GetComponent<TMPro.TextMeshProUGUI>();
            timeText.text = timeRemaining.ToString();
            scoreText.text = "0";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;
            timeText.text = Mathf.CeilToInt(timeRemaining).ToString();
            if (timeRemaining <= 0)
            {
                timeText.text = "0";
                timerRunning = false;
                Destroy(player.gameObject);
            }
        }
    }

    public void UpdateScore(int coinScore)
    {
        this.score += coinScore;
        scoreText.text = (this.score).ToString();
    }

    public int GetScore()
    {
        return this.score;
    }
}
