using UnityEngine;

public class SoundsScript : MonoBehaviour
{
    public AudioClip thunder;
    public AudioClip virusKill;
    public AudioClip rotatePipe;
    public AudioClip missionFail;
    public AudioClip missionSuccess;

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

    public void SoundMissionFail()
    {
        MakeSound(missionFail, 2.0f);
    }
    
    public void SoundMissionSuccess()
    {
        MakeSound(missionSuccess, 1.5f);
    }
    
    private void MakeSound(AudioClip originalClip, float volume=1.0f)
    {
        // As it is not 3D audio clip, position doesn't matter.
        AudioSource.PlayClipAtPoint(originalClip, transform.position, volume);
    }
}
