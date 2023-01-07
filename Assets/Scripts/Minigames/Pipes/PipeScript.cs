using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PipeScript : MonoBehaviour
{

    public bool isStaticPipe;
    
    private float[] rotations = { 0, 90, 180, 270 };
    void Start()
    {
        if (!isStaticPipe)
        {
            int rand = Random.Range(0, rotations.Length);
            transform.eulerAngles = new Vector3(0, 0, rotations[rand]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        transform.Rotate(new Vector3(0,0,90));
    }
}
