using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sensor
{
    public class Sensor : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Show()
        {
            Material materialColored = new Material(Shader.Find("Diffuse"));
            materialColored.color = new Color(0.5f,1,1);
            this.GetComponent<Renderer>().material = materialColored;
        }
        public void Hide()
        {
            Material materialColored = new Material(Shader.Find("Diffuse"));
            materialColored.color = new Color(1, 1, 1);
            this.GetComponent<Renderer>().material = materialColored;
        }
    }
}