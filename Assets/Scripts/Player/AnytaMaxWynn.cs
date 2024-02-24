using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnytaMaxWynn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            gameObject.GetComponent<Animator>().SetTrigger("Activate");

            // On Anim End Call Win
        }
    }
}
