using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{

    [SerializeField] private float forceX = 500f;
    [SerializeField] private float forceY = 500f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            if (other.gameObject.transform.position.x - transform.position.x >= 0)
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX, forceY));
            else
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-forceX, forceY));
        }
    }

}
