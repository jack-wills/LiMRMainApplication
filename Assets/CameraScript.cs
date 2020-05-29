using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;

    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target.position, smoothSpeed);
        transform.position = smoothedPosition;
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, target.rotation, smoothSpeed);
        transform.rotation = smoothedRotation;
    }
}
