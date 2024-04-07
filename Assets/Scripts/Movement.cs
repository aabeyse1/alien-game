using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody2D rigidBody;
    private Animator animator;

    private float lastUpPress = float.MinValue;
    private float lastDownPress = float.MinValue;
    private float lastRightPress = float.MinValue;
    private float lastLeftPress = float.MinValue;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 

        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        Vector2 movement = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.W)) lastUpPress = Time.time;
        if (Input.GetKeyDown(KeyCode.S)) lastDownPress = Time.time;
        if (Input.GetKeyDown(KeyCode.D)) lastRightPress = Time.time;
        if (Input.GetKeyDown(KeyCode.A)) lastLeftPress = Time.time;

        float latestKeyPressTime = Mathf.Max(lastUpPress, lastDownPress, lastRightPress, lastLeftPress);

        if (latestKeyPressTime == lastRightPress && Input.GetKey(KeyCode.D))
        {
            movement.x = speed;
            animator.SetInteger("Direction", 3); 
        }
        else if (latestKeyPressTime == lastLeftPress && Input.GetKey(KeyCode.A))
        {
            movement.x = -speed;
            animator.SetInteger("Direction", 4); 
        }
        else if (latestKeyPressTime == lastUpPress && Input.GetKey(KeyCode.W))
        {
            movement.y = speed;
            animator.SetInteger("Direction", 1); 
        }
        else if (latestKeyPressTime == lastDownPress && Input.GetKey(KeyCode.S))
        {
            movement.y = -speed;
            animator.SetInteger("Direction", 2); 
        }

        rigidBody.velocity = movement;

        if(movement == Vector2.zero)
        {
            animator.SetInteger("Direction", 0); 
        }
    }
}
