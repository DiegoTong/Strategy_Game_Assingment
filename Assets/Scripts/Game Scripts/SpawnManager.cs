using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject unitPrefab;
    public GameObject hQ;
    public GameManager gameManager_script;
    public GameObject unit_builder;
    public int activebuildercount;


    void Start()
    {
        gameManager_script = GameObject.Find("Game Manager").GetComponent<GameManager>();               
    }
    void Update()
    {
        activebuildercount = FindObjectsOfType<UnitController>().Length;
    }
    public void SpawnBase()
    {
        if (gameManager_script.resources > 3 && activebuildercount > 0)
        {
            unit_builder = GameObject.Find("Unit(Clone)");
            Instantiate(hQ, unit_builder.transform.position, unit_builder.transform.rotation);
            Destroy(unit_builder);
        }
    }
}
