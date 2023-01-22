using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentCanvasScript : MonoBehaviour
{
    public static PersistentCanvasScript Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void SpawnPersistentCanvas()
    {
        var obj = Instantiate(Resources.Load("PersistentCanvas"));
        obj.name = "PersistentCanvas";
    }

    public static void DestroyPersistentCanvas()
    {
        Destroy(Instance.gameObject);
    }
}
