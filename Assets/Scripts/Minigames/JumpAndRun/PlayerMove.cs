using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float jumpAmount = 10;
    public float forceUp = 1;
    public float forceDown = 1;
    public float movement = 5;
    [SerializeField] private LayerMask platformLayerMask;

    
    private Rigidbody2D rigidbodyComponent;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private SpriteRenderer playerSprite;
    private BoxCollider2D finishSign;
    private GameManager manager;

    void Start()
    {

    }

    private void Awake()
    {
        finishSign = GameObject.Find("Finish").GetComponent<BoxCollider2D>();
        manager = GameObject.Find("Canvas").GetComponent<GameManager>();

        rigidbodyComponent = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        
        animator.speed = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (PauseScript.instance.gamePaused || !manager.gameRunning)
        {
            animator.SetBool("isMoving", false);
            return;
        }

        HandleMovement();

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space)) rigidbodyComponent.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
        if (rigidbodyComponent.velocity.y >= 0) rigidbodyComponent.gravityScale = forceUp;
        if (rigidbodyComponent.velocity.y < 0) rigidbodyComponent.gravityScale = forceDown;

        if (this.gameObject.transform.position.x >= finishSign.transform.position.x)
            manager.EndGame(false);
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
                animator.SetBool("LeftRight", true);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidbodyComponent.velocity = new Vector2(+movement, rigidbodyComponent.velocity.y);
            playerSprite.flipX = false;
            if (IsGrounded())
            {
                animator.SetBool("isMoving", true);
                animator.SetBool("LeftRight", true);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("LeftRight", false);
            rigidbodyComponent.velocity = new Vector2(0, rigidbodyComponent.velocity.y);
        }
    }

    private void FixedUpdate()
    {
        // rigidbodyComponent.velocity = movement;
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
            manager.EndGame(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collectible coin = collision.gameObject.GetComponent<Collectible>();
        if (coin != null)
        {
            manager.AddScore(coin.value);
            coin.MakeSound();
            Destroy(coin.gameObject);
        }
    }

    private void OnDestroy()
    {
    }
}
