using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{

  public static InputManager instance;

  public bool OpenEscMenuInput { get; private set; }

  private PlayerInput _playerInput;

  private InputAction _openEscMenuAction;
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
    _playerInput = new PlayerInput();
    _openEscMenuAction = _playerInput.actions["OpenEscMenu"];
  }
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    OpenEscMenuInput = _openEscMenuAction.WasPressedThisFrame();
  }
}
