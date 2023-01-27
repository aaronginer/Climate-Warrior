using System.Collections;
using Scoring;
using UnityEngine;

namespace Missions
{
    public class MissionCompleteScript : MonoBehaviour
    {
        public GameObject scoreMaxText;
        public GameObject timeText;
        public GameObject deductionsDecisionsText;
        
        public GameObject scoreMaxScore;
        public GameObject timeScore;
        public GameObject deductionsDecisionsScore;
        public GameObject scoreFinalScore;
        
        public GameObject continueButton;

        private int _finalScore;
        
        private void Awake()
        {
            scoreMaxText.SetActive(false);
            timeText.SetActive(false);
            deductionsDecisionsText.SetActive(false);
            
            scoreMaxScore.SetActive(false);
            timeScore.SetActive(false);
            deductionsDecisionsScore.SetActive(false);
            scoreFinalScore.SetActive(false);
            
            continueButton.SetActive(false);
        }

        void Start()
        {
            UIStateManager.UISM.uIState = UIState.MissionFinish;
            StartCoroutine(ShowMissionResults());
        }

        IEnumerator ShowMissionResults()
        {
            Mission currentMission = GameStateManager.Instance.CurrentMission;
            
            Debug.Assert(currentMission != null);
            
            int baseScore = currentMission.BaseScore;
            
            float missionMaxTime = currentMission.MissionMaxTime;
            float missionTimeLeft = currentMission.State.timeLeft;
            int scoreTime = (int) (missionTimeLeft / missionMaxTime * currentMission.TimeScoreMax);
            
            int deductionsDecisions = currentMission.DeductionsDecisions;
            
            _finalScore = baseScore + scoreTime + deductionsDecisions;

            // update the global score
            GameStateManager.Instance.gameState.score += _finalScore;
            
            Destroy(currentMission.ClimateScoreObject);
            
            scoreMaxText.SetActive(true);
            scoreMaxScore.SetActive(true);
            
            scoreMaxScore.GetComponent<AnimateCounterScript>().StartAnimate(baseScore);

            yield return new WaitForSeconds(1.5f);
            
            timeText.SetActive(true);
            this.timeScore.SetActive(true);
            this.timeScore.GetComponent<AnimateCounterScript>().StartAnimate(scoreTime);

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
            UIStateManager.UISM.uIState = UIState.None;
            GameStateManager.Instance.EndMission();
            GameObject.Find("Score").GetComponentInChildren<AnimateCounterScript>().StartAnimate(_finalScore);
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
