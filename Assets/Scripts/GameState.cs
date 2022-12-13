using System;
using UnityEngine;

[Serializable]
public class GameState
{
    public PlayerData playerData;

    public void PrintGameState()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }
}
