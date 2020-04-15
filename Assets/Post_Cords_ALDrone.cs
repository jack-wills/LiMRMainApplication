using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;


using UnityEngine;

public class Post_Cords_ALDrone : MonoBehaviour
{

    public PostDataCords postData;

    private string url = "https://bmgxwpyyd2.execute-api.us-east-1.amazonaws.com/prod/sensordata";
    private string droneID;
    private string height;
    public GameObject Drone;


    private void Start()
    {
        Drone = GameObject.Find("AxisLockedDrone");
        Drone.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P) || OVRInput.GetDown(OVRInput.Button.Start))
        {
            //Was used for testing Drone.SetActive(true);
            //postData.ButtonPress();
            postData.SendCords();
            
        }

    }

 

}

