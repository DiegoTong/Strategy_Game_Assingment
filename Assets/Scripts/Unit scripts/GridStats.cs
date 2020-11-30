using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridStats : MonoBehaviour
{
    public int x = 0;
    public int y = 0;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool highlight = false;
    public bool isVisible = false;
    public GameObject darkness;
    public float gCost;
    public float hCost;
    public float fCost;
    public GameObject prevousGrid;
    public bool visited = false;
    public bool visionVisited = false;
    public GridStats parent = null;
    public GridStats visionParent = null;
    public int distance = 0;
    public int visionDistance = 0;
    public enum objectType { NONE, ENEMY, UNIT, BASE }
    public List<GameObject> neighbours = new List<GameObject>();
    public objectType objectOnTileType;
    public Color currentColor;
    public GameObject currentObjectOnTile;
    public List<GridStats> adjlist = new List<GridStats>();
    
    // Start is called before the first frame update
    void Start()
    {
        darkness.SetActive(false);
        // neighbours = new GameObject[4];
        currentColor = GetComponent<Renderer>().material.color;
        currentObjectOnTile = gameObject;
        getNeighbours(Vector3.forward);
        getNeighbours(-Vector3.forward);
        getNeighbours(Vector3.right);
        getNeighbours(-Vector3.right);
        gCost = 0;
        hCost = 0;
        fCost = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (current)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if(target)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if(selectable)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if(highlight)
        {
            highlightNeighbours();
        }
        else
        { 
            GetComponent<Renderer>().material.color = currentColor;
        }
        if(isVisible ==true)
        {
            darkness.SetActive(false);
        }
        else
        {
            darkness.SetActive(true);
        }
    }

    public void findNeighbours()
    {
        clear();
        clearVision();
        getNeighbours(Vector3.forward);
        getNeighbours(-Vector3.forward);
        getNeighbours(Vector3.right);
        getNeighbours(-Vector3.right);
    }

    public void getNeighbours(Vector3 direction)
    {
        Vector3 halfExtends = new Vector3(0.25f,0.25f,0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position+direction,halfExtends);
        foreach(Collider item in colliders)
        {
            GameObject gridStats = item.gameObject;
            if(gridStats != null && gridStats.name == "Grid Tile(Clone)")
            {
                neighbours.Add(item.gameObject);
                if(item.gameObject.GetComponent<GridStats>().objectOnTileType == objectType.NONE)
                {
                    adjlist.Add(item.gameObject.GetComponent<GridStats>());
                }
            }
        }  
    }
    public void findNeighbours(GridStats targetT)
    {
        clear();
        getNeighbours(Vector3.forward, targetT);
        getNeighbours(-Vector3.forward, targetT);
        getNeighbours(Vector3.right, targetT);
        getNeighbours(-Vector3.right, targetT);
    }
    public void getNeighbours(Vector3 direction, GridStats enemyTarget)
    {
        Vector3 halfExtends = new Vector3(0.25f, 0.25f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtends);
        foreach (Collider item in colliders)
        {
            GameObject gridStats = item.gameObject;
            if (gridStats != null && gridStats.name == "Grid Tile(Clone)")
            {
                if (item.gameObject.GetComponent<GridStats>().objectOnTileType == objectType.NONE || enemyTarget == item.gameObject.GetComponent<GridStats>())
                {
                    adjlist.Add(item.gameObject.GetComponent<GridStats>());
                }
            }
        }
    }
    public void highlightNeighbours()
    {
        //for (int i = 0; i < 4; i++)
        //{
        //    neighbours[i].GetComponent<Renderer>().material.color = Color.magenta;
        //}
        foreach(GameObject obj in neighbours)
        {
           // obj.GetComponent<GridStats>().selectable = true;
        }
        // neighbours[0].GetComponent<Renderer>().material.color = Color.green;
        // neighbours[1].GetComponent<Renderer>().material.color = Color.green;
        //neighbours[2].GetComponent<Renderer>().material.color = Color.magenta;
        //neighbours[3].GetComponent<Renderer>().material.color = Color.magenta;
    }
    private void OnTriggerEnter(Collider other)
    {
        currentObjectOnTile = other.gameObject;
        checkType();
    }
    private void OnTriggerExit(Collider other)
    {
        currentObjectOnTile = gameObject;
        objectOnTileType = objectType.NONE;
    }
    public void clear()
    {
        adjlist.Clear();
        current = false;
        selectable = false;
        target = false;
        visited = false;    
        parent = null;
        distance = 0;
        gCost = 0;
        hCost = 0;
        fCost = 0;
    }
    public void clearVision()
    {
        neighbours.Clear();
        visionVisited = false;
        visionParent = null;
        visionDistance = 0;
    }

    private void checkType()
    {
        if (currentObjectOnTile.CompareTag("Enemy"))
        {
            objectOnTileType = objectType.ENEMY;
        }
        else if (currentObjectOnTile.CompareTag("Friendly"))
        {
            objectOnTileType = objectType.UNIT;
        }
        else if (currentObjectOnTile.CompareTag("Base") || currentObjectOnTile.CompareTag("Tile"))
        {
            objectOnTileType = objectType.UNIT;
        }
        else
        {
            objectOnTileType = objectType.NONE;
        }
    }
}
