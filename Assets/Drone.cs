using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class Drone : MonoBehaviour
{


    public float dronespeed;
    public float rotationSpeed;
    private bool isDroneMove = false;
    private Menu menu;

    private GameObject robot;
    private GameObject drone;

    private Vector3 Cords;
    private Vector3 Rots;

    private Vector3 robotCords;

    private string url = "https://bmgxwpyyd2.execute-api.us-east-1.amazonaws.com/prod/sensordata";


    // Start is called before the first frame update
    void Start()
    {
        drone = GameObject.Find("AxisLockedDrone");
        robot = GameObject.Find("Robot");
        transform.position = new Vector3(transform.position.x, 0.44f, transform.position.z);
        menu = GameObject.Find("Menu").GetComponent<Menu>();
    }

    // Update is called once per frame
    void Update()
    {
        Cords = drone.transform.localPosition;
        Rots = drone.transform.eulerAngles;
        robotCords = robot.transform.localPosition;
        if (menu.IsDroneMove())
        {
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {

                StartCoroutine(DroneReturn());
                StartCoroutine(PostRequest(url, Cords, Rots, robotCords));
            }
            Movement();
        }
    }

    void Movement()
    {

        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * dronespeed;
        float rotation = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;
        transform.Translate(0, vertical, 0);
        transform.Rotate(Vector3.up,rotation);


        if (transform.position.y < 0.44f)
        {
            transform.position = new Vector3(transform.position.x, 0.44f, transform.position.z);
        }
    }


    private IEnumerator DroneReturn()
    {
        Vector3 objPos = this.GetComponent<Transform>().position;

        Vector3 a = objPos;
        Vector3 b = new Vector3(objPos.x, 0.44f, objPos.z);

        float t = 0;
        while (t <= 1.0f)
        {
            t += (Time.deltaTime / 1.0f); // Goes from 0 to 1, incrementing by step each time
            transform.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        transform.position = b;

    }

    IEnumerator PostRequest(string uri, Vector3 position, Vector3 rotation, Vector3 robotPosition)
    {

        var request = new UnityWebRequest(url, "POST");
        Debug.Log("{\"timestamp\": \"" + System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + "\", \"sensorValues\": \"{\\\"droneImageRot\\\":\\\"" + rotation.y + "\\\", \\\"droneImageY\\\":\\\"" + position.y + "\\\", \\\"droneImageX\\\":\\\"" + robotPosition.x + "\\\", \\\"droneImageZ\\\":\\\"" + robotPosition.z + "\\\"}\" }");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"timestamp\": \"" + System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + "\", \"sensorValues\": \"{\\\"droneImageRot\\\":\\\"" + rotation.y + "\\\", \\\"droneImageY\\\":\\\"" + position.y + "\\\", \\\"droneImageX\\\":\\\"" + robotPosition.x + "\\\", \\\"droneImageZ\\\":\\\"" + robotPosition.z + "\\\"}\" }");

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
