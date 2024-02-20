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

  private GameObject hoverBorderInstance;
  private List<GameObject> activatedBorderInstances = new List<GameObject>();


  // Singleton pattern
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

  // Update is called once per frame
  void Update()
  {

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
    Vector2[,] grid = GetGrid();
    Vector2 borderPosition = grid[gridY, gridX];
    foreach (GameObject border in activatedBorderInstances)
    {
      Vector2 activeBorderPosition = new Vector2(border.transform.position.x, border.transform.position.y);
      if (borderPosition == activeBorderPosition)
      {
        activatedBorderInstances.Remove(border);
        Destroy(border);
        print("Unfreezed Section" + gridX + " " + gridY);

        return false;
      }

    }
    GameObject activeBorderInstance = Instantiate(activeBorderPrefab, borderPosition, Quaternion.identity);
    activatedBorderInstances.Add(activeBorderInstance);
    print("Freezed Section" + gridX + " " + gridY);
    return true;

  }
}
