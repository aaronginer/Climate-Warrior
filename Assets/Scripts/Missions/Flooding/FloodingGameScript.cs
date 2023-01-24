using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        public void Phase2()
        {
            grandma.transform.SetParent(player.transform);
            grandma.transform.localPosition = new Vector3(0, 0.02f, 0);

            GameStateManager.Instance.CurrentMission.State.stateID = (int)MissionFlooding.States.GrandmaSaved;
            GameStateManager.Instance.CurrentMission.AdvanceState();

            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
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