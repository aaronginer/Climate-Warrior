using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipesGame : MonoBehaviour
{
    // Start is called before the first frame update

    private List<GameObject> pipeObjects;
    private GamePipesEnd gameEnd;
    
    public bool timerRunning = true;
    private float timeRemaining = 60.0f;
    private TMPro.TextMeshProUGUI timeText;

    
    void Start()
    {
        pipeObjects = CollectPipeObjects();
        gameEnd = GameObject.Find("EndScreen").GetComponent<GamePipesEnd>();
        timeText = GameObject.Find("TimeValue").GetComponent<TMPro.TextMeshProUGUI>();
        timeText.text = timeRemaining.ToString();
    }
    
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
                gameEnd.Lost();
            }
        }
    }

    List<GameObject> CollectPipeObjects()
    {
        GameObject pipesWrapper = GameObject.Find("Pipes");
        List<GameObject> pipes = new List<GameObject>();
        foreach (Transform rowTransform in pipesWrapper.GetComponentsInChildren<Transform>())
        {
            if (rowTransform.parent != pipesWrapper.transform) continue;
            GameObject row = rowTransform.gameObject;
            foreach (Transform pipeTransform in row.GetComponentsInChildren<Transform>())
            {
                if (pipeTransform.parent != rowTransform) continue;
                GameObject pipe = pipeTransform.gameObject;
                pipes.Add(pipe);
            }
        }
        return pipes;
    }

    private bool iterateCheckGameWin()
    {
        bool didWin = true;
        foreach (var pipeObject in pipeObjects)
        {
            PipeScript pipeScript = pipeObject.GetComponent<PipeScript>();
            if (!pipeScript.isOriginalRotation())
            {
                return false;
            }
        }
        return true;
    }

    public void checkGameWin()
    {
        bool didWin = iterateCheckGameWin();
        if (didWin)
        {
            timerRunning = false;
            gameEnd.Won();
        }
    }
}
