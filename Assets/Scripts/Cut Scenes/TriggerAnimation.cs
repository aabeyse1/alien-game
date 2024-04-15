using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [SerializeField] GameObject animatedCharacter;
    [SerializeField] GameObject animationGameObject;
    [SerializeField] GameObject playerObject;

    [SerializeField] GameObject newBackground;
    [SerializeField] Transform afterAnimationPosition;
    [SerializeField] GameObject closedDoorCollider;

    [SerializeField] GameObject openDoorIcon;

    private Animator animator;
    private Renderer characterRenderer;
    public float tweenSpeed = 0.1f;

    private bool animationAlreadyRun = false;


    private void Start() {
        animator = animationGameObject.GetComponent<Animator>();
        characterRenderer = playerObject.GetComponent<Renderer>();
    }
     private void OnTriggerEnter2D(Collider2D otherCollider) {
        
        if (animationAlreadyRun) {
            return;
        }
        if (otherCollider.CompareTag("Player")) {
            Vector3 initialPosition = playerObject.transform.position;
            Vector3 targetPosition = animationGameObject.transform.position;
            targetPosition.z = 0;
            initialPosition.z = 0;
           
           // move to the position of the animation
            while (Vector3.Distance(playerObject.transform.position, targetPosition) > 0.001f) {
                 // Move our position a step closer to the target.
                var step =  tweenSpeed * Time.deltaTime; // calculate distance to move
                playerObject.transform.position = Vector3.MoveTowards(playerObject.transform.position, targetPosition, step);

            }
            

            
        //    characterRenderer.enabled = false;
        playerObject.SetActive(false);
           animatedCharacter.SetActive(true);
            StartCoroutine(OpenDoorAnimationCoroutine());
        }
    }

    

    

     private IEnumerator OpenDoorAnimationCoroutine()
    {
        animator.enabled = true;
        // Wait for the animation to reach its end
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        animator.enabled = false;
        animationAlreadyRun = true;
        continueAfterAnimation();
    }

    private void continueAfterAnimation() {
        
        playerObject.transform.position = playerObject.transform.position; // not sure why but this code seems to be needed in order to set the player object active again
        playerObject.SetActive(true); 
        animationGameObject.SetActive(false);
        newBackground.SetActive(true);
        closedDoorCollider.SetActive(false);
        
    }
    
}
