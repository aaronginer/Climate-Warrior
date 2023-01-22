using System.Collections;
using Scoring;
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
        
        public GameObject continueButton;

        private int _finalScore;
        
        private void Awake()
        {
            scoreMaxText.SetActive(false);
            deductionsTimeText.SetActive(false);
            deductionsCatastropheText.SetActive(false);
            deductionsDecisionsText.SetActive(false);
            
            scoreMaxScore.SetActive(false);
            deductionsTimeScore.SetActive(false);
            deductionsCatastropheScore.SetActive(false);
            deductionsDecisionsScore.SetActive(false);
            scoreFinalScore.SetActive(false);
            
            continueButton.SetActive(false);
        }

        void Start()
        {
            StartCoroutine(ShowMissionResults());
        }

        IEnumerator ShowMissionResults()
        {
            Mission currentMission = GameStateManager.Instance.CurrentMission;
            ClimateScoreManager csManager =
                currentMission.ClimateScoreObject.GetComponentInChildren<ClimateScoreManager>();
            
            Debug.Assert(currentMission != null);
            Debug.Assert(csManager != null);
            
            const int baseScore = 500;
            
            float missionMaxTime = currentMission.ClimateScoreMaxTime;
            float missionTimeLeft = csManager.secondsLeft;
            int timeScore = (int) (missionTimeLeft / missionMaxTime * 1000);
            
            int deductionsCatastrophe = csManager.catastropheHappened ? -250 : 0;
            
            int deductionsDecisions = 0;
            
            _finalScore = baseScore + timeScore + deductionsCatastrophe + deductionsDecisions;

            // update the global score
            GameStateManager.Instance.gameState.score += _finalScore;
            
            Destroy(currentMission.ClimateScoreObject);
            
            scoreMaxText.SetActive(true);
            scoreMaxScore.SetActive(true);
            
            scoreMaxScore.GetComponent<AnimateCounterScript>().StartAnimate(baseScore);

            yield return new WaitForSeconds(1.5f);
            
            deductionsTimeText.SetActive(true);
            deductionsTimeScore.SetActive(true);
            deductionsTimeScore.GetComponent<AnimateCounterScript>().StartAnimate(timeScore);

            yield return new WaitForSeconds(1.5f);
            
            deductionsCatastropheText.SetActive(true);
            deductionsCatastropheScore.SetActive(true);
            deductionsCatastropheScore.GetComponent<AnimateCounterScript>().StartAnimate(deductionsCatastrophe);

            yield return new WaitForSeconds(1.5f);
            
            deductionsDecisionsText.SetActive(true);
            deductionsDecisionsScore.SetActive(true);
            deductionsDecisionsScore.GetComponent<AnimateCounterScript>().StartAnimate(deductionsDecisions);

            yield return new WaitForSeconds(1.5f);
            
            scoreFinalScore.SetActive(true);
            scoreFinalScore.GetComponent<AnimateCounterScript>().StartAnimate(_finalScore);

            yield return new WaitForSeconds(1.5f);
            continueButton.SetActive(true);
        }

        public void CloseWindow()
        {

            GameObject.Find("Score").GetComponentInChildren<AnimateCounterScript>().StartAnimate(_finalScore);
            
            GameStateManager.Instance.EndMission();
            Destroy(gameObject);
        }
        
        // Spawns a mission complete window. Use when mission is completed. Stat calculations happen automatically.
        public static void MissionComplete()
        {
            var missionCompletePrefab = Resources.Load("Missions/MissionComplete") as GameObject;
            Instantiate(missionCompletePrefab, GameObject.Find("Canvas").transform);
        }
    }

}
