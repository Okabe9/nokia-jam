using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBorder : MonoBehaviour
{
    Collider2D[] collidersInside;
    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(gameObject.GetComponent<BoxCollider2D>().bounds.center, gameObject.GetComponent<BoxCollider2D>().bounds.size, 0f, LayerMask.GetMask("ActiveBorder"));


        if (colliders.Length > 0)
            gameObject.GetComponent<SpriteRenderer>().color = GameManager.instance.palletes[GameManager.instance.currentPalleteIndex].backgroundColor;
        else
            gameObject.GetComponent<SpriteRenderer>().color = GameManager.instance.palletes[GameManager.instance.currentPalleteIndex].foregroundColor;
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
        collidersInside = Physics2D.OverlapBoxAll(gameObject.GetComponent<BoxCollider2D>().bounds.center, gameObject.GetComponent<BoxCollider2D>().bounds.size, 0f, LayerMask.GetMask("Default", "Ground", "Wall"));

        foreach (Collider2D collider in collidersInside)
        {
            if (!IsColliderCompletelyInside(collider))
                continue;

            TimeStoppableEntity timeStoppableEntity = collider.GetComponent<TimeStoppableEntity>();

            if (timeStoppableEntity != null)
                timeStoppableEntity.StopTime();

        }
        PaintOnFreeze();

    }
    public void UnfreezeTimeStoppableEntities()
    {
        collidersInside = Physics2D.OverlapBoxAll(gameObject.GetComponent<BoxCollider2D>().bounds.center, gameObject.GetComponent<BoxCollider2D>().bounds.size, 0f, LayerMask.GetMask("Default", "Ground", "Wall"));

        foreach (Collider2D collider in collidersInside)
        {
            TimeStoppableEntity timeStoppableEntity = collider.GetComponent<TimeStoppableEntity>();

            if (timeStoppableEntity != null)
                timeStoppableEntity.StartTime();
        }
        PaintOnUnfreeze();

    }
    public void PaintOnFreeze()
    {
        collidersInside = Physics2D.OverlapBoxAll(gameObject.GetComponent<BoxCollider2D>().bounds.center, gameObject.GetComponent<BoxCollider2D>().bounds.size, 0f, LayerMask.GetMask("Default", "Ground", "Wall"));


        foreach (Collider2D collider in collidersInside)
        {
            if (!IsColliderCompletelyInside(collider))
                continue;

            if (!collider.gameObject.CompareTag("Player"))
            {

                if (collider.gameObject.tag != "Background" && collider.gameObject.layer != 6 && collider.gameObject.layer != 7)
                {
                    if (collider.gameObject.layer != 6 && collider.gameObject.layer != 7)
                    {
                        collider.gameObject.GetComponent<SpriteRenderer>().color = GameManager.instance.palletes[GameManager.instance.currentPalleteIndex].backgroundColor;

                    }
                }
                else
                    collider.gameObject.GetComponent<SpriteRenderer>().color = GameManager.instance.palletes[GameManager.instance.currentPalleteIndex].foregroundColor;

            }
        }
    }
    public void PaintOnUnfreeze()
    {
        collidersInside = Physics2D.OverlapBoxAll(gameObject.GetComponent<BoxCollider2D>().bounds.center, gameObject.GetComponent<BoxCollider2D>().bounds.size, 0f, LayerMask.GetMask("Default", "Ground", "Wall"));

        foreach (Collider2D collider in collidersInside)
        {
            if (!collider.gameObject.CompareTag("Player"))
            {

                if (collider.gameObject.tag != "Background")
                {
                    if (collider.gameObject.layer != 6 && collider.gameObject.layer != 7)
                    {
                        collider.gameObject.GetComponent<SpriteRenderer>().color = GameManager.instance.palletes[GameManager.instance.currentPalleteIndex].foregroundColor;

                    }
                }

                else
                    collider.gameObject.GetComponent<SpriteRenderer>().color = GameManager.instance.palletes[GameManager.instance.currentPalleteIndex].backgroundColor;

            }
        }
    }
}
