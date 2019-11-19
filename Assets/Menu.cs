using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    private bool isPointerForRobot = false;
    private OVRPhysicsRaycaster ovrPhysicsRaycaster;
    private LaserPointer laserPointer;
    private GameObject robotCursor, sensorCursor;
    // Start is called before the first frame update
    void Start()
    {
        ovrPhysicsRaycaster = GameObject.Find("OVRCameraRig").GetComponent<OVRPhysicsRaycaster>();
        laserPointer = GameObject.Find("LaserPointer").GetComponent<LaserPointer>();
        sensorCursor = GameObject.Find("SensorPointerCursor");
        robotCursor = GameObject.Find("RobotPointerCursor");


        ovrPhysicsRaycaster.eventMask = LayerMask.GetMask("SensorPointerSelect");
        laserPointer.cursorVisual = sensorCursor;
        sensorCursor.SetActive(true);
        robotCursor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            if (isPointerForRobot)
            {
                isPointerForRobot = false;
                ovrPhysicsRaycaster.eventMask = LayerMask.GetMask("SensorPointerSelect");
                laserPointer.cursorVisual = sensorCursor;
                sensorCursor.SetActive(true);
                robotCursor.SetActive(false);
            } else
            {
                isPointerForRobot = true;
                ovrPhysicsRaycaster.eventMask = LayerMask.GetMask("RobotPointerSelect");
                laserPointer.cursorVisual = robotCursor;
                sensorCursor.SetActive(false);
                robotCursor.SetActive(true);
            }
        }
    }
}
