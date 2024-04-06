// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private InteractIcon interactIcon;
    private bool playerIsNear = false; 

    [SerializeField] GameObject logPrefab;

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
       
        if (playerIsNear) {
            // chop down tree
           
            GameObject log = Instantiate(logPrefab);
            log.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);

            Destroy(this.gameObject);

        }
       
    }

     private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.CompareTag("Player")) {
            interactIcon.SetState(active: true, locked: false);
            playerIsNear = true;
            Debug.Log("near");
        }
    }
    private void OnTriggerExit2D(Collider2D otherCollider) {
         if (otherCollider.CompareTag("Player")) {
            playerIsNear = false;
            interactIcon.SetState(active: false, locked: false);
        }
    }
    
}
