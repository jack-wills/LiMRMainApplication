using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using SimpleJSON;

public class SensorContoller : MonoBehaviour
{

    private string url = "https://bmgxwpyyd2.execute-api.us-east-1.amazonaws.com/prod/sensordata";
    public string sensorID = "sensor1";
    private float nextActionTime = 0.0f;
    public float period = 1.0f;
    public Text sensorText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest(url + "?sensorID=" + sensorID));
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            StartCoroutine(GetRequest(url + "?" + sensorID));
            // execute block of code here
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
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                var json = JSON.Parse(webRequest.downloadHandler.text);
                string sensorValues = json["sensorValues"];

                var sensorValuesJson = JSON.Parse(sensorValues);
                string sensorValue = sensorValuesJson[sensorID];
                sensorText.text = sensorValue;
            }
        }
    }
}
