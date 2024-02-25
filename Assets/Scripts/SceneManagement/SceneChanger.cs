using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;

    public GameObject pauseMenu;
    public EventSystem eventSystem;
    GameObject gaijinEventSystem;
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
    private void Start()
    {
        gaijinEventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        PalleteController.instance.SpritePainting();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause(); 
        }
    }

    public void ChangeScene(string sceneName)
    {
        TogglePause(true);
        SceneManager.LoadScene(sceneName);
    }
    public void CloseGame()
    {
        TogglePause();
    }
 
    public void ChangeColorPallete(int palleteIndex)
    {
        PalleteController.instance.currentPalleteIndex = palleteIndex;
        PalleteController.instance.SpritePainting();

    }


    private void TogglePause(bool forceClose = false)
    {
        if (pauseMenu.activeSelf || forceClose)
        {
            gaijinEventSystem.SetActive(true);
            pauseMenu.SetActive(false);
        }

        else
        {
            gaijinEventSystem.SetActive(false);
            pauseMenu.SetActive(true);
        }
        PalleteController.instance.SpritePainting();
    }


}