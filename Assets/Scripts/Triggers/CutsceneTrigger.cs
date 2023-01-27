using UnityEngine;
using UnityEngine.Playables;
 
 
public class CutsceneTrigger: MonoBehaviour
{
    public GameObject Motor;
 
    private void OnTriggerEnter(Collider other)
    {
        PlayableDirector pd = Motor.GetComponent<PlayableDirector>();
        if (pd != null)
        {
            pd.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
       PlayableDirector pd = Motor.GetComponent<PlayableDirector>();
       
        {
                 
            if (pd != null)
            {
               
             pd.Stop();
             pd.time = 0f;
             pd.initialTime = 0f;
             pd.Evaluate();
             }
        }
 
        }
}