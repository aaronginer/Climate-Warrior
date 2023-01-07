using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipesRotation : MonoBehaviour
{
    // Start is called before the first frame update

    private List<GameObject> pipeObjects;

    
    void Start()
    {
        pipeObjects = CollectPipeObjects();
    }

    List<GameObject> CollectPipeObjects()
    {
        GameObject pipesWrapper = GameObject.Find("Pipes");
        List<GameObject> pipes = new List<GameObject>();
        foreach (Transform rowTransform in pipesWrapper.GetComponentsInChildren<Transform>())
        {
            if (rowTransform.parent != pipesWrapper.transform) continue;
            GameObject row = rowTransform.gameObject;
            foreach (Transform pipeTransform in row.GetComponentsInChildren<Transform>())
            {
                if (pipeTransform.parent != rowTransform) continue;
                GameObject pipe = pipeTransform.gameObject;
                pipes.Add(pipe);
            }
        }
        return pipes;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
