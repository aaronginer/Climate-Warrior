using System;
using UnityEngine;

[Serializable]
public class GameState
{
    public PlayerData playerData;
    public bool valid = true;

    public GameState(string playerName, int health)
    {
        playerData = new PlayerData(playerName, health);
    }

    public void PrintGameState()
    {
        playerData.PrintGameState();
        Debug.Log("Valid: " + valid);
    }
}
