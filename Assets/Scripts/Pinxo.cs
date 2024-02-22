using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pinxo : TimeStoppableEntity
{
    float timeRemaining = 0f;
    int verticalMovement = -2;
    [SerializeField]
    float activationTime = 0.2f;

    [SerializeField] private bool dontHide = false;

    [SerializeField] private Sprite spikeUpSprite;
    [SerializeField] private Sprite spikeDownSprite;


    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = activationTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTimeStopped && !dontHide)
            EntityMovement();

    }

    private void EntityMovement()
    {
        if (timeRemaining > 0)
            timeRemaining -= Time.deltaTime;
        else
        {
            if (gameObject.GetComponent<SpriteRenderer>().sprite == spikeUpSprite)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = spikeDownSprite;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = spikeUpSprite;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }

            timeRemaining = activationTime;
        }
    }
}
