using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicScript : MonoBehaviour
{
    public List<string> scenes;
    public AudioSource audioSource;
    
    public void Play(string scene)
    {
        audioSource.volume = scenes.Contains(scene) ? 0.04f : 0.0f;
    }
}
