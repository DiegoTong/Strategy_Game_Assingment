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
        CheckForTargets();
        destroyUnit();
    }
    public void destroyUnit()
    {
        if(health <=0)
        {
            Destroy(gameObject);
        }
    }
    public void disableMovement()
    {
        if(hasMoved == true)
        {
            foreach (Transform child in transform)
            {
                //child.gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
                if(child.gameObject.tag == "Tile")
                {
                    child.gameObject.SetActive(false);
                }
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
        if (other.gameObject.CompareTag("Gold") && this.gameObject.tag == "Friendly")
        {
            gameManager_script.resources++;
            Destroy(other.gameObject);
        }
        if(other.gameObject.name == "Grid Tile(Clone)")
        {
            gridTile = other.gameObject;
        }
        if(this.gameObject.tag == "Base" && other.gameObject.tag == "Enemy")
        {
            this.gameObject.GetComponent<Building>().health -= this.gameObject.GetComponent<Building>().health - other.gameObject.GetComponent<Enemy>().attack;
        }
    }
    private void CheckForTargets()
    {
        foreach (GameObject obj in gridTile.GetComponent<GridStats>().neighbours)
        {
            if (obj.GetComponent<GridStats>().objectOnTileType == GridStats.objectType.ENEMY)
            {
                obj.GetComponent<GridStats>().target = true;
                hasValidTarget = true;
                break;
            }
            else if (obj.GetComponent<GridStats>().objectOnTileType == GridStats.objectType.UNIT)
            {
                // gridTile.GetComponent<GridStats>().target = true;
                hasValidTarget = false;
            }
            else if (obj.GetComponent<GridStats>().objectOnTileType == GridStats.objectType.BASE)
            {
                //  gridTile.GetComponent<GridStats>().target = true;
                hasValidTarget = false;
            }
            else
            {
                //gridTile.GetComponent<GridStats>().target = true;
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
