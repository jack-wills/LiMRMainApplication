using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorThreshold : MonoBehaviour
{
    public Color goodColour;
    public Color badColour;
    public float lowerThreshold;
    public float upperThreshold;

    private Sensor sensor;

    // Start is called before the first frame update
    void Start()
    {
        sensor = this.GetComponent<Sensor>();
    }

    private static float Sigmoid(float value)
    {
        return 1.0f / (1.0f + (float)Mathf.Exp(-value));
    }

    // Update is called once per frame
    void Update()
    {
        Material currentMaterial = this.GetComponent<Renderer>().material;
        if (sensor.hidden)
        {
            float sensorValue = sensor.sensorValue;
            if (sensorValue <= lowerThreshold)
            {
                currentMaterial.color = goodColour;
            } else if (sensorValue >= upperThreshold)
            {
                currentMaterial.color = badColour;
            } else
            {
                currentMaterial.color = Color.Lerp(goodColour, badColour, (sensorValue - lowerThreshold) / (upperThreshold - lowerThreshold));
            }
            this.GetComponent<Renderer>().material = currentMaterial;
        }
    }
}
