using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
  int gridX = 0;
  int gridY = 0;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      GameManager.instance.FreezeSection(gridX, gridY);
    }
    if (Input.GetKeyDown(KeyCode.W))
    {
      gridY = (gridY + 2) % 3;
      UpdateBorderPosition();
    }
    if (Input.GetKeyDown(KeyCode.A))
    {
      gridX = (gridX + 2) % 3;
      UpdateBorderPosition();

    }
    if (Input.GetKeyDown(KeyCode.S))
    {
      gridY = (gridY + 1) % 3;
      UpdateBorderPosition();

    }
    if (Input.GetKeyDown(KeyCode.D))
    {
      gridX = (gridX + 1) % 3;
      UpdateBorderPosition();
    }
    if (Input.GetKeyDown(KeyCode.Z))
    {
      GameManager.instance.currentPalleteIndex = (GameManager.instance.currentPalleteIndex + 1) % 6;
      GameManager.instance.SpritePainting();
    }
  }
  void UpdateBorderPosition()
  {
    GameManager.instance.UpdateBorderPosition(gridX, gridY);
  }
}
