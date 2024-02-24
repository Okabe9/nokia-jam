using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{

  public static InputManager instance;



  // Start is called before the first frame update

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else if (instance != this)
    {
      Destroy(gameObject);
    }

  }
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
  }
}
