using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP : MonoBehaviour
{
    [SerializeField] private Vector2 targetPos;
    [SerializeField] private GameObject targetTP;
    [HideInInspector] public bool isTPActive = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Killer")) && isTPActive)
        {
            targetTP.GetComponent<TP>().isTPActive = false;
            other.transform.position = new Vector3(targetPos.x, targetPos.y, 0);
            other.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Killer")) && !isTPActive)
            isTPActive = true;
    }
}
