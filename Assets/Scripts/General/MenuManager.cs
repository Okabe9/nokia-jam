using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
  [SerializeField] private GameObject _mainMenuCanvas;
  [SerializeField] private GameObject _optionsCanvas;
  /*   [SerializeField] private GameObject _levelSelectCanvas;
   */
  public static MenuManager instance;
  public bool isPaused = false;
  // Start is called before the first frame update
  void Start()
  {
    _mainMenuCanvas.SetActive(true);
    _optionsCanvas.SetActive(false);
    /*     _levelSelectCanvas.SetActive(false);
     */
  }

  // Update is called once per frame
  void Update()
  {
    if (InputManager.instance.OpenEscMenuInput)
    {
      if (!isPaused)
      {
        Pause();
      }
      else
      {
        Unpause();
      }
    }
  }
  public void Pause()
  {
    isPaused = true;
    Time.timeScale = 0f;

    OpenMainMenu();
  }
  public void Unpause()
  {
    isPaused = false;
    Time.timeScale = 1f;
    CloseAllMenus();
  }
  public void OpenMainMenu()
  {
    _mainMenuCanvas.SetActive(true);
  }
  public void CloseAllMenus()
  {
    _mainMenuCanvas.SetActive(false);
    _optionsCanvas.SetActive(false);
    /*     _levelSelectCanvas.SetActive(false);
     */
  }

}
