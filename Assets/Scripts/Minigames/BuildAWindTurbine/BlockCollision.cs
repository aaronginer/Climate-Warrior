using UnityEngine;

public class BlockCollision : MonoBehaviour
{
    private bool didCollide = false;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!didCollide)
        {
            InstantiateTurbineTower turbineTowerScript = GameObject.Find("Scripts").GetComponent<InstantiateTurbineTower>();            
            turbineTowerScript.onCollide();
            didCollide = true;
        }
            
    }
}
