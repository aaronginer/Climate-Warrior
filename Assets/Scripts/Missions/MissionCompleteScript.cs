using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Missions
{
    public class MissionCompleteScript : MonoBehaviour
    {
        public GameObject scoreMaxText;
        public GameObject deductionsTimeText;
        public GameObject deductionsCatastropheText;
        public GameObject deductionsDecisionsText;
        
        public GameObject scoreMaxScore;
        public GameObject deductionsTimeScore;
        public GameObject deductionsCatastropheScore;
        public GameObject deductionsDecisionsScore;
        public GameObject scoreFinalScore;

        void Start()
        {
            StartCoroutine(ShowMissionResults());
        }

        IEnumerator ShowMissionResults()
        {
            scoreMaxText.SetActive(true);
            scoreMaxScore.SetActive(true);
            scoreMaxScore.GetComponent<AnimateCounterScript>().StartAnimate(500);

            yield return new WaitForSeconds(1.5f);
            
            deductionsTimeText.SetActive(true);
            deductionsTimeScore.SetActive(true);
            deductionsTimeScore.GetComponent<AnimateCounterScript>().StartAnimate(-300);

            yield return new WaitForSeconds(1.5f);
            
            deductionsCatastropheText.SetActive(true);
            deductionsCatastropheScore.SetActive(true);
            deductionsCatastropheScore.GetComponent<AnimateCounterScript>().StartAnimate(500);

            yield return new WaitForSeconds(1.5f);
            
            deductionsDecisionsText.SetActive(true);
            deductionsDecisionsScore.SetActive(true);
            deductionsDecisionsScore.GetComponent<AnimateCounterScript>().StartAnimate(500);

            yield return new WaitForSeconds(1.5f);
            
            scoreFinalScore.SetActive(true);
            scoreFinalScore.GetComponent<AnimateCounterScript>().StartAnimate(500);

            yield return new WaitForSeconds(1.5f);
        }

        // Spawns a mission complete window. Use when mission is completed. Stat calculations happen automatically.
        public static void MissionComplete()
        {
            var missionCompletePrefab = Resources.Load("Missions/MissionComplete") as GameObject;
            Instantiate(missionCompletePrefab, GameObject.Find("Canvas").transform);
        }
    }

}
