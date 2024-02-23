using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool isManuallyFrozen = true;

    [SerializeField] private float freezeCooldownTime = 2f;
    private float freezeCooldownTimer = 0f;

    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = walkTime;

        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

            if (!isManuallyFrozen && freezeCooldownTimer <=0)
            {
                isManuallyFrozen = true;

                previousVelocity = rb.velocity;
                rb.velocity = Vector2.zero;

                rb.isKinematic = true;

                animator.speed = 0;
            }
            else
            {
                isManuallyFrozen = false;
                isTimeStopped = false;

                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = previousVelocity;

                freezeCooldownTimer = freezeCooldownTime;
                animator.speed = 1;
            }
        }


        if (freezeCooldownTimer > 0)
            freezeCooldownTimer -= Time.deltaTime;

        if (isManuallyFrozen)
            isTimeStopped = true;


        if (WallInFront())
        {
            //Change direction
            currentDirection *= -1;
            transform.localScale = new Vector3(currentDirection, 1, 1);
        }

        if (!isTimeStopped)
            EntityMovement();


        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(gameObject.GetComponent<BoxCollider2D>().bounds.center, gameObject.GetComponent<BoxCollider2D>().bounds.size, 0f, LayerMask.GetMask("ActiveBorder"));

            if (colliders.Length > 0)
                gameObject.GetComponent<SpriteRenderer>().color = GameManager.instance.palletes[GameManager.instance.currentPalleteIndex].backgroundColor;
            else
                gameObject.GetComponent<SpriteRenderer>().color = GameManager.instance.palletes[GameManager.instance.currentPalleteIndex].foregroundColor;
        }

    }

    bool WallInFront()
    {
        bool wallInFront = Physics2D.Raycast(transform.position, new Vector2(currentDirection, 0), 4f, LayerMask.GetMask("Wall"));
        Debug.DrawRay(transform.position, new Vector2(currentDirection * 4, 0), Color.red);

        return wallInFront;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
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

            if(vertical > 0)
            {
                //Up Collision
            }
            else if(vertical < 0)
            {
                //Down Collision
            }
            else if(horizontal > 0)
            {
                //Right Collision
                currentDirection *= -1;
                transform.localScale = new Vector3(currentDirection, 1, 1);
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            }
            else if(horizontal < 0)
            {
                //Left Collision
                currentDirection *= -1;
                transform.localScale = new Vector3(currentDirection, 1, 1);
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
    

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Killer"))
            Death();

        if (collision.gameObject.CompareTag("MovingPlatform") && !collision.gameObject.GetComponent<TimeStoppableEntity>().isTimeStopped)
            gameObject.layer = collision.gameObject.layer;
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            if (isTimeStopped)
                isTimeStopped = false;
        }

        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
            gameObject.layer = LayerMask.NameToLayer("Player");
    }

}
