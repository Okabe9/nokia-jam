using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenArea : MonoBehaviour
{
    // List to store colliders completely inside the trigger
    private List<Collider2D> collidersInside = new List<Collider2D>();
    private List<Collider2D> collidersInBack = new List<Collider2D>();
    private List<string> sortingLayersNames = new List<string> ();

    private Dictionary<string, int> layerIDs = new Dictionary<string, int>();

    private void Start()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(gameObject.GetComponent<BoxCollider2D>().bounds.center, gameObject.GetComponent<BoxCollider2D>().bounds.size, 0f, LayerMask.GetMask("Default", "Ground", "Wall"));

        foreach (Collider2D collider in colliders)
        {
            // Render Layer Handling
            if (!collidersInside.Contains(collider) && !collider.gameObject.CompareTag("Player"))
            {
                collidersInside.Add(collider);
                sortingLayersNames.Add(collider.gameObject.GetComponent<SpriteRenderer>().sortingLayerName);
                collider.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FrozenPlane";
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        TimeStoppableEntity otherTStoppable = collider.gameObject.GetComponent<TimeStoppableEntity>();

        // Collider Layer Handling
        if (collider.gameObject.layer == LayerMask.NameToLayer("BehindFrozenPlane"))
            otherTStoppable.isTransitioningBehindFreeze  = true;

        if (!collidersInside.Contains(collider))
        {
            if (!layerIDs.ContainsKey(collider.gameObject.name))
                layerIDs.Add(collider.gameObject.name, otherTStoppable.originalLayer);

            if (!collidersInBack.Contains(collider))
                collidersInBack.Add(collider);

            collider.gameObject.layer = LayerMask.NameToLayer("BehindFrozenPlane");
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        TimeStoppableEntity otherTStoppable = collider.gameObject.GetComponent<TimeStoppableEntity>();

        // Collider Layer Handling
        if (layerIDs.ContainsKey(collider.gameObject.name) && !otherTStoppable.isTransitioningBehindFreeze)
        {
            collider.gameObject.layer = layerIDs[collider.gameObject.name];
            layerIDs.Remove(collider.gameObject.name);
        }

        if (collidersInBack.Contains(collider))
            collidersInBack.Remove(collider);

        if (collider.gameObject.layer == LayerMask.NameToLayer("BehindFrozenPlane"))
            otherTStoppable.isTransitioningBehindFreeze = false;
    }

    private void OnDestroy()
    {
        // Collider Layer Handling
        foreach (Collider2D collider in collidersInBack)
        {
            TimeStoppableEntity otherTStoppable = collider.gameObject.GetComponent<TimeStoppableEntity>();

            if (layerIDs.ContainsKey(collider.gameObject.name) && !otherTStoppable.isTransitioningBehindFreeze)
                collider.gameObject.layer = layerIDs[collider.gameObject.name];
        }

        // Render Layer Handling
        int i = 0;
        foreach (Collider2D collider in collidersInside)
        {
            collider.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayersNames[i];
            i++;
        }
    }
}
