using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip sound;
    public int value = 5;

    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeSound()
    {
        AudioSource.PlayClipAtPoint(sound, transform.position);
    }

    private void OnDestroy()
    {
    }
}
