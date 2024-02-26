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
    [SerializeField] private GameObject objectToAffectCollider;
    [SerializeField] private bool isOneTimeUse;
    [SerializeField] private bool isToggle;

    private bool isAffectedButtonActive = true;
    private bool buttonActivated = false;
    private int collisionCount = 0;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Killer") || collision.CompareTag("MovingPlatform"))
        {
            gameObject.GetComponent<Animator>().SetTrigger("Activate");

            collisionCount++;

            if (isOneTimeUse)
                buttonActivated = true;

            if (isToggle && functionality == ButtonFunctionality.DESTROY)
            {
                if (objectToAffect != null && objectToAffectCollider != null)
                {
                    objectToAffect.SetActive(!isAffectedButtonActive);
                    objectToAffectCollider.SetActive(!isAffectedButtonActive);
                }
                else if (objectToAffect != null)
                {
                    objectToAffect.SetActive(!isAffectedButtonActive);
                }
                else if (objectToAffectCollider != null)
                {
                    objectToAffectCollider.SetActive(!isAffectedButtonActive);
                }

                isAffectedButtonActive = !isAffectedButtonActive;
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Killer") || collision.CompareTag("MovingPlatform"))
        {
            buttonActivated = true;

            if (buttonActivated && !isOneTimeUse)
            {
                switch (functionality)
                {
                    case ButtonFunctionality.MOVETO:

                        break;
                    case ButtonFunctionality.DESTROY:

                        if (objectToAffect != null && objectToAffectCollider != null)
                        {
                            objectToAffect.SetActive(false);
                            objectToAffectCollider.SetActive(false);
                        }
                        else if (objectToAffect != null)
                        {
                            objectToAffect.SetActive(false);
                        }
                        else if (objectToAffectCollider != null)
                        {
                            objectToAffectCollider.SetActive(false);
                        }



                        break;
                    case ButtonFunctionality.NONE:
                        break;
                    default:
                        break;
                }
            }
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Killer") || collision.CompareTag("MovingPlatform"))
        {
            collisionCount--;

            if (!isOneTimeUse && collisionCount == 0)
                buttonActivated = false;
        }
    }

    public void RestartObjects()
    {
        objectToAffect.SetActive(true);
        objectToAffectCollider.SetActive(true);
    }
}
