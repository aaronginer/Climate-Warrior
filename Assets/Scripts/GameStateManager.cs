using System;
using System.IO;
using Dialogue;
using Missions;
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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        // register callback
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        Instance = this;
        _persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;

        BaseMission = new BaseMission();
        gameState = new GameState
        {
            baseMissionState = BaseMission.State
        };

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (!missionTimerActive) return;
        
        missionTimer -= Time.deltaTime;

        if (missionTimer > 0) return;
            
        missionTimerActive = false;
        
        CurrentMission?.AdvanceState();
    }

    public void SaveToDisk()
    {
        if (gameState == null)
        {
            Debug.Log("No game state available to save.");
            return;
        }

        if (_openSave != "") // if a save is open, delete it and create new save
        {
            File.Delete(_openSave);
        }

        var dateTime = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
        string path = _persistentPath + "cw_save_" + dateTime + ".json";  
        
        using StreamWriter writer = new StreamWriter(path);
        string json = JsonUtility.ToJson(gameState);

        writer.Write(json);
        
        gameState.playerData.inventory.CleanInventory();

        _openSave = path;

        Debug.Log("Game state saved.");
    }

    public void LoadFromDisk(string saveFile)
    {
        string savePath = _persistentPath + "cw_save_" + saveFile + ".json";

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
        
        mission?.AdvanceState();
    }

    public void LoadBaseMission()
    {
        var m = new BaseMission
        {
            State = gameState.baseMissionState
        };
        BaseMission = m;
        Debug.Log(BaseMission.GetHashCode());
    }
    
    public void EndMission()
    {
        CurrentMission = null;
    }

    public void SetMissionAdvanceTimer(float time)
    {
        missionTimer = time;
        missionTimerActive = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BaseMission?.Setup();
        CurrentMission?.Setup();
        Cursor.visible = true;
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
