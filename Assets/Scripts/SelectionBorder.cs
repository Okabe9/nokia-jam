using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBorder : MonoBehaviour
{
  // List to store colliders completely inside the trigger
  private List<Collider2D> collidersInside = new List<Collider2D>();

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (!collidersInside.Contains(other) && !other.gameObject.CompareTag("Player"))
    {
      collidersInside.Add(other);

      if (IsColliderCompletelyInside(other))
      {
                //do something
      }
    }
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (collidersInside.Contains(other))
    {
      collidersInside.Remove(other);
    }
  }


    
  private bool IsColliderCompletelyInside(Collider2D collider)
  {
    Collider2D triggerCollider = GetComponent<Collider2D>();

    Bounds triggerBounds = triggerCollider.bounds;
    Bounds colliderBounds = collider.bounds;

    return triggerBounds.Contains(colliderBounds.min) && triggerBounds.Contains(colliderBounds.max);
  }

  public void FreezeTimeStoppableEntities()
  {
    foreach (Collider2D collider in collidersInside)
    {
      TimeStoppableEntity timeStoppableEntity = collider.GetComponent<TimeStoppableEntity>();

      if (timeStoppableEntity != null)
        timeStoppableEntity.ToggleTime();
    }
  }
}
