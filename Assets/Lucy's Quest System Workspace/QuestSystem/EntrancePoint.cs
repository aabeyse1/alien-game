// Author(s): Lucy Rubin

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))] // require this component to be attatched to the same gameobject that this script is attatched to 
public class EntrancePoint : MonoBehaviour
{
    [Header("Entrance Point Info")]

    [SerializeField] private string entrancePointName;
    [SerializeField] private bool locked = false; 
    
    [SerializeField] private GameObject positionToChangeTo;

    [Header("Player Info")]
    [SerializeField] private GameObject playerCharacter; 
    
    private bool playerIsNear = false; 
    
    private InteractIcon interactIcon;

    private void Awake() {
        interactIcon = GetComponentInChildren<InteractIcon>();
    }

    private void OnEnable() {
        GameEventsManager.instance.inputEvents.onSubmitPressed += SubmitPressed;
    }

     private void OnDisable() {
        GameEventsManager.instance.inputEvents.onSubmitPressed -= SubmitPressed;

    }

    private void SubmitPressed() {
       
        if (!playerIsNear) {
            return;
        }
        ChangeArea();
    }

    private void ChangeArea()
    {
        CameraFade cameraFade = Camera.main.GetComponent<CameraFade>();
        if (cameraFade != null)
        {
            cameraFade.StartFadeOutAndIn(() =>
            {
                playerCharacter.transform.position = positionToChangeTo.transform.position;
                GameEventsManager.instance.playerEvents.PlayerAreaChange(entrancePointName);
            });
        }
        else
        {
            Debug.LogError("CameraFade component is not found on the main camera.");
        }
    }



    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.CompareTag("Player")) {
            interactIcon.SetState(active: true, locked: locked);
            playerIsNear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D otherCollider) {
         if (otherCollider.CompareTag("Player")) {
            playerIsNear = false;
            interactIcon.SetState(active: false, locked: locked);
        }
    }
   
}
