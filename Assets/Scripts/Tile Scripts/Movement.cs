using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : GridStats
{

    public Transform parenttransform;
    public bool isConnected;
    public bool isConnectedToParent;
    // Start is called before the first frame update
    void Start()
    {
        isConnected = false;
        isConnectedToParent = false;
        //  parent_unit.name = "Unit(Clone)" + 
        //   parent_unit = GameObject.Find("Unit(Clone)");

    }

    // Update is called once per frame
    void Update()
    {
        getNeighbour(Vector3.forward);
        getNeighbour(-Vector3.forward);
        getNeighbour(Vector3.right);
        getNeighbour(-Vector3.right);
        disableTile();

    }
    public void disableTile()
    {
        if(isConnected == false)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void getNeighbour(Vector3 direction)
    {
        Vector3 halfExtends = new Vector3(0.25f, 0.25f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtends);

        foreach (Collider item in colliders)
        {
            GameObject gridStats = item.gameObject;
            if(gridStats != null && gridStats == transform.parent.gameObject)
            {
                isConnectedToParent = false;
                isConnected = false;
                break;
            }
            else
            {
                isConnectedToParent = false;
                isConnected = false;
            }
            if (gridStats != null && gridStats.name == "Movement Tile(Clone)")
            {
                if(gridStats.GetComponent<Movement>().isConnectedToParent)
                {
                    isConnected = false;
                    break;
                }
            }
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Tile")
        {
            this.gameObject.SetActive(false);
            isConnected = false;
        }
        getNeighbour(Vector3.forward);
        getNeighbour(-Vector3.forward);
        getNeighbour(Vector3.right);
        getNeighbour(-Vector3.right);
    }
    public void moveUnit()
    {
        if (transform.parent.GetComponent<UnitController>().hasMoved == false)
        {
            transform.parent.GetComponent<UnitController>().hasMoved = true;
            transform.parent.position = new Vector3(gameObject.transform.position.x, 1, gameObject.transform.position.z);
            transform.parent.GetComponent<UnitController>().disableMovement();
        }

    }
}
