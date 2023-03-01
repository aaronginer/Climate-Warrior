using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Missions;
using UnityEngine.Tilemaps;

public class InstantiateTurbineTower : MonoBehaviour
{
    // Start is called before the first frame update

    public Image progressBar;

    // Reference to the wall block Prefab.
    public GameObject wallBlock;
    public GameObject ProgressBar;
    public float moveUpSpeed = 0.55f;

    private GameObject currentWallBlockToMove = null;
    private Rigidbody2D rigidbodyComponent = null;
    private GameObject SkyDome = null;

    private float speed = 0.3f;
    private GameEnd gameEnd;

    private StateEnum currentState = StateEnum.still;

    private bool didCollide = false;

    private static float Y_COORD_TIMEOUT = 0.05f;
    private static float X_COORD_BOUND = .95f;
    private static float X_COORD_LOSE_MIN = -0.1f;
    private static float X_COORD_LOSE_MAX = 0.12f;
    private static int NUM_BLOCKS_WIN = 15;
    private int blockSpawnCount = 0;
    private float gravity;
    

    private List<GameObject> TowerObjects = new List<GameObject>();

    private TurbineManager manager;
    

    private enum StateEnum
    {
        still,
        movingLeft,
        movingRight,
        released,
        lost,
        timeout,
        win
    }


    void Start()
    {
        SpawnNewWallBlock(true);

        for (int i = 0; i < NUM_BLOCKS_WIN; i++)
        {
            GameObject TowerObject = GameObject.Find("WindTurbinePiece (" + i.ToString() + ")");
            TowerObjects.Add(TowerObject);
            TowerObject.SetActive(false);
        }

        gameEnd = GameObject.Find("EndScreen").GetComponent<GameEnd>();
        manager = GameObject.Find("Scripts").GetComponent<TurbineManager>();
        SkyDome = GameObject.Find("SkyDome");
    }

    void SpawnNewWallBlock(bool isFirstSpawn = false)
    {
        
        float positionX = Random.Range(-X_COORD_BOUND, X_COORD_BOUND);

        if(!isFirstSpawn)
        {
            MoveCameraUp();
        }
            

        currentWallBlockToMove = Instantiate(wallBlock, new Vector3(positionX, 0.2f + moveUpSpeed * blockSpawnCount, 0), Quaternion.identity);
        blockSpawnCount += 1;
        didCollide = false;

        rigidbodyComponent = currentWallBlockToMove.GetComponent<Rigidbody2D>();
        gravity = rigidbodyComponent.gravityScale;
        speed += 0.05f;
        currentState = positionX > 0 ? StateEnum.movingLeft : StateEnum.movingRight;

    }

    void MoveCameraUp()
    {   
        Camera.main.transform.position += new Vector3(0, moveUpSpeed, 0);
        SkyDome.transform.position += new Vector3(0, moveUpSpeed, 0);
        ProgressBar.transform.position += new Vector3(0, moveUpSpeed, 0);
        Destroy(currentWallBlockToMove);
        TowerObjects[blockSpawnCount - 1].SetActive(true);        
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
    
    private float getCurrentY()
    {
        return currentWallBlockToMove.transform.position.y;
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
        if (blockSpawnCount >= NUM_BLOCKS_WIN-1)
        {
            currentState = StateEnum.win;
            progressBar.fillAmount = 1.0f;
            gameEnd.DiplayEndView(manager.wonText);
            gameEnd.ShowButtonWon();
            GameStateManager.Instance.CurrentMission.State.stateID = (int)MissionWindTurbine.States.WindTurbineBuilt;
            return true;
        }
        return false;
    }

    void CheckReleaseOkAfterCollide()
    {
        float currentX = getCurrentX();
        if (currentX < X_COORD_LOSE_MIN || currentX > X_COORD_LOSE_MAX)
        {
            // collision x value bad,, lost
            currentState = StateEnum.lost;
            gameEnd.DiplayEndView(manager.lostText);
            gameEnd.ShowButtonsLost();
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
        if (currentWallBlockToMove == null) return;
        if (currentState == StateEnum.released && didCollide)
        {
            CheckReleaseOkAfterCollide();
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
        progressBar.fillAmount = (float)(blockSpawnCount-1) / (NUM_BLOCKS_WIN-1);
    }

    void CheckTimeout()
    {
        bool isMoving = getIsMovingLeft() || getIsMovingRight();
        if (isMoving && getCurrentY() < Y_COORD_TIMEOUT)
        {
            currentState = StateEnum.timeout;
            gameEnd.DiplayEndView(manager.timeOutText);
            gameEnd.ShowButtonsLost();
        }
    }

    private void FixedUpdate()
    {
  
        CheckTimeout();
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
        if (PauseScript.instance.gamePaused)
        {
            rigidbodyComponent.gravityScale = 0.0f;
            return;
        } 
  
        rigidbodyComponent.gravityScale = gravity;

        bool isMoving = getIsMovingLeft() || getIsMovingRight();
        if (Input.GetKeyDown(KeyCode.Space) && isMoving)
        {
            currentState = StateEnum.released;
        }
    }
}
