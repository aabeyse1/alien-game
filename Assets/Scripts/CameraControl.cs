using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public float targetSize = 0.45f; // The target orthographic size during dialogue
    private float originalSize;
    private Camera cam; // Reference to the camera component
    public DialogueManager dialogueManager;
    public  GameObject animation;
    public float queenBeeZoom = 0.7f; 
    public float zoomThreshold = 5f; 
    public GameObject queenBee;
    public GameObject player; 

    void Start()
    {
        cam = GetComponent<Camera>(); // Get the Camera component
        originalSize = cam.orthographicSize; // Store the original orthographic size
    }

    void Update()
    {
        
        // Check if the dialogue is running from a public static bool in DialogueManager
        if (dialogueManager.dialogueRunner.IsDialogueRunning)
        {
            float distance = Vector3.Distance(player.transform.position, queenBee.transform.position);
            if (distance <= zoomThreshold) 
            {
                StartCoroutine(AdjustCameraSize(queenBeeZoom)); // Zoom in when near Queen Bee
            } 
            else 
            {
                StartCoroutine(AdjustCameraSize(targetSize)); // Zoom out to normal when not near
            }
        }
        else
        {
            if(!animation.activeSelf)
            {
                StartCoroutine(AdjustCameraSize(originalSize));
            } 
        }
    }

    private IEnumerator AdjustCameraSize(float targetSize)
    {
        float duration = 0.6f; // Duration of the size transition
        float elapsedTime = 0;
        float startSize = cam.orthographicSize;

        // Avoiding multiple coroutines running the same process
        if (Mathf.Abs(cam.orthographicSize - targetSize) > 0.01f)
        {
            while (elapsedTime < duration)
            {
                cam.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            cam.orthographicSize = targetSize;
        }
    }
}
