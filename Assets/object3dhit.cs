using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object3dhit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) | Input.GetMouseButtonUp(1))
        {
            RaycastHit hit;
            Vector3 pos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.gameObject.tag == "nextship")
                {
                    Debug.Log("Clicked");
                }
            }
        }
    }
}
