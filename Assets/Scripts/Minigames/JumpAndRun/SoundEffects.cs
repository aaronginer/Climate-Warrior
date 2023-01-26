using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioClip coinSound;

    void Awake()
    {
    }

    public void MakeCoinSound()
    {
        MakeSound(coinSound);
    }

    private void MakeSound(AudioClip sound)
    {
        AudioSource.PlayClipAtPoint(sound, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
