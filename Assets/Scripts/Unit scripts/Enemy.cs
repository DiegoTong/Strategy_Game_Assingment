using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitController
{
    // Start is called before the first frame update
    public GameObject[] targets;
    public Vector3 closestTarget;
    public GameObject activeTarget;
    public AudioSource spawnManager_audio_source;
    void Start()
    {
        hasMoved = false;
        hasActed = false;
        closestTarget = new Vector3(0, 0, 0);
        targets = GameObject.FindGameObjectsWithTag("Base");
        getCurrentTile();
        spawnManager_audio_source = GameObject.Find("Spawn Manager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
 
        if(hasMoved == false)
        {
            CheckForTargets();
            EnemyHasMoved();
        }
        destroyUnit();
    }
    public void destroyUnit()
    {
        if (health <= 0)
        {
            spawnManager_audio_source.Play();
            removeSelectableTiles();
            removeVision();
            Destroy(gameObject);
        }
    }
    void EnemyHasMoved()
    {

            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, activeTarget.transform.position, 1);
            gameObject.transform.position = new Vector3(Mathf.Round(gameObject.transform.position.x), Mathf.Round(gameObject.transform.position.y), Mathf.Round(gameObject.transform.position.z));
            hasMoved = true;
    }
    private void EnemyAttack(GridStats tileWithUnit)
    {
        if(hasActed == false)
        {
            tileWithUnit.currentObjectOnTile.GetComponent<UnitController>().health = tileWithUnit.currentObjectOnTile.GetComponent<UnitController>().health - attack;
        }
        hasActed = true;
    }
    // //code inpsired by https://www.youtube.com/watch?v=2NVEqBeXdBk 
    private GridStats FindLowestFCost(List<GridStats> tileList)
    {
        GridStats lowest = tileList[0];
        foreach(GridStats grid in tileList)
        {
            if(grid.fCost < lowest.fCost)
            {
                lowest = grid;
            }
        }
        tileList.Remove(lowest);
        return lowest;
    }
    // //code inpsired by https://www.youtube.com/watch?v=2NVEqBeXdBk 
    private void findPath()
    {
        GameObject targetTile = getTile(activeTarget);
        calculatePath(targetTile.GetComponent<GridStats>());
    }
    // //code inpsired by https://www.youtube.com/watch?v=2NVEqBeXdBk 
    private void calculatePath(GridStats targetTile)
    {
        targets = GameObject.FindGameObjectsWithTag("Base");
        ComputeAdjacencyList(targetTile);
        getCurrentTile();
        List<GridStats> openList = new List<GridStats>();
        List<GridStats> closedList = new List<GridStats>();
        openList.Add(gridTile.GetComponent<GridStats>());

        gridTile.GetComponent<GridStats>().hCost = Vector3.Distance(gridTile.transform.position, targetTile.transform.position);
        gridTile.GetComponent<GridStats>().fCost = gridTile.GetComponent<GridStats>().hCost;
        while(openList.Count > 0)
        {
            GridStats t = FindLowestFCost(openList);
            closedList.Add(t);
            if(t == targetTile)
            {
                EnemyHasMoved();
                return;
            }

            foreach(GridStats tile in t.adjlist)
            {
                if(closedList.Contains(tile))
                {

                }
                else if(openList.Contains(tile))
                {
                    float tempgCost = t.gCost + Vector3.Distance(tile.transform.position, t.transform.position);

                    if(tempgCost < tile.gCost)
                    {
                        tile.parent = t;
                        tile.gCost = tempgCost;
                        tile.fCost = tile.gCost + tile.hCost;
                    }
                }
                else
                {
                    tile.parent = t;

                    tile.gCost = t.gCost + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.hCost = Vector3.Distance(tile.transform.position, targetTile.transform.position);
                    tile.fCost = tile.gCost + tile.hCost;

                    openList.Add(tile);
                }
            }
        }   
    }

    public void ComputeAdjacencyList(GridStats targetT)
    {
        foreach (GameObject gridT in allTiles)
        {
            GridStats t = gridT.gameObject.GetComponent<GridStats>();
            t.findNeighbours(targetT);
        }
    }
    private void CheckForTargets()
    {
        activeTarget = null;
        float distance = Mathf.Infinity;
        foreach (GameObject chooseTarget in targets)
        {
            float targetDistance = Vector3.Distance(transform.position, chooseTarget.transform.position);
            if(targetDistance < distance)
            {
                distance = targetDistance;
                activeTarget = chooseTarget;
            }
        }
        //foreach (GameObject chooseTarget in targets)
        //{
        //    if ((closestTarget.x + closestTarget.z) > (chooseTarget.transform.position.x + chooseTarget.transform.position.z) || (closestTarget.x + closestTarget.z) == 0)
        //    {
        //        activeTarget = chooseTarget;
        //    }
        //}



        foreach (GameObject obj in gridTile.GetComponent<GridStats>().neighbours)
        {
            if (obj.GetComponent<GridStats>().objectOnTileType == GridStats.objectType.ENEMY)
            {
                hasValidTarget = false;
                break;
            }
            else if (obj.GetComponent<GridStats>().objectOnTileType == GridStats.objectType.UNIT)
            {
                obj.GetComponent<GridStats>().target = true;
                hasValidTarget = true;
                EnemyAttack(obj.GetComponent<GridStats>());
                break;
            }
            else if (obj.GetComponent<GridStats>().objectOnTileType == GridStats.objectType.BASE)
            {
                //  gridTile.GetComponent<GridStats>().target = true;
                hasValidTarget = false;
            }
            else
            {
                //gridTile.GetComponent<GridStats>().target = true;
                hasValidTarget = false;
            }
        }
        targets = GameObject.FindGameObjectsWithTag("Base");
        foreach (GameObject chooseTarget in targets)
        {
            if ((closestTarget.x + closestTarget.z) > (chooseTarget.transform.position.x + chooseTarget.transform.position.z) || (closestTarget.x + closestTarget.z) == 0)
            {
                activeTarget = chooseTarget;
            }
        }
    }
}
