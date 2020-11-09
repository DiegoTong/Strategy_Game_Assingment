using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    public float camera_speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        gameObject.transform.Translate(new Vector3(horizontalInput, 0, verticalInput) * Time.deltaTime * camera_speed);
        if (Input.GetKey(KeyCode.Q))
        {
            gameObject.transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.E))
        {
            gameObject.transform.Rotate(new Vector3(0, -90, 0) * Time.deltaTime, Space.World);
        }
    }
}
