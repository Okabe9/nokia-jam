using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBorder : MonoBehaviour
{
  // List to store colliders completely inside the trigger
  private List<Collider2D> collidersInside = new List<Collider2D>();

  // Method called when a collider stays inside the trigger collider
  private void OnTriggerEnter2D(Collider2D other)
  {
    // Check if the collider is not already in the list
    if (!collidersInside.Contains(other))
    {
      collidersInside.Add(other);
      Debug.Log(other.name + " is completely inside the trigger.");
      // Check if the bounds of the collider are completely inside the trigger collider
      if (IsColliderCompletelyInside(other))
      {
        // Add the collider to the list

      }
    }
  }

  // Method called when a collider exits the trigger collider
  private void OnTriggerExit2D(Collider2D other)
  {
    // Check if the collider is in the list
    if (collidersInside.Contains(other))
    {
      // Remove the collider from the list
      collidersInside.Remove(other);
      Debug.Log(other.name + " exited the trigger.");
    }
  }

  // Method to check if a collider is completely inside the trigger collider
  private bool IsColliderCompletelyInside(Collider2D collider)
  {
    Collider2D triggerCollider = GetComponent<Collider2D>();

    Bounds triggerBounds = triggerCollider.bounds;
    Bounds colliderBounds = collider.bounds;

    // Check if the bounds of the collider are completely inside the trigger collider
    return triggerBounds.Contains(colliderBounds.min) && triggerBounds.Contains(colliderBounds.max);
  }
  public void FreezeTimeStoppableEntities()
  {
    // Iterate through the list of colliders
    foreach (Collider2D collider in collidersInside)
    {
      // Check if the collider has a TimeStoppableEntity component
      TimeStoppableEntity timeStoppableEntity = collider.GetComponent<TimeStoppableEntity>();
      if (timeStoppableEntity != null)
      {
        // Freeze the time of the TimeStoppableEntity
        timeStoppableEntity.ToggleTime();
      }
    }
  }
}
