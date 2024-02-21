using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemmingController : TimeStoppableEntity
{
    float timeRemaining = 0;
    int currentDirection = 1;
    [SerializeField]
    float walkTime = 0.2f;
    [SerializeField]
    Vector2 startingPosition = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = walkTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (WallInFront())
        {
            //Change direction
            currentDirection *= -1;
            transform.localScale = new Vector3(currentDirection, 1, 1);
        }

        if (!isTimeStopped)
            EntityMovement();
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
        Instantiate(gameObject, startingPosition, Quaternion.identity);
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
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Killer"))
            Death();
    }

}
