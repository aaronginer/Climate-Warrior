using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PipeScript : MonoBehaviour
{

    public bool isStaticPipe;
    
    private float[] rotations = { 0, 90, 180, 270 };

    private float rotationSet;
    void Start()
    {
        // if (!isStaticPipe && Random.Range(0.0f, 10.0f) > 9.5f)
        if (!isStaticPipe)
        {
            int rand = Random.Range(0, rotations.Length);
            rotationSet = rotations[rand];
            transform.eulerAngles = new Vector3(0, 0, rotationSet);
        }
    }

    public bool isOriginalRotation()
    {
        return isStaticPipe || rotationSet % 360 == 0;
    }

    private void OnMouseDown()
    {
        if (!isStaticPipe)
        {
            rotationSet += 90;
            transform.Rotate(new Vector3(0,0,90));
            PipesGame pipesGame = GameObject.Find("Scripts").GetComponent<PipesGame>();
            pipesGame.checkGameWin();
        }
    }
}
