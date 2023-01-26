using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseScript : MonoBehaviour
{
    // Start is called before the first frame update
    public PauseScript instance;

    public string backTo;
    public bool gamePaused = false;
    private Button resumeGame;
    private Button backToVillage;
    private TMPro.TextMeshProUGUI taskText;
    private TMPro.TextMeshProUGUI leftControlText;
    private TMPro.TextMeshProUGUI rightControlText;
    private TMPro.TextMeshProUGUI jumpControlText;

    void Start()
    {
        if(instance == null)
        {
            resumeGame = GameObject.Find("ResumeGame").GetComponent<Button>();
            backToVillage = GameObject.Find("PauseBackToVillage").GetComponent<Button>();

            taskText = GameObject.Find("PauseTask").GetComponentInChildren<TMPro.TextMeshProUGUI>();
            leftControlText = GameObject.Find("PauseControllesLeft").GetComponentInChildren<TMPro.TextMeshProUGUI>();
            rightControlText = GameObject.Find("PauseControllesRight").GetComponentInChildren<TMPro.TextMeshProUGUI>();
            jumpControlText = GameObject.Find("PauseControllesJump").GetComponentInChildren<TMPro.TextMeshProUGUI>();

            instance = this;
            HideButtons();
            HideText();
        }
    }

    void HideButtons()
    {
        resumeGame.gameObject.SetActive(false);
        backToVillage.gameObject.SetActive(false);
    }

    void HideText()
    {
        taskText.gameObject.SetActive(false);
        leftControlText.gameObject.SetActive(false);
        rightControlText.gameObject.SetActive(false);
        jumpControlText.gameObject.SetActive(false);
    }

    void ShowButtons()
    {
        resumeGame.gameObject.SetActive(true);
        backToVillage.gameObject.SetActive(true);
    }

    void ShowText()
    {
        taskText.gameObject.SetActive(true);
        leftControlText.gameObject.SetActive(true);
        rightControlText.gameObject.SetActive(true);
        jumpControlText.gameObject.SetActive(true);
    }
    
    public void returnToVillage()
    {
        SceneManager.LoadScene(backTo);
    }

    public void PauseGame()
    {
        ShowButtons();
        ShowText();
        ScoreBoard.instance.timerRunning = false;
        gamePaused = true;
    }

    public void UnPauseGame()
    {
        HideButtons();
        HideText();
        ScoreBoard.instance.timerRunning = true;
        gamePaused = false;
    }

    public void TogglePause()
    {
        if (gamePaused)
            UnPauseGame();
        else
            PauseGame();
    }

    // Update is called once per frame
    void Update()
    {
            
    }
}
