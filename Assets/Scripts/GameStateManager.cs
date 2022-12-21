using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Missions;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Newtonsoft.Json.JsonSerializer;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    
    public GameState gameState;

    public Mission CurrentMission;
    
    private string _persistentPath = "";
    private string _openSave = "";
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        gameState = new GameState();
        CurrentMission = new MissionSabotage("sab");
        
        Instance = this;
        
        DontDestroyOnLoad(this);

        _persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
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

        var dateTime = DateTime.Now.ToString("dd_MM_yyyy_HH_MM_ss");
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

    public bool StartMission(Mission mission)
    {
        if (CurrentMission != null) return false;

        CurrentMission = mission;
        gameState.missionState = mission.State;

        return true;
    }

    public void EndMission()
    {
        CurrentMission = null;
    }
}
