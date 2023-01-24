using System;
using Missions;
using UnityEngine;

[Serializable]
public class GameState
{
    public PlayerData playerData;
    public MissionState baseMissionState;
    public int score = 0;

    public GameState()
    {
        playerData = new PlayerData("default");
        baseMissionState = null;
    }
    
    public void PrintGameState()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }
}
