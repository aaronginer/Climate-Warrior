using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Missions.Flooding
{
    public class FloodingPlayerScript : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.name != "WaterTrigger") return;
            GameStateManager.Instance.CurrentMission.State.stateID = (int)MissionFlooding.States.Init;
            SceneManager.LoadScene("Village");
        }
    }

}
