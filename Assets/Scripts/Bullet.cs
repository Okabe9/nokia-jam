using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}


public class Bullet : MonoBehaviour
{

    [HideInInspector] public BulletDirection direction;
    [HideInInspector] public float bulletMoveTime;

    private float bulletMoveTimer = 0;
    

    // Update is called once per frame
    void Update()
    {
        if(bulletMoveTimer > 0)
            bulletMoveTimer -= Time.deltaTime;
        else
        {
            //Move one unit forward
            switch(direction)
            {
                case BulletDirection.UP:
                    transform.position += new Vector3(0, 1, 0);
                    break;
                case BulletDirection.DOWN:
                    transform.position += new Vector3(0, -1, 0);
                    break;
                case BulletDirection.RIGHT:
                    transform.position += new Vector3(1, 0, 0);
                    break;
                case BulletDirection.LEFT:
                    transform.position += new Vector3(-1, 0, 0);
                    break;
            }

            bulletMoveTimer = bulletMoveTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<LemmingController>().Death();

        Destroy(this.gameObject);
    }
}
