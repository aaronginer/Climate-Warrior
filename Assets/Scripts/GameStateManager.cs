using System;
using System.IO;
using Dialogue;
using InventorySystem;
using Missions;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    
    public GameState gameState;

    public DialogueDisplay dialogueDisplay;
    public InventoryDisplay inventoryDisplay;
    public CurrentTaskDisplay currentTaskDisplay;
    public MusicScript mainMusicScript;
    
    public BaseMission BaseMission;
    public Mission CurrentMission;
    public float missionTimer;
    public bool missionTimerActive;
    
    public string persistentPath = "";
    private string _openSave = "";
    // private TextMeshProUGUI currentTaskTextBox;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Destroy(this);
            return;
        }
        
        
        Instance = this;
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;

        Init();

        DontDestroyOnLoad(this);
        
        // register callback
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Init()
    {
        BaseMission = new BaseMission();
        gameState = new GameState()
        {
            baseMissionState = BaseMission.State
        };
    }
    
    public bool CheckIfMissionAndState(string name, int missionState)
    {
        if (CurrentMission == null)
        {
            return false;
        }
        return CurrentMission.Name == name && CurrentMission.State.stateID == missionState;
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

    public void SaveToDisk()
    {
        if (gameState == null)
        {
            Debug.Log("No game state available to save.");
            return;
        }

        var dateTime = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
        string path = persistentPath + "save_" + Instance.gameState.playerData.name + "_" + dateTime + ".json";  
        
        using StreamWriter writer = new StreamWriter(path);
        string json = JsonUtility.ToJson(gameState);

        writer.Write(json);
        writer.Close();

        if (_openSave != "") // if a save is open, delete it and create new save
        {
            File.Delete(_openSave);
        }
        
        _openSave = path;

        Debug.Log("Game state saved.");
    }

    public void LoadFromDisk(string saveFile)
    {
        string savePath = persistentPath + "save_" + saveFile + ".json";

        if (!File.Exists(savePath))
        {
            Debug.Log(savePath + " not found.");
            return;
        }
        
        _openSave = savePath;
            
        using StreamReader reader = new StreamReader(savePath);
        string json = reader.ReadToEnd();
        reader.Close();
        gameState = JsonUtility.FromJson<GameState>(json);

        Debug.Log("Game state loaded.");
    }

    public void StartMission(Mission mission)
    {
        // destroy MayorDialogue if there is one there currently (used for mission manager)
        Destroy(GameObject.Find("StartMayorDialogue"));
        
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
        ShowCurrentTask();
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

    public void ShowCurrentTask()
    {
        if (currentTaskDisplay != null)
        {
            currentTaskDisplay.ShowCurrentTask();
        }
    }

    public void SetCurrentTaskActive(bool active)
    {
        if (currentTaskDisplay == null) return;
        if (active)
        {
            currentTaskDisplay.Enable();
        }
        else
        {
            currentTaskDisplay.Disable();
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BaseMission?.Setup();
        CurrentMission?.Setup();
        Cursor.visible = true;
        mainMusicScript.Play(scene.name);
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
