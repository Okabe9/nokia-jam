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


}
