using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HighScore
{
    public class HighScoresManager : MonoBehaviour
    {
        public GameObject optionsList;
        public GameObject highScoresContainer;
        public TextMeshProUGUI highScoresTitleText;
        public TextMeshProUGUI noHighScoresText;
        public List<TextMeshProUGUI> highScoreText;
        public List<TextMeshProUGUI> namesText;

        private bool _scoresView;

        private void Start()
        {
            optionsList.SetActive(true);
            highScoresContainer.SetActive(false);
        }

        public void DisplayHighScores(string mission)
        {
            HighScores highScores = GetHighScoresFromFile(mission);

            if (highScores.highScoreList.Count == 0)
            {
                noHighScoresText.text = "No HighScores Yet";
            }
            else
            {
                for (int i = 0; i < (highScores.highScoreList.Count > 9 ? 9 : highScores.highScoreList.Count); i++)
                {
                    namesText[i].text = i + 1 + "   " + highScores.highScoreList[i].playerName;
                    highScoreText[i].text = "" + highScores.highScoreList[i].score;
                }
            }

            highScoresTitleText.text = mission;          
            
            _scoresView = true;
            optionsList.SetActive(false);
            highScoresContainer.SetActive(true);
        }
        
        private static HighScores GetHighScoresFromFile(string mission)
        {
            mission = mission.Replace(" ", "").ToLower();
            string path = GameStateManager.Instance.persistentPath + "highscores_" + mission + ".json";

            if (!File.Exists(path))
            {
                return new HighScores();
            }

            StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            reader.Close();
            HighScores highScores = JsonUtility.FromJson<HighScores>(json);
            highScores.Sort();
            
            return highScores;
        }

        private static void SaveHighScoresToFile(HighScores highScores, string mission)
        {
            mission = mission.Replace(" ", "").ToLower();
            string path = GameStateManager.Instance.persistentPath + "highscores_" + mission + ".json";

            if (File.Exists(path))
            {
                if (File.Exists(path+".old"))
                {
                    File.Delete(path+".old");
                }
                File.Move(path, path+".old");
            }

            using StreamWriter writer = new StreamWriter(path);
            string json = JsonUtility.ToJson(highScores);
            Debug.Log(json);

            writer.Write(json);
            writer.Close();

            Debug.Log("HighScore updated");
        }

        public static void SaveHighScore(string playerName, int score, string mission)
        {
            HighScores highScores = GetHighScoresFromFile(mission);
            highScores.AddHighScore(playerName, score);
            SaveHighScoresToFile(highScores, mission);
        }
        
        public void BackButton()
        {
            if (_scoresView)
            {
                noHighScoresText.text = "";
                for (int i = 0; i < 9; i++)
                {
                    highScoreText[i].text = "";
                    namesText[i].text = "";
                }
                
                optionsList.SetActive(true);
                highScoresContainer.SetActive(false);
                _scoresView = false;
            }
            else
            {
                SceneManager.LoadScene(Constants.SceneNames.mainMenu, LoadSceneMode.Single);
            }
        }
    }
}