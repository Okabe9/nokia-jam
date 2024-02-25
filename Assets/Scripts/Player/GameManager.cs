using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject lemming;

    [SerializeField] private int frozenSelectionAmmo = 2;

    private GameObject hoverBorderInstance;
    private List<GameObject> activatedBorderInstances = new List<GameObject>();

    [SerializeField]
    public List<Pallete> palletes = new List<Pallete>();

    public int currentPalleteIndex = 0;
    private int gridX = 0;
    private int gridY = 0;

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
        Vector2[,] borderPositionsGrid = GetGrid();
        // Instantiate the hoverBorderPrefab at the 0, 0position
        hoverBorderInstance = Instantiate(hoverBorderPrefab, borderPositionsGrid[0, 0], Quaternion.identity);
    }

    // SceneManagement


    #region Game

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
                selectionBorder.UnfreezeTimeStoppableEntities();

                frozenSelectionAmmo++;

                return false;
            }

        }

        if (frozenSelectionAmmo == 0)
            return false;

        GameObject activeBorderInstance = Instantiate(activeBorderPrefab, borderPosition, Quaternion.identity);
        activatedBorderInstances.Add(activeBorderInstance);
        selectionBorder.FreezeTimeStoppableEntities();

        frozenSelectionAmmo--;

        return true;

    }


    public void RestartLevel()
    {
        AudioManager.instance.PlaySFX("StartLevel");
        Instantiate(lemming, lemmingStartingPosition, Quaternion.identity);
    }

    #endregion
    private void MoveSelectionBorderUp()
    {
        gridY = (gridY + 2) % 3;
        UpdateBorderPosition(gridX, gridY);

    }
    private void MoveSelectionBorderLeft()
    {

        gridX = (gridX + 2) % 3;
        UpdateBorderPosition(gridX, gridY);

    }
    private void MoveSelectionBorderDown()
    {

        gridY = (gridY + 1) % 3;
        UpdateBorderPosition(gridX, gridY);

    }
    private void MoveSelectionBorderRight()
    {

        gridX = (gridX + 1) % 3;
        UpdateBorderPosition(gridX, gridY);

    }
    private void Friis()
    {
    FreezeSection(gridX, gridY);
    }
    private void FriisPlayer()
    {
        FreezeSection(gridX, gridY);
    }

}
