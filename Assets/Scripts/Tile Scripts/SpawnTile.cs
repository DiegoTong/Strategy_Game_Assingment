using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
    // Start is called before the first frame update
    GameManager gameManagerScript;
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    public void SpawnUnit()
    {
        if (gameManagerScript.resources > 0)
        {
            gameManagerScript.nextUnit = Instantiate(gameManagerScript.spawnManager_script.unitPrefab, gameObject.transform.position + new Vector3(0, 0.5f, 0), transform.parent.parent.rotation);
            gameManagerScript.number_of_builders++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
