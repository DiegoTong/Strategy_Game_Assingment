using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitController
{
    // Start is called before the first frame update
    public GameObject[] targets;
    public Vector3 closestTarget;
    public GameObject activeTarget;
    void Start()
    {
        hasMoved = false;
        hasActed = false;
        closestTarget = new Vector3(0, 0, 0);
        targets = GameObject.FindGameObjectsWithTag("Base");
        foreach(GameObject chooseTarget in targets)
        {
            if((closestTarget.x + closestTarget.z)>(chooseTarget.transform.position.x+chooseTarget.transform.position.z) || (closestTarget.x + closestTarget.z) == 0)
            {
                activeTarget = chooseTarget;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnemyHasMoved();
        destroyUnit();
    }
    void EnemyHasMoved()
    {
        if(hasMoved == false)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, activeTarget.transform.position, 1);
            gameObject.transform.position = new Vector3(Mathf.Round(gameObject.transform.position.x), Mathf.Round(gameObject.transform.position.y), Mathf.Round(gameObject.transform.position.z));
            hasMoved = true;
        }

    }
}
