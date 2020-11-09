using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform parenttransform;
    // Start is called before the first frame update
    void Start()
    {
        //  parent_unit.name = "Unit(Clone)" + 
        //   parent_unit = GameObject.Find("Unit(Clone)");
      
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    void OnMouseDown()
    {
        gameObject.GetComponentInParent<UnitController>().hasMoved = true;
        // Destroy the gameObject after clicking on it
        transform.parent.position = new Vector3(gameObject.transform.position.x, 1 , gameObject.transform.position.z);
    }
}
