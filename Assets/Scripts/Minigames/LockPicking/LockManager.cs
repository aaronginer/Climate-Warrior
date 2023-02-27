using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Missions;

public class LockManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject pin1;
    public GameObject pin2;
    public GameObject pin3;

    public string lostText;
    public string wonText;

    public float maxSpeed;
    public float minSpeed;

    public int allowedMistakes = 3;

    public Text triesText;

    private GameObject[] winningCondition = new GameObject[3];

    private enum GameState {Won, Lost, Unfinished}
    private GameState state;

    private GameEnd gameEnd;
    

    void Start()
    {
        winningCondition[0] = pin1;
        winningCondition[1] = pin2;
        winningCondition[2] = pin3;

        UpdateTries(allowedMistakes);
        SetUpGame();
        ShuffleArray();

        gameEnd = GameObject.Find("EndScreen").GetComponent<GameEnd>();
        PauseScript.instance.gamePaused = true;
        PauseScript.instance.ShowContols();

    }

    // Update is called once per frame
    void Update()
    {
        if (PauseScript.instance.gamePaused)
            return;
        switch(CheckWin())
        {
            case GameState.Lost:
                allowedMistakes--;
                if (allowedMistakes <= 0)
                {
                    UpdateTries(0);
                    ShowGameLost();
                    return;
                }
                UpdateTries(allowedMistakes);
                SetUpGame();
                break;
            case GameState.Won:
                ShowGameWon();
                break;
            default:
                break;
        }
   
    }

    private void SetUpGame()
    {
        RandomStart(pin1);
        RandomStart(pin2);
        RandomStart(pin3);
    }

    private void ShowGameLost()
    {
        gameEnd.DiplayEndView(lostText);
        gameEnd.ShowButtonsLost();
    }

    private void ShowGameWon()
    {
        gameEnd.DiplayEndView(wonText);
        gameEnd.ShowButtonWon();
        GameStateManager.Instance.CurrentMission.State.stateID =
            (int)MissionSolarPanel.States.FixingPipes;
    }

    private void ShuffleArray()
    {
        GameObject tmp;
        for (int i = 0; i < winningCondition.Length - 1; i++)
        {
            int rnd = Random.Range(i, winningCondition.Length);
            tmp = winningCondition[rnd];
            winningCondition[rnd] = winningCondition[i];
            winningCondition[i] = tmp;
        }
    }

    private void UpdateTries(int tries)
    {
        triesText.text = tries.ToString();
    }
    private void RandomStart(GameObject pin)
    {
        System.Random rand = new System.Random();
        float x = pin.transform.position.x;
        float z = pin.transform.position.z;
        float border = Camera.main.ScreenToWorldPoint(Vector3.zero).y + pin.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        float newPosition;
        do
        {
            newPosition = GetNextRandomFloat(border, border * (-1));
        } while (newPosition == 0.0f);
        
        float newSpeed = GetNextRandomFloat(minSpeed, maxSpeed);
        pin.GetComponent<PinMove>().speed = newSpeed;
        pin.GetComponent<PinMove>().movePin(new Vector3(x, newPosition, z));
        pin.GetComponent<PinMove>().positioned = false;
        pin.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    private float GetNextRandomFloat(float min, float max)
    {
        System.Random rand = new System.Random();
        return (float)(rand.NextDouble()*(max - min) + min);
    }

    private GameState CheckWin()
    {
        bool condition = false;
        for (int i = 0; i < winningCondition.Length; i++)
        {
            if (!condition && winningCondition[i].GetComponent<PinMove>().positioned && i != 0)
                return GameState.Lost;
            condition = winningCondition[i].GetComponent<PinMove>().positioned;
        }
        return condition ? GameState.Won : GameState.Unfinished;
    }
}
