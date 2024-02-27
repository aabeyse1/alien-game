using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public float smoothing = 2f;

    Vector3 offset;

    void Start()
    {
        offset = new Vector3(0,0,0);
        CenterCameraOnTarget();
    }

    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
    
    void CenterCameraOnTarget()
    {
        // Vector3 desiredPosition = target.position + offset;
        // transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothing * Time.deltaTime);
        transform.position = target.position + offset;
    }
}
