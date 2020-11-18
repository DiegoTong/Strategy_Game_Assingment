using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public int resources;
    public Camera camera;
    public GameObject Indicator;
    public GameObject focalPoint;
    public GameObject nextUnit;
    public UIManager uiManager_script;
    public SpawnManager spawnManager_script;
    public int number_of_builders;
    public GameObject[] activeUnits;
    public GameObject[] activeEnemyUnits;
    public GameObject[] activeGold;
    public bool gameOver;
    public int numb_of_Bases;
    public bool nextTurn;
    public bool spawnInGold;
    public GameObject selectedUnit;

    // Start is called before the first frame update
    void Start()
    {
        uiManager_script = GameObject.Find("Game Manager").GetComponent<UIManager>();
        spawnManager_script = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        gameOver = false;
        nextTurn = false;
       
        spawnManager_script.SpawnGold();
        spawnInGold = true;
    }

    // Update is called once per frame
    void Update()
    {

        SelectUnit();
        CheckActiveUnits();
        CheckActionButton(nextTurn);
        CheckGameOver();
    }
    void SpawnGrid()
    {

    }
    void SelectUnit()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                //print out the name if the raycast hits something
                Indicator.SetActive(false);
                Debug.Log(hit.collider.name);
                if(hit.collider.tag != "Tile")
                {
                    Indicator.SetActive(true);
                    Indicator.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, 2.5f, hit.collider.gameObject.transform.position.z);
                    if (hit.collider.name == "Unit(Clone)" || hit.collider.name == "Unit" || hit.collider.name == "Attk_Unit")
                    {
                        nextUnit = hit.collider.gameObject;
                        uiManager_script.setStats(nextUnit.GetComponent<UnitController>());
                        uiManager_script.spawnbutton.gameObject.SetActive(true);
                        uiManager_script.skipButton.gameObject.SetActive(true);
                        if (nextUnit.GetComponent<UnitController>().hasValidTarget)
                        {
                            uiManager_script.attacButton.gameObject.SetActive(true);
                        }
                        else
                        {
                            uiManager_script.attacButton.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        uiManager_script.setStats();
                        uiManager_script.attacButton.gameObject.SetActive(false);
                        uiManager_script.spawnbutton.gameObject.SetActive(false);
                        uiManager_script.skipButton.gameObject.SetActive(false);
                    }
                }
                else if(hit.collider.tag == "Tile" && (hit.collider.name == "Movement Tile" || hit.collider.name == "Movement Tile(Clone)"))
                {
                    hit.collider.GetComponent<Movement>().moveUnit();
                }
                else if (hit.collider.tag == "Tile" && (hit.collider.name == "Spawn Tile" || hit.collider.name == "Spawn Tile(Clone)"))
                {
                    hit.collider.GetComponent<SpawnTile>().SpawnUnit();
                }
            }

        }
    }
    void CheckActiveUnits()
    {
        activeUnits = GameObject.FindGameObjectsWithTag("Friendly");
        activeEnemyUnits = GameObject.FindGameObjectsWithTag("Enemy");
        activeGold = GameObject.FindGameObjectsWithTag("Gold");   
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
        uiManager_script.actionButtonText.text = "Next Turn";
        if (activeUnits.Length > 0 && activeUnits.Length !=0)
        {
            foreach (GameObject friendly in activeUnits)
            {
                if (friendly.GetComponent<UnitController>().hasMoved == false)
                {
                    uiManager_script.actionButtonText.text = "Unit Awaiting Order";
                    nextUnit = friendly;
                    break;
                }
            }
        }

        //    if (activeUnits.Length !=0)
        //{
        //    foreach (GameObject friendly in activeUnits)
        //    {
        //        if (activeUnits.Length != 0)
        //        {
        //            if (friendly.GetComponent<UnitController>().hasMoved == false)
        //            {
        //                if (nextUnit.CompareTag("Base"))
        //                {
        //                    uiManager_script.actionButtonText.text = "Unit Awaiting Order";
        //                    nextUnit = friendly;
        //                }
        //                else if ((nextUnit.GetComponent<UnitController>().id != friendly.GetComponent<UnitController>().id))
        //                {
        //                    uiManager_script.actionButtonText.text = "Unit Awaiting Order";
        //                    //  nextUnit = friendly;                   
        //                }
        //                nextTurn = false;
        //                break;
        //            }
        //        }
        //    }
        //}

    }
    public void CheckActionButton(bool checkNextTurn)
    {
        uiManager_script.actionButtonText.text = "Next Turn";
        nextTurn = true;
        if (activeUnits.Length > 0 && activeUnits.Length != 0)
        {
            foreach (GameObject friendly in activeUnits)
            {
                if (friendly.GetComponent<UnitController>().hasMoved == false)
                {
                    uiManager_script.actionButtonText.text = "Unit Awaiting Order";
                    //   nextUnit = friendly;
                    nextTurn = false;
                    break;
                }
            }
        }
    }
    public void BuildHQ()
    {
        if (resources > 3 && GameObject.FindGameObjectsWithTag("Friendly").Length > 0)
        {
            uiManager_script.spawnbuttonText.text = "Spawn Base";
            spawnManager_script.SpawnBase(nextUnit);
            CheckActionButton();
        }
        else
        {
            uiManager_script.spawnbuttonText.text = "No Resources";
        }
    }
    public void skipAction()
    {
        nextUnit.GetComponent<UnitController>().hasMoved = true;
        nextUnit.GetComponent<UnitController>().disableMovement();
    }
    public void InstantiateEnemies()
    {
        if (activeGold.Length>0)
        {
            spawnInGold = true;
            spawnManager_script.SpawnEnemy(spawnInGold, activeGold[Random.Range(0, activeGold.Length)].transform.position);
        }
        else
        {
            spawnInGold = false;
            spawnManager_script.SpawnEnemy(spawnInGold);
        }
  
    }

    public void NextTurn()
    {
        if (nextTurn == false)
        {
            CheckActionButton();
            focalPoint.transform.position = nextUnit.transform.position;
        }
        else
        {
            if(GameObject.FindGameObjectsWithTag("Friendly").Length != 0)
            {
                foreach (GameObject friendly in activeUnits)
                {
                    friendly.GetComponent<UnitController>().hasMoved = false;
                    friendly.GetComponent<UnitController>().disableMovement();
                }
            }
            if (GameObject.FindGameObjectsWithTag("Enemy").Length != 0)
            {
                foreach (GameObject enemy in activeEnemyUnits)
                {
                    enemy.GetComponent<Enemy>().hasMoved = false;
                }
            }

            spawnManager_script.SpawnGold();
            InstantiateEnemies();
        }
    }
}

