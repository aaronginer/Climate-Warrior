using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurbineManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject startScreen;


    public bool gameRunning = false;
    public int minScore;
    public float timeRemaining = 60;

    private int score;
    private GameTurbineEnd gameEnd;


    void Start()
    {
        startScreen.SetActive(true);
        gameRunning = false;
        gameEnd = GameObject.Find("EndScreen").GetComponent<GameTurbineEnd>();
    }

    // Update is called once per frame



    public void EndGame(bool fell)
    {
        gameRunning = false;
    }

    void Update()
    {
        if (!gameRunning && Input.GetKey(KeyCode.Space))
        {
            gameRunning = true;
            startScreen.SetActive(false);
            return;
        }
        if (!gameRunning || PauseScript.instance.gamePaused)
            return;
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0.0f)
            EndGame(false);
    }
}
