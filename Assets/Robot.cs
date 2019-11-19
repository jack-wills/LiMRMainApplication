using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public float updateTime;

    private Vector2 robotCoord;
    private SensorController sensorController;
    // Start is called before the first frame update
    void Start()
    {
        sensorController = GameObject.Find("SensorController").GetComponent<SensorController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 robotCoord = new Vector2(sensorController.GetSensorValue("robotCoordX"), sensorController.GetSensorValue("robotCoordY"));
        Vector3 objPos = this.GetComponent<Transform>().position;
        if (robotCoord.x != objPos.x || robotCoord.y != objPos.y)
        {
            StopCoroutine(MoveRoutine(robotCoord));
            StartCoroutine(MoveRoutine(robotCoord));
        }
    }

    private IEnumerator MoveRoutine(Vector2 robotCoord)
    {
        Vector3 objPos = this.GetComponent<Transform>().position;

        Vector3 a = objPos;
        Vector3 b = new Vector3(robotCoord.x, objPos.y, robotCoord.y);
        
        float t = 0;
        while (t <= 1.0f)
        {
            t += (Time.deltaTime / updateTime); // Goes from 0 to 1, incrementing by step each time
            this.GetComponent<Transform>().position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        this.GetComponent<Transform>().position = b;

    }
}
