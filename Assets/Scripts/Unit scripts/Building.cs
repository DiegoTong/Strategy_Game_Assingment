using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : UnitController
{
    // Start is called before the first frame update
    void Start()
    {
        allTiles = GameObject.FindGameObjectsWithTag("Tile");
        audioManager_Script = GameObject.Find("Game Manager").GetComponent<AudioManager>();
        removeVision();
        GiveVision();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForTargets();
        destroyUnit(deathsound);
        hasMoved = true;
    }
}
