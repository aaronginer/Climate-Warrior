using System;
using UnityEngine;

[Serializable]
public class GameState
{
    public PlayerData playerData;

    public GameState()
    {
        playerData = new PlayerData("default");
    }
    
    public void PrintGameState()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }
}
