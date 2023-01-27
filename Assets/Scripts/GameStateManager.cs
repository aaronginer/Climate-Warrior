using System;
using System.IO;
using System.Security.Cryptography;
using Dialogue;
using Missions;
using TMPro;
using Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    
    public GameState gameState;

    public DialogueDisplay dialogueDisplay;
    
    public BaseMission BaseMission;
    public Mission CurrentMission;
    public float missionTimer;
    public bool missionTimerActive;
    
    private string _persistentPath = "";
    private string _openSave = "";
    // private TextMeshProUGUI currentTaskTextBox;

    private void Awake()
    {
        Debug.Log(Instance?.GetHashCode());
        Debug.Log(GetHashCode());
        
        if (Instance != null && Instance != this)
        {
            Debug.Log("destroyed");
            Destroy(gameObject);
            Destroy(this);
            return;
        }
        
        
        Instance = this;
        _persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;

        Debug.Log("reached");
        BaseMission = new BaseMission();
        gameState = new GameState
        {
            baseMissionState = BaseMission.State
        };

        DontDestroyOnLoad(this);
        
        // register callback
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public bool CheckIfMissionAndState(string name, int missionState)
    {
        if (CurrentMission == null)
        {
            return false;
        }
        return CurrentMission.name == name && CurrentMission.State.stateID == missionState;
    }

    private void Update()
    {
        CurrentMission?.UpdateTime();
        
        // mission timer
        if (!missionTimerActive) return;
        missionTimer -= Time.deltaTime;
        if (missionTimer > 0) return;
        missionTimerActive = false;
        CurrentMission?.AdvanceState();
    }

    private string GetCurrentTaskStringBasedOnState()
    {
        switch (BaseMission.State.stateID)
        {
            case (int) BaseMission.States.PrepareMission:
                return BaseMission.GetCurrentTaskBeforeMission();
            case (int) BaseMission.States.MissionActive:
                return CurrentMission.GetCurrentTask();
        }
        return "none";
    }
    
    public void UpdateCurrentTask()
    {
        TextMeshProUGUI currentTaskTextBox = GameObject.Find("CurrentTask")?.GetComponentInChildren<TextMeshProUGUI>();
        if (currentTaskTextBox == null)
        {
            return;
        }
        string currentTaskString = GetCurrentTaskStringBasedOnState();
        currentTaskTextBox.text = $"Current task:\n {currentTaskString}";
    }

    public void SaveToDisk()
    {
        if (gameState == null)
        {
            Debug.Log("No game state available to save.");
            return;
        }

        var dateTime = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
        string path = _persistentPath + "save_" + Instance.gameState.playerData.name + "_" + dateTime + ".json";  
        
        using StreamWriter writer = new StreamWriter(path);
        string json = JsonUtility.ToJson(gameState);

        writer.Write(json);
        
        gameState.playerData.inventory.CleanInventory();

        if (_openSave != "") // if a save is open, delete it and create new save
        {
            File.Delete(_openSave);
        }
        
        _openSave = path;

        Debug.Log("Game state saved.");
    }

    public void LoadFromDisk(string saveFile)
    {
        string savePath = _persistentPath + "save_" + saveFile + ".json";

        if (!File.Exists(savePath))
        {
            Debug.Log(savePath + " not found.");
            return;
        }
        
        _openSave = savePath;
            
        using StreamReader reader = new StreamReader(savePath);
        string json = reader.ReadToEnd();
        gameState = JsonUtility.FromJson<GameState>(json);

        Debug.Log("Game state loaded.");
    }

    public void StartMission(Mission mission)
    {
        if (CurrentMission != null) return;

        CurrentMission = mission;
        gameState.missionState = mission.State;
        
        mission.AdvanceState();
    }

    public void LoadBaseMission()
    {
        var m = new BaseMission
        {
            State = gameState.baseMissionState
        };
        BaseMission = m;
    }
    
    public void LoadMission()
    {
        var m = Mission.LoadMission(gameState.missionState.missionName);
        if (m == null)
        {
            return;
        }
        m.State = gameState.missionState;
        CurrentMission = m;
    }
    
    public void EndMission()
    {
        CurrentMission = null;
        gameState.missionState = null;
    }

    public void SetMissionAdvanceTimer(float time)
    {
        missionTimer = time;
        missionTimerActive = true;
    }

    public SoundsScript GetSoundScript()
    {
        return GetComponent<SoundsScript>();
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SceneLoaded: " + GetHashCode());
        BaseMission?.Setup();
        CurrentMission?.Setup();
        Cursor.visible = true;
        UpdateCurrentTask();
    }

    public static void Destroy()
    {
        Instance.enabled = false;
        Destroy(Instance.gameObject);
        Instance = null;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
