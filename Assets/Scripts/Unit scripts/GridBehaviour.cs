using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    public int rows = 0;
    public int columns = 0;
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    public GameObject[,] gridArray;
    public int startX = 0;
    public int startY = 0;
    public int endx = 2;
    public int endy = 2;

    // Start is called before the first frame update
    private void Awake()
    {
        gridArray = new GameObject[columns, rows];
        GenerateGrid();
    }
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

    }
    void GenerateGrid()
    {
        for(int i =0;i<columns;i++)
        {
            for(int j =0; j<rows;j++)
            {
                GameObject gridTile = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i, 0.4f, leftBottomLocation.z + scale * j), gridPrefab.transform.rotation);
                gridTile.transform.SetParent(gameObject.transform);
                gridTile.GetComponent<GridStats>().visited = false;
                gridTile.GetComponent<GridStats>().x = i;
                gridTile.GetComponent<GridStats>().y = j;
                gridArray[i, j] = gridTile;
            }
        }
    }
}
