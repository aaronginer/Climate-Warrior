using System;
using Missions;
using UnityEngine;

[Serializable]
public class GameState
{
    public PlayerData playerData;
    public MissionState missionState;

    public GameState()
    {
        playerData = new PlayerData("default");
        missionState = null;
    }
    
    public void PrintGameState()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }
}
