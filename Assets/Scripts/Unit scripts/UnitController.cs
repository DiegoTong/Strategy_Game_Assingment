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
    // Start is called before the first frame update
    void Start()
    {
        gameManager_script = GameObject.Find("Game Manager").GetComponent<GameManager>();
        spawnManager_script = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        id = spawnManager_script.activebuildercount;
        InstantiateTiles(movement);
        hasMoved = false;
        hasActed = false;
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gold"))
        {
            gameManager_script.resources++;
            Destroy(other.gameObject);
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
