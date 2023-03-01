using System;
using Catastrophes;
using Missions;
using UnityEngine;

[Serializable]
public class GameState
{
    public PlayerData playerData;
    public MissionState baseMissionState;
    public MissionState missionState;
    public CatastropheState catastropheState;
    public int score;
    public bool displayTurbine;

    public GameState()
    {
        playerData = new PlayerData("Warrior");
        baseMissionState = null;
        missionState = null;
        catastropheState = new CatastropheState();
    }
    
    public void PrintGameState()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }
}
