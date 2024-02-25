using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonFunctionality
{
    MOVETO,
    DESTROY,
    NONE,
}

public class Button : MonoBehaviour
{
    [SerializeField] private ButtonFunctionality functionality;
    [SerializeField] private GameObject objectToAffect;
    [SerializeField] private bool isOneTimeUse;

    private bool buttonActivated = false;
    private int collisionCount = 0;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        collisionCount++;

        if (isOneTimeUse)
            buttonActivated = true;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        buttonActivated = true;

        if(buttonActivated)
        {
            switch (functionality)
            {
                case ButtonFunctionality.MOVETO:

                    break;
                case ButtonFunctionality.DESTROY:
                    if(objectToAffect != null)
                        objectToAffect.SetActive(false);
                    break;
                case ButtonFunctionality.NONE:
                    break;
                default:
                    break;
            }
        }
        
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        collisionCount--;

        if (!isOneTimeUse && collisionCount == 0)
            buttonActivated = false;
    }

    public void RestartObjects()
    {
        objectToAffect.SetActive(true);
    }
}
