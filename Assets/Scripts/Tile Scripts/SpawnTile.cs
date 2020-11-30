using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : GridStats
{
    // Start is called before the first frame update
    GameManager gameManagerScript;
    public bool hasObjOn;
    void Start()
    {
        hasObjOn = false;
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Tile")
        {
            hasObjOn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        hasObjOn = false;

    }
    public void SpawnUnit()
    {
        if (gameManagerScript.resources > 0)
        {
            gameManagerScript.audioManager_Script.loadClip(3);
            gameManagerScript.nextUnit = Instantiate(gameManagerScript.spawnManager_script.activeUnityToSpawn, gameObject.transform.position + new Vector3(0, 0.5f, 0), transform.parent.parent.rotation);
            gameManagerScript.number_of_builders++;
            gameManagerScript.resources--;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
