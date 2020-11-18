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
    public enum objectType { NONE, ENEMY, UNIT, BASE }
    public GameObject[] neighbours;
    public objectType objectOnTileType;
    public Color currentColor;
    public GameObject currentObjectOnTile;
      
    // Start is called before the first frame update
    void Start()
    {
        neighbours = new GameObject[4];
        currentColor = GetComponent<Renderer>().material.color;
        currentObjectOnTile = gameObject;
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
        else
        { 
            GetComponent<Renderer>().material.color = currentColor;
        }

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
