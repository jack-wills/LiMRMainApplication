using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using SimpleJSON;
using System;

public class ImageDisplay : MonoBehaviour
{
    private float nextActionTime = 0.0f;
    public float period = 1.0f;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            StartCoroutine(GetRequest("https://sqs.us-east-1.amazonaws.com/592986159531/ImageQueue?Action=ReceiveMessage&MaxNumberOfMessages=5&Version=2012-11-05"));
        }
    }

    void CreateNewImage(string url, float x, float y, float z, float angle)
    {
        GameObject img1obj = new GameObject("img" + count);
        count++;
        img1obj.transform.SetParent(this.gameObject.transform);
        RawImage img1 = img1obj.AddComponent(typeof(RawImage)) as RawImage;
        img1.rectTransform.sizeDelta = new Vector2(1, 1);
        img1.rectTransform.localPosition = new Vector3(x, y-0.5f, z);
        img1.rectTransform.eulerAngles = new Vector3(0, angle, 0);
        StartCoroutine(DownloadImage(url, img1));
    }

    IEnumerator DownloadImage(string MediaUrl, RawImage img)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            img.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
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
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(webRequest.downloadHandler.text);
                XmlNodeList body = xml.GetElementsByTagName("Body");
                if (body[0] != null)
                {
                    try
                    {
                        string receiptHandle = xml.GetElementsByTagName("ReceiptHandle")[0].InnerText;
                        Debug.Log(body[0].InnerText);
                        var json = JSON.Parse(body[0].InnerText);
                        float x = float.Parse(json["x"]);
                        float y = float.Parse(json["y"]);
                        float z = float.Parse(json["z"]);
                        float angle = float.Parse(json["angle"]);
                        string url = json["url"];
                        CreateNewImage(url, x, y, z, angle);
                        StartCoroutine(GetRequest("https://sqs.us-east-1.amazonaws.com/592986159531/ImageQueue?Action=DeleteMessage&ReceiptHandle=" + UnityWebRequest.EscapeURL(receiptHandle) + "&Version=2012-11-05"));
                    } catch (Exception e)
                    {
                        Debug.Log(e.ToString());
                    }
                }
            }
        }
    }
}
