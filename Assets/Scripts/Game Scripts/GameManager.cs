using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public int resources;
    public GameObject focalPoint;
    public GameObject nextUnit;
    public UIManager uiManager_script;
    public int number_of_builders;
    public GameObject[] activeUnits;
    public GameObject[] activeEnemyUnits;
    public bool gameOver;
    public int numb_of_Bases;
    public bool nextTurn;
    public GameObject selectedUnit;
    // Start is called before the first frame update
    void Start()
    {
        uiManager_script = GameObject.Find("Game Manager").GetComponent<UIManager>();
        gameOver = false;
        nextTurn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (resources > 3)
        {
            uiManager_script.spawnbuttonText.text = "Spawn Base";
        }
        else
        {
            uiManager_script.spawnbuttonText.text = "No Resources";
        }
        CheckActiveUnits();
        CheckActionButton();
        CheckGameOver();
    }
    void CheckActiveUnits()
    {
        if (GameObject.FindGameObjectsWithTag("Friendly").Length != 0)
        {
            activeUnits = GameObject.FindGameObjectsWithTag("Friendly");
        }
        if (GameObject.FindGameObjectsWithTag("Enemy").Length != 0)
        {
            activeEnemyUnits = GameObject.FindGameObjectsWithTag("Enemy");
        }
    }
    void CheckGameOver()
    {
        if (GameObject.FindGameObjectsWithTag("Base").Length == 0)
        {
            gameOver = true;
            Debug.Log("Game Over");
        }
    }
    public void CheckActionButton()
    {
        foreach (GameObject friendly in activeUnits)
        {
            uiManager_script.actionButtonText.text = "Next Turn";
            nextTurn = true;
            if (friendly.GetComponent<UnitController>().hasMoved == false)
            {
                if(nextUnit.CompareTag("Base"))
                {
                    uiManager_script.actionButtonText.text = "Unit Awaiting Order";
                    nextUnit = friendly;
                }
                else if ((nextUnit.GetComponent<UnitController>().id != friendly.GetComponent<UnitController>().id) )
                {
                    uiManager_script.actionButtonText.text = "Unit Awaiting Order";
                  //  nextUnit = friendly;                   
                }
                nextTurn = false;
                break;
            }
        }

    }
    private void OnMouseDown()
    {
        
    }
    public void NextTurn()
    {
        if (nextTurn == false)
        {

            foreach (GameObject friendly in activeUnits)
            {
                if (friendly.GetComponent<UnitController>().hasMoved == false)
                {
                    if (nextUnit.GetComponent<UnitController>().id != friendly.GetComponent<UnitController>().id)
                    {
                        nextUnit = friendly;
                        break;
                    }
                }
            }
            focalPoint.transform.position = nextUnit.transform.position;
        }
        else
        {
            foreach (GameObject friendly in activeUnits)
            {
                friendly.GetComponent<UnitController>().hasMoved = false;
            }
            foreach(GameObject enemy in activeEnemyUnits)
            {
                enemy.GetComponent<Enemy>().hasMoved = false;
            }
        }
    }
}

