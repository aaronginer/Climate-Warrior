using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace Missions.Flooding
{
    public class FloodingGameScript : MonoBehaviour
    {
        public List<GameObject> gridVariations;
        public GameObject player;
        public GameObject grandma;

        private Tilemap _activeVariation;
        private bool _phase2;

        private void Awake()
        {
            foreach (var gridVariation in gridVariations)
            {
                gridVariation.SetActive(false);
            }

            Random r = new Random();
            int randomVariation = r.Next(0, 3);
            _activeVariation = gridVariations[randomVariation].GetComponent<Tilemap>();
            gridVariations[randomVariation].SetActive(true);
        }

        private void Start()
        {
            PathIndicatorScript.Instance.Disable();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.name != "WaterTrigger") return;
            StartCoroutine(FallInWater());
        }
        
        public void Phase2()
        {
            grandma.transform.SetParent(player.transform);
            grandma.transform.localPosition = new Vector3(0, 0.1f, 0);
            _phase2 = true;
            
            GameStateManager.Instance.CurrentMission.State.stateID = (int)MissionFlooding.States.GrandmaSaved;
            GameStateManager.Instance.CurrentMission.AdvanceState();

            StartCoroutine(FadeOutTiles());
        }

        private IEnumerator FallInWater()
        {
            player.GetComponent<PlayerController>().enabled = false;
            const float seconds = 3;
            Color color = player.GetComponent<SpriteRenderer>().color;
            while (color.a > 0)
            {
                color.a -= 0.01f;
                player.GetComponent<SpriteRenderer>().color = color;
                if (_phase2) grandma.GetComponent<SpriteRenderer>().color = color;  
                yield return new WaitForSeconds(seconds/100f);
            }
            
            GameStateManager.Instance.CurrentMission.State.stateID = (int)MissionFlooding.States.Init;
            SceneManager.LoadScene("Village");
        }
        
        private IEnumerator FadeOutTiles()
        {
            const float seconds = 3;
            Color color = _activeVariation.color;
            while (color.a > 0)
            {
                color.a -= 0.01f;
                _activeVariation.color = color;  
                yield return new WaitForSeconds(seconds/100f);
            }
        }
    }
}