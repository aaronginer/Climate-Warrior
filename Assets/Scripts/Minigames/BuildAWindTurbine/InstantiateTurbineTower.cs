using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTurbineTower : MonoBehaviour
{
    // Start is called before the first frame update

    // Reference to the wall block Prefab.
    public GameObject wallBlock;

    private GameObject currentWallBlockToMove = null;
    private Rigidbody2D rigidbodyComponent = null;

    private float speed = 0.3f;
    
    private StateEnum currentState = StateEnum.still;

    private bool didCollide = false;

    private static float X_COORD_BOUND = .95f;
    private static float X_COORD_LOSE_MIN = -0.016f;
    private static float X_COORD_LOSE_MAX = 0.216f;
    
            

    private enum StateEnum
    {
        still,
        movingLeft,
        movingRight,
        released,
        lost
    }


    void Start()
    {
        SpawnNewWallBlock();
    }

    void SpawnNewWallBlock()
    {
        float positionX = Random.Range(-X_COORD_BOUND, X_COORD_BOUND);

        currentWallBlockToMove = Instantiate(wallBlock, new Vector3(positionX, 0.2f, 0), Quaternion.identity);

        rigidbodyComponent = currentWallBlockToMove.GetComponent<Rigidbody2D>();

        speed += 0.01f;
        currentState = positionX > 0 ? StateEnum.movingLeft : StateEnum.movingRight;
        didCollide = false;
    }

    void MoveLeft()
    {
        Rigidbody2D rb = currentWallBlockToMove.GetComponent<Rigidbody2D>();
        rb.MovePosition(currentWallBlockToMove.transform.position - new Vector3(1, 0, 0) * Time.deltaTime * speed);
    }

    void MoveRight()
    {
        Rigidbody2D rb = currentWallBlockToMove.GetComponent<Rigidbody2D>();
        rb.MovePosition(currentWallBlockToMove.transform.position + new Vector3(1, 0, 0) * Time.deltaTime * speed);
    }

    private float getCurrentX()
    {
        return currentWallBlockToMove.transform.position.x;
    }

    void CheckReleaseOk()
    {
        float currentX = getCurrentX();
        if(currentX < X_COORD_LOSE_MIN || currentX > X_COORD_LOSE_MAX)
        {
            currentState = StateEnum.lost;
        }
        
    }

    void CheckState()
    {
        if (currentWallBlockToMove == null) return;
        float currentX = getCurrentX();
        if (currentState == StateEnum.still)
        {

        } else if(currentState == StateEnum.released)
        {
            CheckReleaseOk();
        } else if (currentX < -X_COORD_BOUND)
        {
            currentState = StateEnum.movingRight;
        } else if (currentX > X_COORD_BOUND)
        {
            currentState = StateEnum.movingLeft;
        }
    }

    bool getIsMovingLeft()
    {
        return currentWallBlockToMove != null && currentState == StateEnum.movingLeft;
    }


    bool getIsMovingRight()
    {
        return currentWallBlockToMove != null && currentState == StateEnum.movingRight;
    }

    private void FixedUpdate()
    {
        bool isMoving = getIsMovingLeft() || getIsMovingRight();

        if (Input.GetKeyDown("space") && isMoving)
        {
            currentState = StateEnum.released;
        }

        if (getIsMovingLeft())
        {
            MoveLeft();
        } else if (getIsMovingRight())
        {
            MoveRight();
        }
        else if (currentState == StateEnum.released)
        {
           //WaitForCollision();
        }

        CheckState();

        

    }

    // Update is called once per frame
    void Update()
    {
        //MoveLeft();
    }
}
