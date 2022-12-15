using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Newtonsoft.Json.JsonSerializer;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager GSM { get; private set; }
    
    public GameState gameState = new();
    
    private string _persistentPath = "";
    private string _openSave = "";
    
    private void Awake()
    {
        if (GSM != null && GSM != this)
        {
            Destroy(this);
            return;
        }
        GSM = this;
        
        DontDestroyOnLoad(this);

        _persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }

    private void Start()
    {
        SceneManager.LoadScene(Constants.SceneNames.mainMenu, LoadSceneMode.Single);
    }

    public static void Save(GameState gameState)
    {
        GSM.gameState = gameState;
    }

    public static GameState Load()
    {
        return GSM.gameState;
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
        
        using StreamWriter writer = new StreamWriter(_persistentPath + "cw_save_" + dateTime + ".json");
        string json = JsonUtility.ToJson(gameState);

        writer.Write(json);

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
}
