using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
    // Start is called before the first frame update
    SpawnManager spawnManager_Script;
    void Start()
    {
        spawnManager_Script = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }
    void OnMouseDown()
    {
        if (spawnManager_Script.gameManager_script.resources > 0)
        {
            Instantiate(spawnManager_Script.unitPrefab, gameObject.transform.position + new Vector3(0, 0.5f, 0), transform.parent.parent.rotation);
            spawnManager_Script.gameManager_script.number_of_builders++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
