using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float spawnTime = 0;
    [SerializeField] private BulletDirection direction;
    [SerializeField] private float bulletMoveTime;

    private float spawnTimer = 0;

    // Update is called once per frame
    void Update()
    {
        if(spawnTimer > 0)
            spawnTimer -= Time.deltaTime;
        else
        {
            spawnTimer = spawnTime;
            GameObject spawnedBullet = GameObject.Instantiate(bullet);
            spawnedBullet.transform.position = transform.position;
            spawnedBullet.GetComponent<Bullet>().direction = direction;
            spawnedBullet.GetComponent<Bullet>().bulletMoveTime = bulletMoveTime;
        }
    }
    
}
