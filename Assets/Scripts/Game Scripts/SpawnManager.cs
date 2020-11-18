using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject unitPrefab;
    public GameObject hQ;
    public GameObject goldPrefab;
    public GameObject enemyPrefab;
   
    public int limitMax = 40;
    public int limitMin = -40;

    void Start()
    {
                   
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
            Instantiate(enemyPrefab, GenerateRandPosition(spawnInGold), enemyPrefab.transform.rotation);
    }

    public Vector3 GenerateRandPosition(bool isEnemy)
    {
        Vector3 randomPos = new Vector3(0,0,0);
        if (isEnemy == true)
        {
            float posX = Random.Range(-1, 1);
            float posz = Random.Range(-1, 1);
            randomPos = new Vector3(posX, 0.5f, posz);
        }
        else
        {
            float posX = Random.Range(limitMin, limitMax);
            float posz = Random.Range(limitMin, limitMax);
            randomPos = new Vector3(posX, 0.5f, posz);       
        }
        return randomPos;
    }
}
