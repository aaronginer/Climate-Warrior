using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class InstantiateTurbineTower : MonoBehaviour
{
    // Start is called before the first frame update

    //public Image progressBar;

    // Reference to the wall block Prefab.
    public GameObject wallBlock;

    private GameObject currentWallBlockToMove = null;
    private Rigidbody2D rigidbodyComponent = null;

    private float speed = 0.3f;
    
    private StateEnum currentState = StateEnum.still;

    private bool didCollide = false;

    private static float X_COORD_BOUND = .95f;
    private static float X_COORD_LOSE_MIN = -0.1f;
    private static float X_COORD_LOSE_MAX = 0.12f;
    private static int NUM_BLOCKS_WIN = 15;

    private int blockSpawnCount = 0;
    

    private List<GameObject> gameObjectsToMove = new List<GameObject>();
    

    private enum StateEnum
    {
        still,
        movingLeft,
        movingRight,
        released,
        lost,
        win
    }


    void Start()
    {
        gameObjectsToMove.Add(GameObject.Find("TilesGrass"));
        gameObjectsToMove.Add(GameObject.Find("TilemapTower"));
        SpawnNewWallBlock(true);
    }

    void SpawnNewWallBlock(bool isFirstSpawn = false)
    {
        float positionX = Random.Range(-X_COORD_BOUND, X_COORD_BOUND);

        if(!isFirstSpawn)
        {
            MoveOtherGameObjectsDown();
        }
            

        currentWallBlockToMove = Instantiate(wallBlock, new Vector3(positionX, 0.2f, 0), Quaternion.identity);
        blockSpawnCount += 1;
        didCollide = false;

        gameObjectsToMove.Add(currentWallBlockToMove);

        rigidbodyComponent = currentWallBlockToMove.GetComponent<Rigidbody2D>();

        speed += 0.05f;
        currentState = positionX > 0 ? StateEnum.movingLeft : StateEnum.movingRight;
    }

    void MoveOtherGameObjectsDown()
    {
        int numGameObjects = gameObjectsToMove.Count;
        if (numGameObjects > 4)
        {
            GameObject blockOnTop = gameObjectsToMove[numGameObjects - 1];
            GameObject blockBelowTop = gameObjectsToMove[numGameObjects - 2];
            GameObject blockToRemove = gameObjectsToMove[numGameObjects - 3];
            Vector3 posTopBlock = blockOnTop.transform.position;
            Vector3 posBlockBelowTop = blockBelowTop.transform.position;
            Vector3 posBlockToRemove = blockToRemove.transform.position;
            Destroy(blockToRemove);
            gameObjectsToMove.RemoveAt(numGameObjects - 3);
            blockOnTop.transform.position = new Vector3(posTopBlock.x, posBlockBelowTop.y, posTopBlock.z);
            blockBelowTop.transform.position = new Vector3(posBlockBelowTop.x, posBlockToRemove.y, posBlockBelowTop.z);
        } else
        {
            foreach (GameObject obj in gameObjectsToMove)
            {
                obj.transform.position -= new Vector3(0, 0.6f, 0);
            }
        }
        
    }

    void MoveLeft()
    {
        rigidbodyComponent.MovePosition(currentWallBlockToMove.transform.position - new Vector3(1, 0, 0) * Time.deltaTime * speed);
    }

    void MoveRight()
    {
        rigidbodyComponent.MovePosition(currentWallBlockToMove.transform.position + new Vector3(1, 0, 0) * Time.deltaTime * speed);
    }

    private float getCurrentX()
    {
        return currentWallBlockToMove.transform.position.x;
    }

    void CheckMovingBound()
    {
        if (currentWallBlockToMove == null) return;
        float currentX = getCurrentX();
        if (currentX < -X_COORD_BOUND)
        {
            currentState = StateEnum.movingRight;
        }
        else if (currentX > X_COORD_BOUND)
        {
            currentState = StateEnum.movingLeft;
        }
    }

    bool CheckWin()
    {
        if (blockSpawnCount >= NUM_BLOCKS_WIN)
        {
            currentState = StateEnum.win;
            return true;
        }
        return false;
    }

    void CheckReleaseOk()
    {
        float currentX = getCurrentX();
        if (currentX < X_COORD_LOSE_MIN || currentX > X_COORD_LOSE_MAX)
        {
            // coollision x value bad,, lost
            currentState = StateEnum.lost;
        } else
        {

            // coollision OK, spawning new block
            if(!CheckWin())
            {
                SpawnNewWallBlock();
            }
            
        }

    }

    void CheckCollisionState()
    {
        Debug.Log("checking collision state");

        if (currentWallBlockToMove == null) return;

        Debug.Log("    currentWallBlockToMove is not null");

        if (currentState == StateEnum.released && didCollide)
        {
            CheckReleaseOk();
        } else if (currentState == StateEnum.still)
        {

        }
    }

    public void onCollide()
    {
        didCollide = true;
    }

    bool getIsMovingLeft()
    {
        return currentWallBlockToMove != null && currentState == StateEnum.movingLeft;
    }


    bool getIsMovingRight()
    {
        return currentWallBlockToMove != null && currentState == StateEnum.movingRight;
    }

    void FillProgressBar()
    { 
        //progressBar.fillAmount
    }

    private void FixedUpdate()
    {
        if (getIsMovingLeft())
        {
            MoveLeft();
            CheckMovingBound();
        } else if (getIsMovingRight())
        {
            MoveRight();
            CheckMovingBound();
        }
        else
        {
            CheckCollisionState();
        }
        FillProgressBar();

    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = getIsMovingLeft() || getIsMovingRight();

        if (Input.GetKeyDown("space") && isMoving)
        {
            currentState = StateEnum.released;
        }
    }
}
