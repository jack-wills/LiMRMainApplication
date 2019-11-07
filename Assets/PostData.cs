using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class PostData : MonoBehaviour
{
    private string url = "https://bmgxwpyyd2.execute-api.us-east-1.amazonaws.com/prod/sensordata";
    public string sensorID;
    public string value;

    public void ButtonPress()
    {
        StartCoroutine(GetRequest(url));
    }

    IEnumerator GetRequest(string uri)
    {

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"timestamp\": \"" + System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + "\", \"sensorValues\": \"{\\\"" + sensorID + "\\\":\\\"" + value + "\\\"}\" }");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        string[] pages = uri.Split('/');
        int page = pages.Length - 1;

        if (request.isNetworkError)
        {
            Debug.LogError(pages[page] + ": Error: " + request.error);
        }
        else
        {
            Debug.Log(pages[page] + ":\nReceived: " + request.downloadHandler.text);
        }
    }
}
