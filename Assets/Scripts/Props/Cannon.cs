using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : TimeStoppableEntity
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float spawnTime = 0;
    [SerializeField] private BulletDirection direction;
    [SerializeField] private float bulletMoveTime;

    private float spawnTimer = 0;

    void Update()
    {
        if (spawnTimer > 0)
            spawnTimer -= Time.deltaTime;
        else
        {
            spawnTimer = spawnTime;

            Quaternion rotation = Quaternion.Euler(0, 0, 0);

            switch (direction)
            {
                case BulletDirection.UP:
                    rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case BulletDirection.DOWN:
                    rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case BulletDirection.LEFT:
                    rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case BulletDirection.RIGHT:
                    rotation = Quaternion.Euler(0, 0, 180);
                    break;
                default:
                    break;
            }

            if (isTimeStopped)
                return;

            GameObject spawnedBullet = GameObject.Instantiate(bullet, transform.position, rotation);
            spawnedBullet.transform.position = transform.position;


            spawnedBullet.GetComponent<Bullet>().direction = direction;
            spawnedBullet.GetComponent<Bullet>().bulletMoveTime = bulletMoveTime;
            PalleteController.instance.SpritePainting();

        }
    }

}
