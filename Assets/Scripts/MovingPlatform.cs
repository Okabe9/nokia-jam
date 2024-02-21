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

    private void EntityMovement()
    {
        if (timeRemaining > 0)
            timeRemaining -= Time.deltaTime;
        else
        {
            MoveToNextPatrolPoint();
            timeRemaining = activationTime;
        }
    }

    private void MoveToNextPatrolPoint()
    {
        CheckPatrolBounds();

        Vector2 nextDirection = patrolPoints[patrolTargetPoint] - new Vector2(transform.position.x, transform.position.y);
        nextDirection = new Vector2(nextDirection.x > 0 ? 1 : nextDirection.x < 0 ? -1 : 0, nextDirection.y > 0 ? 1 : nextDirection.y < 0 ? -1 : 0);

        if (nextDirection == Vector2.zero)
            patrolTargetPoint += 1 * patrolDirection;

        CheckPatrolBounds();

        //Move one unit forward
        transform.position += new Vector3(nextDirection.x, nextDirection.y, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Killer") && !collision.gameObject.CompareTag("Player"))
        {
            patrolTargetPoint -= 1 * patrolDirection;
            patrolDirection *= -1;

            CheckPatrolBounds();

            Vector2 nextDirection = patrolPoints[patrolTargetPoint] - new Vector2(transform.position.x, transform.position.y);
            nextDirection = new Vector2(nextDirection.x > 0 ? 1 : nextDirection.x < 0 ? -1 : 0, nextDirection.y > 0 ? 1 : nextDirection.y < 0 ? -1 : 0);


            if (nextDirection == Vector2.zero)
                patrolTargetPoint += 1 * patrolDirection;
        }
    }

    private void CheckPatrolBounds()
    {
        // Loops patrol points
        if (patrolTargetPoint >= patrolPoints.Count)
        {
            if (pingPongMovement)
            {
                patrolTargetPoint -= 2 * patrolDirection;
                patrolDirection *= -1;
            }
            else
                patrolTargetPoint = 0;
        }
        else if (patrolTargetPoint < 0)
            patrolTargetPoint = patrolPoints.Count - 1;
    }
}
