using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroll : MonoBehaviour
{
    // Start is called before the first frame update
    private float startpos;
    public float speed = 0.5f;

    void Start()
    {
        startpos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = (Time.time * speed);

        transform.position = new Vector3(startpos - dist, transform.position.y, transform.position.z);
        if (transform.position.x < -14f) startpos += 90;
    }
}
