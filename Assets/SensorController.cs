using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using SimpleJSON;

public class SensorController : MonoBehaviour
{

    private string url = "https://bmgxwpyyd2.execute-api.us-east-1.amazonaws.com/prod/sensordata";
    private float nextActionTime = 0.0f;
    public float period = 1.0f;
    private JSONNode sensorValues;
    // Start is called before the first frame update
    public GameObject ML_Prediction_false;
    public GameObject ML_Prediction_true;
    void Start()
    {
        StartCoroutine(GetRequest(url));
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            StartCoroutine(GetRequest(url));
            // execute block of code here
        }
        //ml prediction warning sign
        float CurrentNum = GetSensorValue("ml_prediction");
        if (CurrentNum == -1)
        {
            ML_Prediction_false.SetActive(true);
            ML_Prediction_true.SetActive(false);
        }
        else if (CurrentNum == 1)
        {
            ML_Prediction_true.SetActive(true);
            ML_Prediction_false.SetActive(false);
        }
    }

  

    public float GetSensorValue(string sensorID)
    {
        if (sensorValues != null)
        {
            if (sensorValues[sensorID] != null)
            {
                return sensorValues[sensorID].AsFloat;
            }
            else
            {
                Debug.LogError("\"" + sensorID + "\" is not a valid sensor ID.");
                return 0;
            }
        }
        return 0;
    }

    public string GetSensorValueString(string sensorID)
    {
        if (sensorValues[sensorID] != null)
        {
            return sensorValues[sensorID];
        }
        else
        {
            Debug.LogError("\"" + sensorID + "\" is not a valid sensor ID.");
            return "";
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                var json = JSON.Parse(webRequest.downloadHandler.text);
                string sensorValuesString = json["sensorValues"];

                sensorValues = JSON.Parse(sensorValuesString);
            }
        }
    }
}
