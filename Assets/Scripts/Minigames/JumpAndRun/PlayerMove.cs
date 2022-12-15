using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float jumpAmount = 10;
    public float forceUp = 1;
    public float forceDown = 1;
    public int counter = 0;
    public float movement = 5;
    [SerializeField] private LayerMask platformLayerMask;

    
    private Rigidbody2D rigidbodyComponent;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private SpriteRenderer playerSprite;
    private GameEnd gameEndScreen;
    private BoxCollider2D finishSign;

    void Start()
    {
        
    }

    private void Awake()
    {
        rigidbodyComponent = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        finishSign = GameObject.Find("Finish").GetComponent<BoxCollider2D>();
        gameEndScreen = GameObject.Find("EndScreen").GetComponent<GameEnd>();
        animator.speed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreBoard.instance.running == false)
            return;
        if (IsGrounded() && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))) rigidbodyComponent.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
        if (rigidbodyComponent.velocity.y >= 0) rigidbodyComponent.gravityScale = forceUp;
        if (rigidbodyComponent.velocity.y < 0) rigidbodyComponent.gravityScale = forceDown;

        HandleMovement();
        if(this.gameObject.transform.position.x >= finishSign.transform.position.x)
        {
            gameEndScreen.ShowEndScreen(false);
        }
    }


    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, platformLayerMask);

        return hit.collider != null;
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rigidbodyComponent.velocity = new Vector2(-movement, rigidbodyComponent.velocity.y);
            playerSprite.flipX = true;
            if (IsGrounded())
            {
                animator.SetBool("isMoving", true);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidbodyComponent.velocity = new Vector2(+movement, rigidbodyComponent.velocity.y);
            playerSprite.flipX = false;
            animator.SetBool("isMoving", true);

        }
        else
        {
            animator.SetBool("isMoving", false);
            rigidbodyComponent.velocity = new Vector2(0, rigidbodyComponent.velocity.y);
        }
    }

    private void FixedUpdate()
    {
        // rigidbodyComponent.velocity = movement;
        if (transform.position.y < -5)
        {
            gameEndScreen.ShowEndScreen(true);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collectible coin = collision.gameObject.GetComponent<Collectible>();
        if (coin != null)
        {
            ScoreBoard.instance.UpdateScore(coin.value);
            Destroy(coin.gameObject);
        }
    }

    private void OnDestroy()
    {
        ScoreBoard.instance.timerRunning = false;
    }
}
