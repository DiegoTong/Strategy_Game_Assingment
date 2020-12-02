using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public int resources;
    public Camera camera;
    public Camera frontFacingCamera;
    public GameObject Indicator;
    public GameObject focalPoint;
    public GameObject nextUnit;
    public UIManager uiManager_script;
    public SpawnManager spawnManager_script;
    public AudioManager audioManager_Script;
    public int number_of_builders;
    public GameObject[] activeUnits;
    public GameObject[] activeEnemyUnits;
    public GameObject[] activeGold;
    public GameObject[] buildings;
    public GameObject[] allFriendlyUnits;
    public bool gameOver;
    public int numb_of_Bases;
    public bool nextTurn;
    public bool spawnInGold;
    public bool selectingEnemy;
    public int turn;
    public GameObject selectedUnit;
    public int maxCap;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        turn = 1;
        offset = new Vector3 (0,2f,0);
        uiManager_script = GameObject.Find("Game Manager").GetComponent<UIManager>();
        spawnManager_script = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        audioManager_Script = GameObject.Find("Game Manager").GetComponent<AudioManager>();
        gameOver = false;
        nextTurn = false;
        selectingEnemy = false;
        spawnManager_script.SpawnGold();
        spawnInGold = true;
        maxCap = 5;
        nextUnit = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        checkNextUnit();
        SelectUnit();
        CheckActiveUnits();
        CheckActionButton(nextTurn);
        CheckGameOver();
        populateUI();
    }

    void checkNextUnit()
    {
        if(nextUnit == null)
        {
            nextUnit = gameObject;
        }
    }
    void SelectUnit()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Click");
            if (Physics.Raycast(ray, out hit))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {

                }
                else
                {
                    if (selectingEnemy == false)
                    {                        
                        //print out the name if the raycast hits something
                        Indicator.SetActive(false);
                        Debug.Log(hit.collider.name);
                        if (hit.collider.tag != "Tile")
                        {
                            Indicator.SetActive(true);
                            Indicator.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, 2.5f, hit.collider.gameObject.transform.position.z);
                            if (nextUnit.name != "Game Manager" )
                            {
                                nextUnit.GetComponent<UnitController>().removeSelectableTiles();
                            }
                            if (hit.collider.tag == "Friendly")
                            {
                                uiManager_script.disableUIBUttons();
                                nextUnit = hit.collider.gameObject;
                                uiManager_script.setStats(nextUnit.GetComponent<UnitController>());
                                if(hit.collider.name == "Unit(Clone)")
                                {
                                    uiManager_script.spawnbutton.gameObject.SetActive(true);
                                }
                                else
                                {
                                    uiManager_script.spawnbutton.gameObject.SetActive(false);
                                }
                                if(hit.collider.name == "GatlingTower(Clone)" || hit.collider.name == "Wall(Clone)")
                                {
                                    hit.collider.GetComponent<UnitController>().hasMoved = true;
                                }

                                if (!nextUnit.GetComponent<UnitController>().hasMoved)
                                {
                                    uiManager_script.skipButton.gameObject.SetActive(true);
                                    nextUnit.GetComponent<UnitController>().FindSelectableTiles();
                                }
                                else
                                {
                                    uiManager_script.skipButton.gameObject.SetActive(false);
                                }
                                if (nextUnit.GetComponent<UnitController>().hasValidTarget && nextUnit.GetComponent<UnitController>().hasActed == false)
                                {
                                    uiManager_script.attacButton.gameObject.SetActive(true);
                                }
                                else
                                {
                                    uiManager_script.attacButton.gameObject.SetActive(false);
                                }
                            }
                            else if (hit.collider.tag == "Enemy")
                            {
                                uiManager_script.setStats(hit.collider.GetComponent<UnitController>());
                                uiManager_script.attacButton.gameObject.SetActive(false);
                                uiManager_script.spawnbutton.gameObject.SetActive(false);
                                uiManager_script.skipButton.gameObject.SetActive(false); 
                                uiManager_script.disableUIBUttons();
                            }
                            else if (hit.collider.tag == "Base")
                            {
                                uiManager_script.disableUIBUttons();
                                uiManager_script.setStats(hit.collider.GetComponent<UnitController>());
                                uiManager_script.EnableSpawnButtons();
                                uiManager_script.attacButton.gameObject.SetActive(false);
                                uiManager_script.spawnbutton.gameObject.SetActive(false);
                                uiManager_script.skipButton.gameObject.SetActive(false);
                            }
                            else
                            {
                                nextUnit.GetComponent<UnitController>().removeSelectableTiles();
                                uiManager_script.attacButton.gameObject.SetActive(false);
                                uiManager_script.spawnbutton.gameObject.SetActive(false);
                                uiManager_script.skipButton.gameObject.SetActive(false);
                                uiManager_script.disableUIBUttons();
                            }
                            frontFacingCamera.transform.position = hit.collider.gameObject.transform.position + hit.collider.gameObject.transform.forward * 3 + offset;
                            frontFacingCamera.transform.LookAt(hit.collider.gameObject.transform);
                        }
                        else if (hit.collider.tag == "Tile" && (hit.collider.name == "Movement Tile" || hit.collider.name == "Movement Tile(Clone)"))
                        {
                            //hit.collider.GetComponent<Movement>().moveUnit();
                            uiManager_script.attacButton.gameObject.SetActive(false);
                            uiManager_script.spawnbutton.gameObject.SetActive(false);
                            uiManager_script.skipButton.gameObject.SetActive(false);
                            uiManager_script.disableUIBUttons();
                        }
                        else if (hit.collider.tag == "Tile" && (hit.collider.name == "Spawn Tile" || hit.collider.name == "Spawn Tile(Clone)"))
                        {
                            if ((allFriendlyUnits.Length < maxCap))
                            {
                                hit.collider.GetComponent<SpawnTile>().SpawnUnit();
                            }                
                            uiManager_script.attacButton.gameObject.SetActive(false);
                            uiManager_script.spawnbutton.gameObject.SetActive(false);
                            uiManager_script.skipButton.gameObject.SetActive(false);
                            uiManager_script.disableUIBUttons();
                        }
                        else if (hit.collider.tag == "Tile")
                        {
                            if(hit.collider.gameObject.GetComponent<GridStats>().selectable)
                            {
                                nextUnit.transform.position = new Vector3(hit.collider.transform.position.x,1f, hit.collider.transform.position.z);
                                GetAllVision();
                                nextUnit.GetComponent<UnitController>().removeSelectableTiles();
                                nextUnit.GetComponent<UnitController>().hasMoved = true;
                                uiManager_script.attacButton.gameObject.SetActive(false);
                            }
                        //    uiManager_script.attacButton.gameObject.SetActive(false);
                        //    uiManager_script.spawnbutton.gameObject.SetActive(false);
                        //    uiManager_script.skipButton.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        bool hasMovedBefore = nextUnit.GetComponent<UnitController>().hasMoved;
                        nextUnit.GetComponent<UnitController>().hasMoved = true;
                        nextUnit.GetComponent<UnitController>().disableMovement();
                        if (hit.collider.tag == "Enemy")
                        {
                            Debug.Log("Hit");
                            audioManager_Script.loadClip(5);
                            hit.collider.GetComponent<UnitController>().health = hit.collider.GetComponent<UnitController>().health - nextUnit.GetComponent<UnitController>().attack;
                            nextUnit.GetComponent<UnitController>().hasActed = true;
                            uiManager_script.attacButton.gameObject.SetActive(false);
                        }
                        else
                        {
                            nextUnit.GetComponent<UnitController>().hasMoved = hasMovedBefore;
                        }
                        selectingEnemy = false;
                    }
                }
            }
        }


    }
    public void populateUI()
    {
        buildings = GameObject.FindGameObjectsWithTag("Base");
        allFriendlyUnits = GameObject.FindGameObjectsWithTag("Friendly");
        maxCap = buildings.Length * 5;
        
        uiManager_script.updateUI(resources, turn, activeGold.Length, allFriendlyUnits.Length ,maxCap);
    }


    public void GetAllVision()
    {
        nextUnit.GetComponent<UnitController>().setDarkvision();
        foreach (GameObject obj in allFriendlyUnits)
        {
            obj.GetComponent<UnitController>().removeVision();
            obj.GetComponent<UnitController>().GiveVision();
        }
        foreach (GameObject obj in buildings)
        {
            obj.GetComponent<UnitController>().removeVision();
            obj.GetComponent<UnitController>().GiveVision();
        }
    }
    void CheckActiveUnits()
    {
        activeUnits = GameObject.FindGameObjectsWithTag("Friendly");
        activeEnemyUnits = GameObject.FindGameObjectsWithTag("Enemy");
        activeGold = GameObject.FindGameObjectsWithTag("Gold");   
    }
    public void Attack()
    {
        selectingEnemy = true;
    }

    void CheckGameOver()
    {
        if (GameObject.FindGameObjectsWithTag("Base").Length == 0)
        {
            gameOver = true;
            SceneManager.LoadScene("Main Menu");
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
        Debug.Log("Build");

        if (resources > 3 && GameObject.FindGameObjectsWithTag("Friendly").Length > 0)
        {
            audioManager_Script.loadClip(2);
            spawnManager_script.SpawnBase(nextUnit);
            CheckActionButton();
            resources--;
        }
        else
        {

        }
        uiManager_script.disableUIBUttons();
    }

    public void BuildWall()
    {
        Debug.Log("Build");
        if (resources > 3 && GameObject.FindGameObjectsWithTag("Friendly").Length > 0)
        {
            audioManager_Script.loadClip(2);
            spawnManager_script.SpawnWall(nextUnit);
            CheckActionButton();
            resources--;
        }
        else
        {

        }
        uiManager_script.disableUIBUttons();
    }
    public void Buildturret()
    {
        Debug.Log("Build");
        if (resources > 3 && GameObject.FindGameObjectsWithTag("Friendly").Length > 0)
        {
            audioManager_Script.loadClip(2);
            uiManager_script.spawnbuttonText.text = "Spawn Base";
            spawnManager_script.SpawnTurret(nextUnit);
            CheckActionButton();
            resources--;
        }
        else
        {
        
        }
        uiManager_script.disableUIBUttons();
    }
    public void skipAction()
    {
        Debug.Log("skip");
        nextUnit.GetComponent<UnitController>().hasMoved = true;
        nextUnit.GetComponent<UnitController>().disableMovement();
        uiManager_script.attacButton.gameObject.SetActive(false);
        uiManager_script.spawnbutton.gameObject.SetActive(false);
        uiManager_script.skipButton.gameObject.SetActive(false);
    }
    public void InstantiateEnemies()
    {
        if (activeGold.Length>0 )
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
        Debug.Log("NextTurn");
        if (nextTurn == false)
        {
            CheckActionButton();
            Indicator.SetActive(true);
            Indicator.transform.position = new Vector3(nextUnit.transform.position.x, 2.5f, nextUnit.transform.position.z);
            focalPoint.transform.position = nextUnit.transform.position;
        }
        else
        {
            if(GameObject.FindGameObjectsWithTag("Friendly").Length != 0)
            {
                foreach (GameObject friendly in activeUnits)
                {
                    friendly.GetComponent<UnitController>().hasMoved = false;
                    friendly.GetComponent<UnitController>().hasActed = false;
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
            turn++;
            resources += (buildings.Length *2);
           if(activeGold.Length < (buildings.Length * 3))
            {
                spawnManager_script.SpawnGold();
            }
           foreach(GameObject enemySpawn in activeGold)
            {
                InstantiateEnemies();
            }
        }
    }
}

