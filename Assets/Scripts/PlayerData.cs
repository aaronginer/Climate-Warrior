using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public String name;
    public int health;

    public PlayerData(string name, int health)
    {
        this.name = name;
        this.health = health;
    }

    public void PrintGameState()
    {
        Debug.Log("PlayerData:");
        Debug.Log(" - name: " + name);
        Debug.Log(" - health: " + health);
    }
}