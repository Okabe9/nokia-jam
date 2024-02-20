using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP : MonoBehaviour
{
    [SerializeField] private GameObject targetTP;
    [HideInInspector] public bool isTPActive = true;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isTPActive)
        {
            targetTP.GetComponent<TP>().isTPActive = false;
            other.gameObject.transform.position = targetTP.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isTPActive)
            isTPActive = true;
    }
}
