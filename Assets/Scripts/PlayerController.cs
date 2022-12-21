using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 1f;
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;

    Vector2 movementInput;
    Rigidbody2D rb;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    Animator animator;

    SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        gameObject.transform.position = GameStateManager.Instance.gameState.playerData.position;
        GameStateManager.Instance.gameState.playerData.sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        // if no input, idle
        if (movementInput != Vector2.zero) {
            bool success = TryMove(movementInput);

            if (!success && movementInput.x > 0) {
                success = TryMove(new Vector2(movementInput.x, 0));
            }

            if (!success && movementInput.y > 0) {
                success = TryMove(new Vector2(0, movementInput.y));
            }
            

            animator.SetBool("isMoving", success);
            
        } else {
            animator.SetBool("isMoving", false);
        }

        // set sprite direction
        if (movementInput.x < 0) {
            spriteRenderer.flipX = true;
        } else if (movementInput.x > 0) {
            spriteRenderer.flipX = false;
        }

        GameStateManager.Instance.gameState.playerData.position = gameObject.transform.position;
    }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }

    private bool TryMove(Vector2 direction) {
        if (!UIStateManager.UISM.CanPlayerMove())
        {
            return false;
        }
        
        if(direction != Vector2.zero) {
            int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset
            );

            if (count == 0) {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;           
            }
        }

        return false;
    }
}
