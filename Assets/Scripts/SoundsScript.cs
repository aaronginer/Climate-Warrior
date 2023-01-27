using UnityEngine;

public class SoundsScript : MonoBehaviour
{
    public AudioClip thunder;

    public void SoundThunder()
    {
        MakeSound(thunder);
    }
    
    private void MakeSound(AudioClip originalClip)
    {
        // As it is not 3D audio clip, position doesn't matter.
        AudioSource.PlayClipAtPoint(originalClip, transform.position);
    }
}
