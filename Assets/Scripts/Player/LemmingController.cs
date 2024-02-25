using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LemmingController : TimeStoppableEntity
{
    float timeRemaining = 0;
    int currentDirection = 1;

    [SerializeField]
    float walkTime = 0.2f;
    bool imDead = false;


    [SerializeField]
    GameObject lemming;

    private Vector3 previousVelocity;
    private float previousAngularVelocity;

    [SerializeField] private bool isManuallyFrozen = false;
    private bool snappedToPlatform = true;

    [SerializeField] private float freezeCooldownTime = 2f;
    private float freezeCooldownTimer = 0f;

    private Animator animator;

    private bool isGrounded = false;
    [SerializeField] private float feetOffset = 4f;
    [SerializeField] private float heightOffset = 4f;
    private BoxCollider2D collider;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        isGrounded = IsGrounded();
        timeRemaining = walkTime;
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isGrounded", isGrounded);
        isGrounded = IsGrounded();

        if (freezeCooldownTimer > 0)
            freezeCooldownTimer -= Time.deltaTime;

        if (isManuallyFrozen && !snappedToPlatform)
            isTimeStopped = true;


        if (WallInFront())
        {
            if (snappedToPlatform)
                return;

            //Change direction
            currentDirection *= -1;
            FlipHorizontally();
        }

        if (!isTimeStopped)
            EntityMovement();

        // Keep Rotation Depending On Direction
        transform.localScale = new Vector3(currentDirection, 1, 1);


        if (gameObject.layer == LayerMask.NameToLayer("Player") || gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(gameObject.GetComponent<BoxCollider2D>().bounds.center, gameObject.GetComponent<BoxCollider2D>().bounds.size, 0f, LayerMask.GetMask("ActiveBorder"));

            if (colliders.Length > 0)
                gameObject.GetComponent<SpriteRenderer>().color = PalleteController.instance.palletes[PalleteController.instance.currentPalleteIndex].backgroundColor;
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = PalleteController.instance.palletes[PalleteController.instance.currentPalleteIndex].foregroundColor;

                if (isManuallyFrozen && snappedToPlatform && isTimeStopped)
                    isTimeStopped = false;
            }
        }

    }

    bool WallInFront()
    {

        bool wallInFront = Physics2D.Raycast(new Vector3(transform.position.x + (collider.offset.x * currentDirection), transform.position.y + collider.offset.y, 0), new Vector2(currentDirection, 0), 4f, LayerMask.GetMask("Wall"));
        Debug.DrawRay(new Vector3(transform.position.x + (collider.offset.x * currentDirection), transform.position.y + collider.offset.y, 0), new Vector2(currentDirection * 4, 0), Color.red);

        return wallInFront;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("MovingPlatform") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            int vertical = 0;
            int horizontal = 0;
            float threshold = 0.85f;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 direction = contact.point - (Vector2)transform.position;

                direction.Normalize();

                if (Vector2.Dot(direction, Vector2.up) > threshold)
                {
                    vertical++;
                }
                else if (Vector2.Dot(direction, Vector2.down) > threshold)
                {
                    vertical--;
                }
                else if (Vector2.Dot(direction, Vector2.right) > threshold)
                {
                    horizontal++;
                }
                else if (Vector2.Dot(direction, Vector2.left) > threshold)
                {
                    horizontal--;
                }
            }

            if (vertical > 0)
            {
                //Up Collision
            }
            else if (vertical < 0)
            {
                //Down Collision
            }
            else if (horizontal > 0)
            {
                //Right Collision
                currentDirection *= -1;
                FlipHorizontally();

            }
            else if (horizontal < 0)
            {
                //Left Collision
                currentDirection *= -1;
                FlipHorizontally();
            }
        }
    }

    public void Death()
    {
        if (!imDead)
        {
            GameManager.instance.RestartLevel();
            Destroy(gameObject);
            imDead = true;

        }

    }
    public void EntityMovement()
    {
        if (timeRemaining > 0)
            timeRemaining -= Time.deltaTime;
        else
        {
            //Move one unit forward
            transform.position += new Vector3(currentDirection, 0, 0);
            timeRemaining = walkTime;
        }
    }

    private void CheckForPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x + (collider.offset.x * currentDirection), transform.position.y - collider.offset.y, 0), new Vector2(currentDirection, 0), 4f, LayerMask.GetMask("Wall", "Ground"));

        if (isManuallyFrozen)
        {
            Vector2 platformDirection = hit.collider.gameObject.GetComponent<MovingPlatform>().currentMovementDirection;
            if (platformDirection.x != 0)
            {
                if (currentDirection != Mathf.RoundToInt(platformDirection.x))
                {
                    FlipHorizontally();
                    transform.position = new Vector3(transform.position.x - 3 * currentDirection, transform.position.y, 0);
                }

                currentDirection = Mathf.RoundToInt(platformDirection.x);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Killer") && !isDead)
        {
            isDead = true;
            Death();
        }


        if (collision.gameObject.CompareTag("MovingPlatform") && !collision.gameObject.GetComponent<TimeStoppableEntity>().isTimeStopped)
        {
            gameObject.layer = collision.gameObject.layer;
            snappedToPlatform = true;

            if (isManuallyFrozen)
            {
                Vector2 platformDirection = collision.collider.gameObject.GetComponent<MovingPlatform>().currentMovementDirection;
                if (platformDirection.x != 0)
                {
                    if (currentDirection != Mathf.RoundToInt(platformDirection.x))
                    {
                        FlipHorizontally();
                        transform.position = new Vector3(transform.position.x - 3 * currentDirection, transform.position.y, 0);
                    }

                    currentDirection = Mathf.RoundToInt(platformDirection.x);
                }
            }
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            if (isTimeStopped)
                isTimeStopped = false;
            snappedToPlatform = false;
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
            gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private bool IsGrounded()
    {

        LayerMask groundLayer = LayerMask.NameToLayer("Ground");

        bool centerGrounded = Physics2D.Raycast((Vector2)transform.position + new Vector2(collider.offset.x * currentDirection, collider.offset.y), Vector2.down, heightOffset, LayerMask.GetMask("Ground", "Default", "Wall"));
        Debug.DrawRay(transform.position + new Vector3(collider.offset.x * currentDirection, collider.offset.y, 0f), new Vector2(0f, -heightOffset), Color.white);

        return centerGrounded;
    }

    public void FlipHorizontally()
    {
        // Assuming the pivot is at the top left of the sprite
        // Calculate the pivot offset based on the sprite's bounds
        float pivotOffsetX = gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;

        // Flip the sprite by changing its localScale.x
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;

        // Adjust the position to compensate for the pivot offset
        // This moves the GameObject so that it appears stationary relative to the screen
        Vector3 position = transform.position;
        position.x -= pivotOffsetX * localScale.x * 2; // Multiply by 2 to compensate for the initial offset and the flip
        transform.position = position;
    }

    public void FriisSelf()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        if (!isManuallyFrozen && freezeCooldownTimer <= 0 && isGrounded)
        {
            isManuallyFrozen = true;
            AudioManager.instance.PlaySFX("FreezeLemming");
            animator.speed = 0;

        }
        else if (isGrounded)
        {
            if (isManuallyFrozen)
            {
                isManuallyFrozen = false;
                isTimeStopped = false;
                AudioManager.instance.PlaySFX("UnfreezeLemming");

                freezeCooldownTimer = freezeCooldownTime;
                animator.speed = 1;
            }

        }
    }
}
