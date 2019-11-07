using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sensor : MonoBehaviour
{

    public float fadeTime;
    public Color lineColour;
    public string sensorID;

    private GameObject playerObject;
    private LineRenderer lineRenderer;
    private Text sensorText;
    private Canvas sensorTextCanvas;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("OVRPlayerController");
        GameObject sensorTextCanvasGO = new GameObject("Canvas");
        RectTransform sensorTextCanvasTrans = sensorTextCanvasGO.AddComponent<RectTransform>();
        sensorTextCanvasTrans.anchoredPosition = new Vector2(0, 0);
        sensorTextCanvasTrans.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        sensorTextCanvasTrans.sizeDelta = new Vector2(2300, 500);
        sensorTextCanvas = sensorTextCanvasGO.AddComponent<Canvas>();

        GameObject sensorTextGO = new GameObject("Text2");
        sensorTextGO.transform.SetParent(sensorTextCanvasGO.transform);

        RectTransform sensorTextTrans = sensorTextGO.AddComponent<RectTransform>();
        sensorTextTrans.anchoredPosition = new Vector2(0, 0);
        sensorTextTrans.localScale = new Vector3(5, 5, 1);
        sensorTextTrans.sizeDelta = new Vector2(300, 60);

        sensorText = sensorTextGO.AddComponent<Text>();
        sensorText.color = new Color(0, 0, 0, 0);
        sensorText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        sensorText.fontSize = 50;

        sensorTextCanvas.renderMode = RenderMode.WorldSpace;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.02f;
        lineRenderer.positionCount = 2;
        lineRenderer.positionCount = 2;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, this.GetComponent<Transform>().position);
        lineRenderer.SetPosition(1, this.GetComponent<Transform>().position);
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(lineColour, 0.0f), new GradientColorKey(lineColour, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 objPos = this.GetComponent<Transform>().position;
        Vector3 playerPos = playerObject.transform.position;
        objPos.y = 0;
        playerPos.y = 0;
        float angle = Mathf.Atan2(objPos.z - playerPos.z, objPos.x - playerPos.x) * Mathf.Rad2Deg;
        float sensorValue = GameObject.Find("SensorController").GetComponent<SensorController>().GetSensorValue(sensorID);
        sensorText.text = sensorValue.ToString();
        sensorText.transform.LookAt(Camera.main.transform);
        sensorText.transform.Rotate(new Vector3(0, 180, 0));

    }
    public void Show()
    {
        Material materialColored = new Material(Shader.Find("Diffuse"));
        materialColored.color = new Color(0.5f,1,1);
        this.GetComponent<Renderer>().material = materialColored;
        StopCoroutine(MoveInRoutine());
        StopCoroutine(FadeOutRoutine());
        StartCoroutine(FadeInRoutine());
        StartCoroutine(MoveOutRoutine());
    }
    public void Hide()
    {
        Material materialColored = new Material(Shader.Find("Diffuse"));
        materialColored.color = new Color(1, 1, 1);
        this.GetComponent<Renderer>().material = materialColored;
        StopCoroutine(MoveOutRoutine());
        StopCoroutine(FadeInRoutine());
        StartCoroutine(FadeOutRoutine());
        StartCoroutine(MoveInRoutine());
    }


    private IEnumerator MoveOutRoutine()
    {
        Vector3 objPos = this.GetComponent<Transform>().position;
        Vector3 playerPos = playerObject.transform.position;
        float angle = Mathf.Atan2(objPos.z - playerPos.z, objPos.x - playerPos.x);
        if (angle > 1.5708f)
        {
            angle -= 1.5708f;
        } else
        {
            angle += 1.5708f;
        }
        
        Vector3 a = objPos;
        Vector3 b = objPos + new Vector3(Mathf.Cos(angle), 1, Mathf.Sin(angle));

        RectTransform sensorTextRT = sensorText.GetComponent<RectTransform>();
        float t = 0;
        while (t <= 1.0f)
        {
            t += (Time.deltaTime / fadeTime); // Goes from 0 to 1, incrementing by step each time
            sensorTextRT.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            lineRenderer.SetPosition(1, sensorTextRT.position);
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        sensorTextRT.position = b;
        
    }

    private IEnumerator MoveInRoutine()
    {
        RectTransform sensorTextRT = sensorText.GetComponent<RectTransform>();
        Vector3 a = sensorTextRT.transform.position;
        Vector3 b = this.GetComponent<Transform>().position;
        
        float t = 0;
        while (t <= 1.0f)
        {
            t += (Time.deltaTime / fadeTime); // Goes from 0 to 1, incrementing by step each time
            sensorTextRT.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            lineRenderer.SetPosition(1, sensorTextRT.position);
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        sensorTextRT.position = b;
    }

    private IEnumerator FadeOutRoutine()
    { 
        while (sensorText.color.a > 0.0f)
        {
            sensorText.color = new Color(sensorText.color.r, sensorText.color.g, sensorText.color.b, sensorText.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }
    }
    private IEnumerator FadeInRoutine()
    { 
        while (sensorText.color.a < 1.0f)
        {
            sensorText.color = new Color(sensorText.color.r, sensorText.color.g, sensorText.color.b, sensorText.color.a + (Time.deltaTime / fadeTime));
            yield return null;
        }
    }
}