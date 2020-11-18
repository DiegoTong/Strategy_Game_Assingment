using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public int health;
    public int attack;
    public int movement;
    public GameManager gameManager_script;
    public GameObject movement_Tile_Prefab;
    public bool hasMoved;
    public bool hasActed;
    public bool isSelected;
    public int id;
    public SpawnManager spawnManager_script;
    public string unitName;
    public GameObject gridTile;
    public bool hasValidTarget;
    // Start is called before the first frame update
    void Start()
    {
        unitName = gameObject.name;
        gameManager_script = GameObject.Find("Game Manager").GetComponent<GameManager>();
        spawnManager_script = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        id = gameManager_script.number_of_builders;
        InstantiateTiles(movement);
        hasMoved = false;
        hasActed = false;

    }

    // Update is called once per frame
    void Update()
    {
    //    CheckForTargets();
    }

    public void disableMovement()
    {
        if(hasMoved == true)
        {
            foreach (Transform child in transform)
            {
                //child.gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
               // child.gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
                child.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gold"))
        {
            gameManager_script.resources++;
            Destroy(other.gameObject);
        }
        if(other.gameObject.name == "Grid Tile(Clone)")
        {
            gridTile = other.gameObject;
        }
    }
    private void CheckForTargets()
    {
        for(int i =0; i<4;i++)
        {
            if (gridTile.GetComponent<GridStats>().neighbours[i].GetComponent<GridStats>().objectOnTileType == GridStats.objectType.ENEMY)
            {
                hasValidTarget = true;
            }        
            else if(gridTile.GetComponent<GridStats>().neighbours[i].GetComponent<GridStats>().objectOnTileType == GridStats.objectType.UNIT)
            {
                hasValidTarget = false;
            }
            else if (gridTile.GetComponent<GridStats>().neighbours[i].GetComponent<GridStats>().objectOnTileType == GridStats.objectType.BASE)
            {
                hasValidTarget = false;
            }
            else
            {
                hasValidTarget = false;
            }
        }
        
    }
    private void InstantiateTiles(int numTiles)
    {
      
        for (int i = 0; i < numTiles; i++)
        {
            int p = -1;
            for (int j = 0; j < 2; j++)
            {
                Instantiate(movement_Tile_Prefab, gameObject.transform.position + new Vector3(p-i, -0.5f, 0), Quaternion.Euler(90.0f,0.0f,0.0f),gameObject.transform);
                Instantiate(movement_Tile_Prefab, gameObject.transform.position + new Vector3(0, -0.5f, p-i), Quaternion.Euler(90.0f, 0.0f, 0.0f), gameObject.transform);
            }
            p = 1;
            for (int j = 0; j < 2; j++)
            {
                Instantiate(movement_Tile_Prefab, gameObject.transform.position + new Vector3(p + i, -0.5f, 0), Quaternion.Euler(90.0f, 0.0f, 0.0f), gameObject.transform);
                Instantiate(movement_Tile_Prefab, gameObject.transform.position + new Vector3(0, -0.5f, p + i), Quaternion.Euler(90.0f, 0.0f, 0.0f), gameObject.transform);
            }
        }
    }
}
