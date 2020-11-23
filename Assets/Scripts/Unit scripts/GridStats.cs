using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridStats : MonoBehaviour
{
    public int visited = -1;
    public int x = 0;
    public int y = 0;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool highlight = false;
    public enum objectType { NONE, ENEMY, UNIT, BASE }
    public List<GameObject> neighbours = new List<GameObject>();
    public objectType objectOnTileType;
    public Color currentColor;
    public GameObject currentObjectOnTile;
    
    // Start is called before the first frame update
    void Start()
    {
       // neighbours = new GameObject[4];
        currentColor = GetComponent<Renderer>().material.color;
        currentObjectOnTile = gameObject;
        getNeighbours(Vector3.forward);
        getNeighbours(-Vector3.forward);
        getNeighbours(Vector3.right);
        getNeighbours(-Vector3.right);
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
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if(selectable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if(highlight)
        {
            highlightNeighbours();
        }
        else
        { 
            GetComponent<Renderer>().material.color = currentColor;
        }

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
            obj.GetComponent<GridStats>().selectable = true;
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
        else if (currentObjectOnTile.CompareTag("Base"))
        {
            objectOnTileType = objectType.UNIT;
        }
        else
        {
            objectOnTileType = objectType.NONE;
        }
    }
}
