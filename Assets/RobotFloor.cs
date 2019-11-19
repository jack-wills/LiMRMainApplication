using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class RobotFloor : MonoBehaviour
{
    public GameObject robotPointerCursor;
    public GameObject ghostRobot;

    private string url = "https://bmgxwpyyd2.execute-api.us-east-1.amazonaws.com/prod/sensordata";

    public void MoveRobot()
    {
        ghostRobot.GetComponent<Transform>().position = robotPointerCursor.GetComponent<Transform>().position;
        StartCoroutine(PostRequest(url, ghostRobot.GetComponent<Transform>().position));
    }

    IEnumerator PostRequest(string uri, Vector3 position)
    {

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"timestamp\": \"" + System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + "\", \"sensorValues\": \"{\\\"robotGoToX\\\":\\\"" + position.x + "\\\", \\\"robotGoToY\\\":\\\"" + position.z + "\\\"}\" }");
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
