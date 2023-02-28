using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HighScore
{
    [Serializable]
    public class HighScores
    {
        public List<HighScorePair> highScoreList = new ();

        public void AddHighScore(string playerName, int score)
        {
            highScoreList.Add(new HighScorePair(playerName, score));
            
            Sort();
        }

        public void Sort()
        {
            var list = highScoreList.OrderBy(x => x.score).ToList();
            list.Reverse();
            highScoreList = list;
        }
    }

    [Serializable]
    public class HighScorePair
    {
        public string playerName;
        public int score;

        public HighScorePair(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }
    }
}