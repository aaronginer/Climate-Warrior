using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Missions.Flooding
{
    public class FloodingPlayerScript : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("entering + " + col.gameObject.name);
        }
    }

}
