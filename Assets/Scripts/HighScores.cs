using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class HighScores : MonoBehaviour
{
    public List<TextMeshProUGUI> highScoreText;

    private void Start()
    {
        throw new NotImplementedException();
    }

    private static List<String> GetHighScoresFromFile(string mission)
    {
        string path = GameStateManager.Instance.persistentPath + "highscores_" + mission;

        if (!File.Exists(path))
        {
            return new List<string>();
        }

        StreamReader reader = new StreamReader(path);

        return new List<string>(reader.ReadToEnd().Split("\n"));
    }

    public static void SaveHighScoresToFile(List<String> scores, string mission)
    {
        string path = GameStateManager.Instance.persistentPath + "highscores_" + mission;

        bool fileExists = File.Exists(path);

        StreamWriter writer = new StreamWriter(path);
        foreach (var line in scores)
        {
            writer.WriteLine(line);
        }
        
        
    }
}