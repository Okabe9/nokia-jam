using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemmingController : TimeStoppableEntity
{
  private float timeRemaining = 0;
  private int currentDirection = 1;
  [SerializeField] private GameObject bichinPrefab;
  [SerializeField] private float walkTime = 0.2f;
  [SerializeField] private Vector2 startingPosition = new Vector2(0, 0);
  [SerializeField] private float feetOffset = 4f;
  [SerializeField] private float heightOffset;
  [SerializeField] private float fallSpeed = 1;

  // Start is called before the first frame update
  void Start()
  {
    timeRemaining = walkTime;
  }

  // Update is called once per frame
  void Update()
  {
    Debug.Log(isGrounded());
    if (WallInFront())
    {
      //Change direction
      currentDirection *= -1;
      transform.localScale = new Vector3(currentDirection, 1, 1);
    }

    if (!isTimeStopped)
      EntityMovement();


    Collider2D[] colliders = Physics2D.OverlapBoxAll(gameObject.GetComponent<BoxCollider2D>().bounds.center, gameObject.GetComponent<BoxCollider2D>().bounds.size, 0f, LayerMask.GetMask("ActiveBorder"));


    if (colliders.Length > 0)
      gameObject.GetComponent<SpriteRenderer>().color = GameManager.instance.palletes[GameManager.instance.currentPalleteIndex].backgroundColor;
    else
      gameObject.GetComponent<SpriteRenderer>().color = GameManager.instance.palletes[GameManager.instance.currentPalleteIndex].foregroundColor;

  }

  bool WallInFront()
  {
    bool wallInFront = Physics2D.Raycast(transform.position, new Vector2(currentDirection, 0), 4f, LayerMask.GetMask("Wall"));
    Debug.DrawRay(transform.position, new Vector2(currentDirection * 4, 0), Color.red);

    return wallInFront;
  }

  public void Death()
  {
    //Instantiate another lemming on the startingPosition
    Instantiate(bichinPrefab, startingPosition, Quaternion.identity);
    Destroy(gameObject);
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
      if (!isGrounded())
      {
        transform.position -= new Vector3(0, fallSpeed, 0);
      }
    }

  }

  private void OnCollisionStay2D(Collision2D collision)
  {

    if (collision.gameObject.CompareTag("Killer"))
      Death();
  }

  private bool isGrounded()
  {

    Vector2 rcPosLeft = new Vector2(transform.position.x - feetOffset, transform.position.y);
    bool leftGrounded = Physics2D.Raycast(rcPosLeft, Vector2.down, heightOffset, LayerMask.GetMask("Ground"));
    Debug.DrawRay(rcPosLeft, new Vector2(0, -heightOffset), Color.white);

    Vector2 rcPosRight = new Vector2(transform.position.x + feetOffset, transform.position.y);
    bool rightGrounded = Physics2D.Raycast(rcPosRight, Vector2.down, heightOffset, LayerMask.GetMask("Ground"));
    Debug.DrawRay(rcPosRight, new Vector2(0, -heightOffset), Color.white);

    bool centerGrounded = Physics2D.Raycast(transform.position, Vector2.down, heightOffset, LayerMask.GetMask("Ground"));
    Debug.DrawRay(transform.position, new Vector2(0, -heightOffset), Color.white);


    return leftGrounded || rightGrounded || centerGrounded;
  }
  /* private void JumpingLogic()
  {
    // Check if the player is on the ground
    character.isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundRayDistance, groundMask);

    //Set jumpVelocity negative so we don't get errors with positive velocities
    if (character.isGrounded && jumpVelocity.y < 0)
      jumpVelocity.y = -2f;

    //In-air logic
    if (jumpVelocity.y >= 0.3f)
    {
      character.isGrounded = false;
      jumpVelocity.y += (gravity - jumpVelocity.y) * Time.deltaTime;

    }
    else if (jumpVelocity.y < 0.3f && jumpVelocity.y >= 0f)
      jumpVelocity.y += gravity / 4f * Time.deltaTime;
    else
      jumpVelocity.y += (gravity - jumpVelocity.y * jumpVelocity.y / 5f) * 1.5f * Time.deltaTime;

    // Move vertically
    characterController.Move(jumpVelocity * Time.deltaTime);
  }

  private void Jump()
  {
    // Set States
    character.isGrounded = false;
    jumpVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    inputManager.bufferedAction = BufferActions.CLEAR;

    // Set Animation
    character.animator.SetTrigger(character.animKeys.jumpTriggerKey);
  } */
}
