using System;
using System.Collections;
using System.Collections.Generic;
using Dialogue;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTriggers : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.name)
        {
            case "StartJmpNRunMinigame":
                SceneManager.LoadScene(Constants.SceneNames.miniGameJumpAndRunCollectTurbine, LoadSceneMode.Single);
                break;
            case "StartTurbineMinigame" when GameStateManager.Instance.gameState.playerData.CheckMissionCompleted(MiniGame.jumpAndRunCollectTurbineParts):
                SceneManager.LoadScene(Constants.SceneNames.miniGameBuildAWindTurbine, LoadSceneMode.Single);
                break;
            case "HydroPlantLowerFloor":
                GameStateManager.Instance.gameState.playerData.position = new Vector3(2.01200008f, -0.746999979f, 0);
                SceneManager.LoadScene("HydroPlantLower", LoadSceneMode.Single);
                break;
            case "HydroPlantUpperFloor":

                GameStateManager.Instance.gameState.playerData.position = new Vector3(1.00800002f, -0.425000012f, 0);
                SceneManager.LoadScene("HydroPlantUpper", LoadSceneMode.Single);
                break;
        }
    }
}
