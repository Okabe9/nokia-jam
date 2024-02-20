using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovingPlatform : TimeStoppableEntity
{
    public List<Vector2> patrolPoints;
    public bool pingPongMovement = false;

    float timeRemaining = 0f;
    int verticalMovement = -2;

    [SerializeField] float activationTime = 0.2f;

    private int patrolTargetPoint = 0;
    private int patrolDirection = 1;


    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = activationTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTimeStopped)
            EntityMovement();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<LemmingController>().Death();
        }
    }

    private void EntityMovement()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            MoveToNextPatrolPoint();
            timeRemaining = activationTime;
        }
    }

    private void MoveToNextPatrolPoint()
    {

        // Loops patrol points
        if (patrolTargetPoint >= patrolPoints.Count || patrolTargetPoint < 0)
        {
            if (pingPongMovement)
            {
                patrolTargetPoint -= 2 * patrolDirection;
                patrolDirection *= -1;
            } else                 
                patrolTargetPoint = 0;
        }

        
        Vector2 nextDirection = patrolPoints[patrolTargetPoint] - new Vector2(transform.position.x, transform.position.y);

        nextDirection = new Vector2(nextDirection.x > 0 ? 1 : nextDirection.x < 0 ? -1 : 0, nextDirection.y > 0 ? 1 : nextDirection.y < 0 ? -1 : 0);


        if (nextDirection == Vector2.zero)
            patrolTargetPoint += 1 * patrolDirection;


        //Move one unit forward
        transform.position += new Vector3(nextDirection.x, nextDirection.y, 0);
    }
}
