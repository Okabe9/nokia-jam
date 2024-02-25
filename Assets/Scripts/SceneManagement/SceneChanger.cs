using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;

    // This method is called to change the scene
    [SerializeField]
    public List<Pallete> palletes = new List<Pallete>();
    public int currentPalleteIndex = 0;

    public GameObject pauseMenu;
    public EventSystem eventSystem;
    public GameObject exitButton;
    public GameObject menuButton;
    private GameObject firstSelectedPauseButton;
    GameObject gaijinEventSystem;
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
    private void Start()
    {
        gaijinEventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        menuButton.SetActive(false);
        exitButton.SetActive(true);
        SpritePainting();
        firstSelectedPauseButton = exitButton; 
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
        SceneManager.LoadScene(sceneName);
    }
    public void CloseGame()
    {
        Application.Quit();
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
                    go.GetComponent<SpriteRenderer>().color = palletes[currentPalleteIndex].foregroundColor;

                }
                else
                {
                    go.GetComponent<SpriteRenderer>().color = palletes[currentPalleteIndex].backgroundColor;
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
    public void ChangeColorPallete(int palleteIndex)
    {
        currentPalleteIndex = palleteIndex;
        SpritePainting();

    }

    private void OnLevelWasLoaded(int level)
    {

       if(level != 0)
        {
            menuButton.SetActive(true);
            exitButton.SetActive(false);
            firstSelectedPauseButton = menuButton;  
        }
        else
        {
            menuButton.SetActive(false);
            exitButton.SetActive(true);
            firstSelectedPauseButton = exitButton; 
        }
        gaijinEventSystem = GameObject.FindGameObjectWithTag("EventSystem");

        SpritePainting();

    }

    private void TogglePause()
    {

        if (pauseMenu.activeSelf)
        {
            gaijinEventSystem.SetActive(true);
            pauseMenu.SetActive(false);
        }

        else
        {
            gaijinEventSystem.SetActive(false);
            pauseMenu.SetActive(true);
        }
        SpritePainting();

    }


}