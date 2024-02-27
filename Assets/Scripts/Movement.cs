using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody2D rigidBody;
    private Animator animator; // Reference to the Animator component

    // Variables to track the last time a key was pressed
    private float lastUpPress = float.MinValue;
    private float lastDownPress = float.MinValue;
    private float lastRightPress = float.MinValue;
    private float lastLeftPress = float.MinValue;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Initialize the Animator component
    }

    void Update()
    {
        Vector2 movement = Vector2.zero;

        // Update the time of the key press
        if (Input.GetKeyDown(KeyCode.UpArrow)) lastUpPress = Time.time;
        if (Input.GetKeyDown(KeyCode.DownArrow)) lastDownPress = Time.time;
        if (Input.GetKeyDown(KeyCode.RightArrow)) lastRightPress = Time.time;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) lastLeftPress = Time.time;

        // Determine the most recent key press
        float latestKeyPressTime = Mathf.Max(lastUpPress, lastDownPress, lastRightPress, lastLeftPress);

        if (latestKeyPressTime == lastRightPress && Input.GetKey(KeyCode.RightArrow))
        {
            movement.x = speed;
            animator.SetInteger("Direction", 3); // Assuming 2 represents right movement
        }
        else if (latestKeyPressTime == lastLeftPress && Input.GetKey(KeyCode.LeftArrow))
        {
            movement.x = -speed;
            animator.SetInteger("Direction", 4); // Assuming 4 represents left movement
        }
        else if (latestKeyPressTime == lastUpPress && Input.GetKey(KeyCode.UpArrow))
        {
            movement.y = speed;
            animator.SetInteger("Direction", 1); // Assuming 1 represents upward movement
        }
        else if (latestKeyPressTime == lastDownPress && Input.GetKey(KeyCode.DownArrow))
        {
            movement.y = -speed;
            animator.SetInteger("Direction", 2); // Assuming 3 represents downward movement
        }

        rigidBody.velocity = movement;

        // Reset to idle if no movement
        if(movement == Vector2.zero)
        {
            animator.SetInteger("Direction", 0); // Back to idle
        }
    }
}
