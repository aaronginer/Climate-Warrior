using System.Collections;
using Scoring;
using UnityEngine;

namespace Missions
{
    public class MissionFailedScript : MonoBehaviour
    {
        public GameObject scoreFinalScore;
        
        public GameObject continueButton;

        private int _finalScore = -250;
        
        private void Awake()
        {
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
            ClimateScoreManager csManager =
                currentMission.ClimateScoreObject.GetComponentInChildren<ClimateScoreManager>();
            
            Debug.Assert(currentMission != null);
            Debug.Assert(csManager != null);

            // update the global score
            GameStateManager.Instance.gameState.score += _finalScore;
            
            Destroy(currentMission.ClimateScoreObject);

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
        
        // Spawns a mission failed window. Use when mission is completed. Stat calculations happen automatically.
        public static void MissionFailed()
        {
            Debug.Log(GameStateManager.Instance.currentTaskDisplay);
            GameStateManager.Instance.SetCurrentTaskActive(false);
            var missionCompletePrefab = Resources.Load("Missions/MissionFailed") as GameObject;
            Instantiate(missionCompletePrefab, GameObject.Find("Canvas").transform);
        }
    }

}
