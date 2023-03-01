using System;
using UnityEngine;

namespace Missions.WindTurbine
{
    public class ShowTurbine : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(GameStateManager.Instance.gameState.displayTurbine);
        }
    }
}