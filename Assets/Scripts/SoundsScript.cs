using UnityEngine;

public class SoundsScript : MonoBehaviour
{
    public AudioClip thunder;
    public AudioClip virusKill;
    public AudioClip rotatePipe;

    public void SoundThunder()
    {
        MakeSound(thunder);
    }

    public void SoundVirusKill()
    {
        MakeSound(virusKill);
    }

    public void SoundRotatePipe()
    {
        MakeSound(rotatePipe, 0.2f);
    }
    
    private void MakeSound(AudioClip originalClip, float volume=1.0f)
    {
        // As it is not 3D audio clip, position doesn't matter.
        AudioSource.PlayClipAtPoint(originalClip, transform.position, volume);
    }
}
