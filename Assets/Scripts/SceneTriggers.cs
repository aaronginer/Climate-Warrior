using System;
using System.Collections;
using System.Collections.Generic;
using Dialogue;
using Unity.VisualScripting;
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
            default:
                if (GameStateManager.Instance.CurrentMission != null)
                {
                    GameStateManager.Instance.CurrentMission.HandleSceneTriggers(col.gameObject);
                }
                break;
        }
    }
}
