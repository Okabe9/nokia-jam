using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Create a 3x3 grid of Vector2 accesible from editor
    [SerializeField] Vector2[] borderPositionsArray;
    [SerializeField] Vector2[,] borderPositionsGrid;

    [SerializeField] GameObject hoverBorderPrefab;
    [SerializeField] GameObject activeBorderPrefab;

    [SerializeField] Vector2 lemmingStartingPosition = new Vector2(0, 0);
    [SerializeField] public GameObject lemming;
    private GameObject currentLemming;

    [SerializeField] private int frozenSelectionAmmo = 2;

    private GameObject hoverBorderInstance;
    private List<GameObject> activatedBorderInstances = new List<GameObject>();

    [SerializeField]
    public List<Pallete> palletes = new List<Pallete>();

    public int currentPalleteIndex = 0;
    private int gridX = 0;
    private int gridY = 0;
    private bool levelStarted = false;

    //Mover a un nuevo script HUD
    public GameObject CooldownPrefab;
    public GameObject FreezeChargesPrefab;
    public GameObject CooldownInstance;
    private GameObject FreezeChargesInstance1;
    private GameObject FreezeChargesInstance2;
    public GameObject sectionShader;
    private GameObject[,] sectionShadersGrid = new GameObject[3, 3];
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


    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.StopMusic();

    }

    // SceneManagement

    void Update()
    {
        if (levelStarted)
        {
            switch (frozenSelectionAmmo)
            {
                case 0:
                    FreezeChargesInstance1.SetActive(false);
                    FreezeChargesInstance2.SetActive(false);
                    break;
                case 1:
                    FreezeChargesInstance1.SetActive(false);
                    FreezeChargesInstance2.SetActive(true);
                    break;
                case 2:
                    FreezeChargesInstance1.SetActive(true);
                    FreezeChargesInstance2.SetActive(true);
                    break;

            }
        }


    }
    #region Game

    public void StartLevel()
    {
        Vector2[,] borderPositionsGrid = GetGrid();
        InstantiateShaders(borderPositionsGrid);
        hoverBorderInstance = Instantiate(hoverBorderPrefab, borderPositionsGrid[0, 0], Quaternion.identity);
        CooldownInstance = Instantiate(CooldownPrefab, new Vector2(-37, -13), Quaternion.identity);
        FreezeChargesInstance1 = Instantiate(FreezeChargesPrefab, new Vector2(-40, -13), Quaternion.identity);
        FreezeChargesInstance2 = Instantiate(FreezeChargesPrefab, new Vector2(-40, -18), Quaternion.identity);
        levelStarted = true;

        currentLemming = Instantiate(lemming, lemmingStartingPosition, Quaternion.identity);
        currentLemming.GetComponent<Animator>().SetBool("isFirstTime", true);
        PalleteController.instance.SpritePainting();
    }
    public Vector2[,] GetGrid()
    {
        Vector2[,] newArray = new Vector2[3, 3];

        // Populate the 2D array with elements from the 1D array
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                newArray[i, j] = borderPositionsArray[i * 3 + j];
            }
        }

        return newArray;
    }
    public void UpdateBorderPosition(int gridX, int gridY)
    {
        Vector2[,] grid = GetGrid();
        Vector2 borderPosition = grid[gridY, gridX];
        hoverBorderInstance.transform.position = borderPosition;

    }
    public bool FreezeSection(int gridX, int gridY)
    {
        //Get the selection border class from hoverborderinstance
        SelectionBorder selectionBorder = hoverBorderInstance.GetComponent<SelectionBorder>();
        Vector2[,] grid = GetGrid();
        Vector2 borderPosition = grid[gridY, gridX];
        foreach (GameObject border in activatedBorderInstances)
        {

            Vector2 activeBorderPosition = new Vector2(border.transform.position.x, border.transform.position.y);
            if (borderPosition == activeBorderPosition)
            {
                activatedBorderInstances.Remove(border);
                Destroy(border);
                sectionShadersGrid[gridY, gridX].GetComponent<SpriteRenderer>().enabled = false;
                selectionBorder.UnfreezeTimeStoppableEntities();

                frozenSelectionAmmo++;

                return false;
            }

        }

        if (frozenSelectionAmmo == 0)
            return false;

        GameObject activeBorderInstance = Instantiate(activeBorderPrefab, borderPosition, Quaternion.identity);
        activatedBorderInstances.Add(activeBorderInstance);
        sectionShadersGrid[gridY, gridX].GetComponent<SpriteRenderer>().enabled = true;

        selectionBorder.FreezeTimeStoppableEntities();

        frozenSelectionAmmo--;

        return true;

    }


    public void RestartLevel()
    {
        currentLemming.GetComponent<Animator>().SetBool("isFirstTime", false);
        currentLemming = Instantiate(lemming, lemmingStartingPosition, Quaternion.identity);

        GameObject[] buttonsInScene = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject button in buttonsInScene)
        {
            button.GetComponent<Button>().RestartObjects();
        }
        AudioManager.instance.PlaySFX("StartLevel");

    }

    #endregion
    public void MoveSelectionBorderUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            gridY = (gridY + 2) % 3;
            UpdateBorderPosition(gridX, gridY);
        }

    }
    public void MoveSelectionBorderLeft(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            gridX = (gridX + 2) % 3;
            UpdateBorderPosition(gridX, gridY);
        }
    }
    public void MoveSelectionBorderDown(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            gridY = (gridY + 1) % 3;
            UpdateBorderPosition(gridX, gridY);
        }
    }
    public void MoveSelectionBorderRight(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            gridX = (gridX + 1) % 3;
            UpdateBorderPosition(gridX, gridY);
        }
    }
    public void Friis(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FreezeSection(gridX, gridY);
        }
    }

    public void FriisSelf(InputAction.CallbackContext context)
    {
        if (context.performed)

            currentLemming.GetComponent<LemmingController>().FriisSelf();
    }
    public void InstantiateShaders(Vector2[,] positionsGrid)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                print(sectionShader + " " + positionsGrid);
                GameObject shaderSection = Instantiate(sectionShader, positionsGrid[i, j], Quaternion.identity);
                shaderSection.GetComponent<SpriteRenderer>().enabled = false;
                sectionShadersGrid[i, j] = shaderSection;
            }
        }
    }


}
