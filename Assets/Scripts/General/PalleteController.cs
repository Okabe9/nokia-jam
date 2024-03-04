using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PalleteController : MonoBehaviour
{
    public static PalleteController instance;

    // This method is called to change the scene
    [SerializeField]
    public List<Pallete> palletes = new List<Pallete>();
    public int currentPalleteIndex = 0;

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

    public void SpritePainting()
    {
    }
}
