using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class PostDataCords : MonoBehaviour
{
    public GameObject drone;
    private Vector3 Cords;
    private Vector3 Rots;

    private string url = "https://bmgxwpyyd2.execute-api.us-east-1.amazonaws.com/prod/sensordata";
    public string sensorID;
    private string value;

    // Start is called before the first frame update
    void Start()
    {
        drone = GameObject.Find("AxisLockedDrone");
    }

    // Update is called once per frame
    void Update()
    {
        Cords = drone.transform.position;
        Rots = drone.transform.eulerAngles;

        //value = writeVectorProperly(Cords);
        
    }

   

    public void SendCords()
    {
        
        StartCoroutine(PostRequest(url, Cords, Rots));
    }

    IEnumerator PostRequest(string uri, Vector3 position, Vector3 rotation)
    {

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"timestamp\": \"" + System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + "\", \"sensorValues\": \"{\\\"Drone.Roty\\\":\\\"" + rotation.y + "\\\", \\\"Drone.Posy\\\":\\\"" + position.y + "\\\", \\\"Drone.Rotx\\\":\\\"" + rotation.x + "\\\"}\" }");
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

    public string writeVectorProperly(Vector3 temp)
    {
        string final;
        final = temp.ToString("F3");
        Debug.Log(final);

        return final;
    }

    public void ButtonPress()
    {
        StartCoroutine(PostRequest(url));
    }

    IEnumerator PostRequest(string uri)
    {

        var request = new UnityWebRequest(url, "POST");
        //byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"timestamp\": \"" + System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + "\", \"sensorValues\": \"{\\\"" + sensorID + "\\\":\\\"" + value + "\\\"}\" }");
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
