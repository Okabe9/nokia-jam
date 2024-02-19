using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pinxo : TimeStoppableEntity
{
  float timeRemaining = 0f;
  int verticalMovement = -2;
  [SerializeField]
  float activationTime = 0.2f;
  // Start is called before the first frame update
  void Start()
  {
    timeRemaining = activationTime;

  }

  // Update is called once per frame
  void Update()
  {
    if (!isTimeStopped)
    {

    }

  }
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      Debug.Log("Player has been hit by Pinxo");
      other.gameObject.GetComponent<LemmingController>().Death();
    }
  }
  private void EntityMovement()
  {
    if (timeRemaining > 0)
    {
      Debug.Log(timeRemaining);

      timeRemaining -= Time.deltaTime;
    }
    else
    {
      //Move spike vertically
      Debug.Log("Moving spike");
      this.transform.position += new Vector3(0, verticalMovement, 0);
      verticalMovement *= -1;
      if (verticalMovement == 2)
      {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
      }
      else
      {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
      }
      timeRemaining = activationTime;
    }
  }
}
