using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenArea : MonoBehaviour
{
    // List to store colliders completely inside the trigger
    private List<Collider2D> collidersInside = new List<Collider2D>();

    private void Start()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(gameObject.GetComponent<BoxCollider2D>().bounds.center, gameObject.GetComponent<BoxCollider2D>().bounds.size, 0f, LayerMask.GetMask("Default"));
        foreach (Collider2D collider in colliders)
        {
            if (!collidersInside.Contains(collider) && !collider.gameObject.CompareTag("Player"))
            {
                print(collider.gameObject.name);
                collidersInside.Add(collider);
                collider.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
    }
}
