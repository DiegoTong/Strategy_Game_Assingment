using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject builderPrefab;
    public GameObject soldierPrefab;
    public GameObject hQ;
    public GameObject goldPrefab;
    public GameObject enemyPrefab;
    public GameObject turret;
    public GameObject wall;
    public GameObject activeUnityToSpawn;
    public GridBehaviour gB;
    public bool spawningUnit;
    public int limitMax = 70;
    public int limitMin = 0;


    void Start()
    {
        spawningUnit = false;
        activeUnityToSpawn = builderPrefab;
    }
    void Update()
    {
   
    }
    public void SpawnBase(GameObject activeUnit)
    {
        if(activeUnit.name == "Unit(Clone)")
        { 
            Instantiate(hQ, activeUnit.transform.position, activeUnit.transform.rotation);
            Destroy(activeUnit);
        }
    }

    public void SpawnTurret(GameObject activeUnit)
    {
        if (activeUnit.name == "Unit(Clone)")
        {
            Instantiate(turret, activeUnit.transform.position, activeUnit.transform.rotation);
            Destroy(activeUnit);
        }
    }
    public void SpawnWall(GameObject activeUnit)
    {
        if (activeUnit.name == "Unit(Clone)")
        {
            Instantiate(wall, activeUnit.transform.position, activeUnit.transform.rotation);
            Destroy(activeUnit);
        }
    }
    public void selectBuilderToSpawn()
    {
        activeUnityToSpawn = builderPrefab;
    }
    public void selectAttackerToSpawn()
    {
        activeUnityToSpawn = soldierPrefab;
    }
    public void SpawnGold()
    {       
        if(GameObject.FindGameObjectsWithTag("Gold").Length <=1)
        {
            int randGen = Random.Range(1, 15);
            for (int i = 0; i < randGen; i++)
            {
                Instantiate(goldPrefab, GenerateRandPosition(false), goldPrefab.transform.rotation);
            }
        }
    }

    public void SpawnEnemy(bool spawnInGold)
    {
        Instantiate(enemyPrefab, GenerateRandPosition(spawnInGold), enemyPrefab.transform.rotation);
    }
    public void SpawnEnemy(bool spawnInGold,Vector3 newPos)
    {

            Instantiate(enemyPrefab, newPos + GenerateRandPosition(spawnInGold), enemyPrefab.transform.rotation);
    }

    public Vector3 GenerateRandPosition(bool isEnemy)
    {
        bool cantSpawn = true;
        Vector3 randomPos = new Vector3(0,0,0);
        int posX;
        int posz;
        if (isEnemy == true)
        {
            posX = Random.Range(-1, 1);
            posz = Random.Range(-1, 1);
            randomPos = new Vector3(posX, 0.5f, posz);
        }
        else
        {
            while (cantSpawn)
            {

                posX = Random.Range(limitMin, limitMax);
                posz = Random.Range(limitMin, limitMax);
                randomPos = gB.gridArray[posX, posz].gameObject.transform.position;
                cantSpawn = gB.gridArray[posX, posz].GetComponent<GridStats>().isVisible;
            }
        }
 

        return randomPos;
    }
}
