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

    private bool initObjectState;
    private bool initObjectCollState;

    private void Start()
    {
        if (objectToAffect != null)
        {
             initObjectState = objectToAffect.activeSelf;
            isAffectedButtonActive = initObjectState;
        }

        if (objectToAffectCollider != null)
        {
            initObjectCollState = objectToAffectCollider.activeSelf;
            isAffectedButtonActive = initObjectCollState;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Killer") || collision.CompareTag("MovingPlatform"))
        {
            AudioManager.instance.PlaySFX("Blip3");

            gameObject.GetComponent<Animator>().SetTrigger("Activate");

            collisionCount++;

            if (isOneTimeUse)
                buttonActivated = true;

            if (isToggle && functionality == ButtonFunctionality.DESTROY)
            {
                if (objectToAffect != null)
                {
                    objectToAffect.SetActive(!isAffectedButtonActive);
                }

                if (objectToAffectCollider != null)
                {
                    objectToAffectCollider.SetActive(!isAffectedButtonActive);
                }

                isAffectedButtonActive = !isAffectedButtonActive;
            } else
            {
                if (objectToAffect != null)
                {
                    objectToAffect.SetActive(false);
                }

                if (objectToAffectCollider != null)
                {
                    objectToAffectCollider.SetActive(false);
                }
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

                        if (objectToAffect != null)
                        {
                            objectToAffect.SetActive(false);
                        }

                        if (objectToAffectCollider != null)
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
        if(objectToAffect != null)
            objectToAffect.SetActive(initObjectState);

        if(objectToAffectCollider != null)
            objectToAffectCollider.SetActive(initObjectCollState);

        if (objectToAffect != null)
        {
            initObjectState = objectToAffect.activeSelf;
            isAffectedButtonActive = initObjectState;
        }

        if (objectToAffectCollider != null)
        {
            initObjectCollState = objectToAffectCollider.activeSelf;
            isAffectedButtonActive = initObjectCollState;
        }
    }
}
