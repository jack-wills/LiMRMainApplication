using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisLockedDroneMovementScript : MonoBehaviour
{


    public float dronespeed;
    public float rotationSpeed;

  
    // Start is called before the first frame update
    void Start()
    {
        dronespeed = 10f;

    }

    // Update is called once per frame
    void Update()
    {

        Movement();

    }

    void Movement()
    {
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * dronespeed;
        float rotation = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;
        transform.Translate(0, vertical, 0);
        transform.Rotate(Vector3.up,rotation);


    }

 
}
