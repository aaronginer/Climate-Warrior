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
            case "StartTurbineMinigame" when GameStateManager.GSM.gameState.playerData.CheckMissionCompleted(MiniGame.jumpAndRunCollectTurbineParts):
                SceneManager.LoadScene(Constants.SceneNames.miniGameBuildAWindTurbine, LoadSceneMode.Single);
                break;
        }
    }
}
