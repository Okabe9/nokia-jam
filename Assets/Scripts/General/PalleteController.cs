using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PalleteController : MonoBehaviour
{
  public static PalleteController instance;

  // This method is called to change the scene
  [SerializeField]
  public List<Pallete> palletes = new List<Pallete>();
  public int currentPalleteIndex = 0;

  private void Awake()
  {
    DontDestroyOnLoad(gameObject);

    if (instance == null)
    {
      instance = this;
    }
    else if (instance != this)
    {
      Destroy(gameObject);
    }
  }

  public void SpritePainting()
  {
    GameObject[] gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
    Camera mainCamera = Camera.main;
    if (mainCamera != null)
    {
      mainCamera.backgroundColor = palletes[currentPalleteIndex].backgroundColor;
    }
    foreach (GameObject go in gameObjects)
    {
      SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
      Image image = go.GetComponent<Image>();

      if (go.tag == "ColorChange")
      {
        Material material = spriteRenderer.material;
        material.SetColor("_ForegroundColor", palletes[currentPalleteIndex].foregroundColor);
        material.SetColor("_BackgroundColor", palletes[currentPalleteIndex].backgroundColor);

      }
      else if (spriteRenderer != null && go.tag != "ColorChange")
      {
        if (go.tag != "Background")
        {
          if (spriteRenderer.sortingLayerName != "FrozenPlane")
          {
            go.GetComponent<SpriteRenderer>().color = palletes[currentPalleteIndex].foregroundColor;
          }
          else
          {
            go.GetComponent<SpriteRenderer>().color = palletes[currentPalleteIndex].backgroundColor;

          }

        }
        else
        {
          if (spriteRenderer.sortingLayerName != "FrozenPlane")
          {
            go.GetComponent<SpriteRenderer>().color = palletes[currentPalleteIndex].backgroundColor;
          }
          else
          {
            go.GetComponent<SpriteRenderer>().color = palletes[currentPalleteIndex].foregroundColor;

          }
        }
      }
      else if (image != null)
      {
        if (go.tag != "Background")
        {
          image.color = palletes[currentPalleteIndex].foregroundColor;

        }
        else
        {
          image.color = palletes[currentPalleteIndex].backgroundColor;
        }
      }
    }
  }
}
