using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public AudioManager audioManager_Script;
    public int vision;
    public int health;
    public int maxHealth;
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
    public List<GridStats> seletcableTiles = new List<GridStats>();
    public List<GridStats> visibleTiles = new List<GridStats>();
    public GameObject[] allTiles;
    public int deathsound;
    public GameObject explosion;
    Stack<GridStats> path = new Stack<GridStats>();

    // Start is called before the first frame update
    void Start()
    {
        allTiles = GameObject.FindGameObjectsWithTag("Tile"); 
        maxHealth = health;
        unitName = gameObject.name;
        gameManager_script = GameObject.Find("Game Manager").GetComponent<GameManager>();
        spawnManager_script = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        audioManager_Script = GameObject.Find("Game Manager").GetComponent<AudioManager>();
        id = gameManager_script.number_of_builders;
        //InstantiateTiles(movement);
        hasMoved = false;
        hasActed = false;
        removeVision();
        GiveVision();
    }

    // Update is called once per frame
    void Update()
    {
          CheckForTargets();
          destroyUnit(deathsound);
    }
    public void destroyUnit(int i)
    {
        if(health <=0)
        {
            audioManager_Script.loadClip(i);
            removeSelectableTiles();
            removeVision();
            Instantiate(explosion, gameObject.transform.position,gameObject.transform.rotation);
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
        if(other.gameObject.tag == "Enemy")
        {

              this.gameObject.GetComponent<UnitController>().health = this.gameObject.GetComponent<UnitController>().health - other.gameObject.GetComponent<UnitController>().attack;
             other.gameObject.GetComponent<UnitController>().health = 0;
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
    public GameObject getTile(GameObject target)
    {
        RaycastHit hit;
        GameObject targetTile = null;
        if(Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            targetTile = hit.collider.gameObject;
        }
        return targetTile;
    }

    public void getCurrentTile()
    {
        gridTile = getTile(gameObject);
        gridTile.GetComponent<GridStats>().current = true;
    }
    public void ComputeAdjacencyList()
    {
        foreach (GameObject gridT in allTiles)
        {
            GridStats t = gridT.gameObject.GetComponent<GridStats>();
            t.findNeighbours();
        }
    }
    //code inpsired by https://www.youtube.com/watch?v=2NVEqBeXdBk 
    public void FindSelectableTiles()
    {
        ComputeAdjacencyList();
        getCurrentTile();
        Queue<GridStats> process = new Queue<GridStats>();
        process.Enqueue(gridTile.GetComponent<GridStats>());
        gridTile.GetComponent<GridStats>().visited = true;
        while(process.Count >0)
        {
            GridStats t = process.Dequeue(); 
            seletcableTiles.Add(t);
            t.selectable = true;
            if (t.distance < movement)
            {
                foreach (GridStats tile in t.adjlist)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }
    public void GiveVision()
    {
        removeVision();
        ComputeAdjacencyList();
        getCurrentTile();
        Queue<GridStats> process = new Queue<GridStats>();
        process.Enqueue(gridTile.GetComponent<GridStats>());
        gridTile.GetComponent<GridStats>().visionVisited = true;
        while (process.Count > 0)
        {
            GridStats t = process.Dequeue();
            visibleTiles.Add(t);
            t.isVisible = true;
            if (t.visionDistance < vision)
            {
                foreach (GameObject tile in t.neighbours)
                {
                    if (!tile.GetComponent<GridStats>().visionVisited)
                    {
                        tile.GetComponent<GridStats>().visionParent = t;
                        tile.GetComponent<GridStats>().visionVisited = true;
                        tile.GetComponent<GridStats>().visionDistance = 1 + t.visionDistance;
                        process.Enqueue(tile.GetComponent<GridStats>());
                    }
                }
            }
        }
    }
    public void removeVision()
    {
        if (gridTile != null)
        {
            gridTile.GetComponent<GridStats>().current = false;
        }
        foreach (GridStats tile in visibleTiles)
        {
            tile.clearVision();
        }
        visibleTiles.Clear();
    }
    //code inpsired by https://www.youtube.com/watch?v=2NVEqBeXdBk 
    public void removeSelectableTiles()
    {
        if(gridTile != null)
        {
            gridTile.GetComponent<GridStats>().current = false;
        }
        foreach(GridStats tile in seletcableTiles)
        {
            tile.clear();
        }
        seletcableTiles.Clear();
    }

    public void setDarkvision()
    {
        foreach (GameObject gridT in allTiles)
        {
            GridStats t = gridT.gameObject.GetComponent<GridStats>();
            t.isVisible = false;
        }
    }
}
