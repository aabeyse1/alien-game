using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        Vector2 movement = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            movement.y = speed;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movement.y = -speed;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            movement.x = speed;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement.x = -speed;
        }

        rigidBody.velocity = movement;
    }
}
