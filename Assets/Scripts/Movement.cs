using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody2D rigidBody;
    private Animator animator;

    public DialogueManager dialogueManager;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 

        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (dialogueManager.dialogueRunner.IsDialogueRunning)
        {
            rigidBody.velocity = Vector2.zero;
            animator.SetInteger("Direction", 0);
            return;
        }

        Vector2 movement = Vector2.zero;

        // Check each direction independently
        if (Input.GetKey(KeyCode.W))
        {
            movement.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement.y -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement.x -= 1;
        }

        movement.Normalize(); // Normalize the vector to maintain consistent speed in all directions

        // Set the velocity based on the movement direction
        rigidBody.velocity = movement * speed;

        // Set the animation parameter based on the movement direction
        if (movement != Vector2.zero)
        {
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                animator.SetInteger("Direction", movement.x > 0 ? 3 : 4); // Right or Left
            }
            else
            {
                animator.SetInteger("Direction", movement.y > 0 ? 1 : 2); // Up or Down
            }
        }
        else
        {
            animator.SetInteger("Direction", 0); // Idle
        }
    }
}
