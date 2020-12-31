using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputs playerInputs;
    private Rigidbody2D rb;
    private float speed = 5;
    private int direction = 1;

    void Awake() 
    {
        playerInputs = GetComponent<PlayerInputs>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        GroundMovement();
    }

    private void GroundMovement() 
    {
        float xVelocity = speed * playerInputs.horizontal;

        if (direction * xVelocity < 0) {
            Flip();
        }

        rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    private void Flip() 
    {
        direction *= -1;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale; 
    }

}
