using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Newtonsoft.Json.JsonSerializer;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager GSM { get; private set; }
    
    private string _tmpPath = "";
    private string _persistentPath = "";
    public GameState gameState = null;

    private void Awake()
    {
        if (GSM != null && GSM != this)
        {
            Destroy(this);
            return;
        }
        GSM = this;
        
        DontDestroyOnLoad(this);

        _tmpPath = Application.dataPath + Path.AltDirectorySeparatorChar + "GameState.json";
        _persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "GameState.json";

        gameState = new GameState("Aaron", 6);
    }

    private void Start()
    {
        SceneManager.LoadScene("Scene1", LoadSceneMode.Single);
    }

    public static void Save(GameState gameState)
    {
        GSM.gameState = gameState;
    }

    public static GameState Load()
    {
        return GSM.gameState;
    }
    
    public static void SaveToDisk()
    {
        if (GSM.gameState == null)
        {
            Debug.Log("No game state available to save.");
            return;
        }

        using StreamWriter writer = new StreamWriter(GSM._tmpPath);
        string json = JsonUtility.ToJson(GSM.gameState);
        writer.Write(json);
        
        Debug.Log("Game state saved.");
        SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
    }

    public static void LoadFromDisk()
    {
        if (!File.Exists(GSM._tmpPath))
        {
            Debug.Log("GameState.json not found.");
            return;
        }
        
        using StreamReader reader = new StreamReader(GSM._tmpPath);
        string json = reader.ReadToEnd();
        GSM.gameState = JsonUtility.FromJson<GameState>(json);
        if (!GSM.gameState.valid)
        {
            Debug.Log("Loading game state failed, game state invalid.");
            return;
        }
        
        Debug.Log("Game state loaded.");
        GSM.gameState.PrintGameState();
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }
}
