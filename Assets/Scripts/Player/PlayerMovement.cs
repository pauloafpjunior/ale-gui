using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputs playerInputs;
    private Rigidbody2D rb;

    private float speed = 5;
    private int direction = 1;
    
    [SerializeField] private LayerMask groundLayer;
    private float xOffset = 0.4f;
    private float yOffset = 0.05f;
    private float groundDistance = 0.2f;
    
    private bool isOnGround;
    private float jumpForce = 8.5f;
    private bool isJumping;
    private float coyoteDuration = 0.1f;
    private float coyoteTime;
    private float maxFallSpeed = -25f;

    void Awake() 
    {
        playerInputs = GetComponent<PlayerInputs>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        GroundCheck();
        GroundMovement();
        Jump();
    }

    private void GroundMovement() 
    {
        float xVelocity = speed * playerInputs.horizontal;

        if (direction * xVelocity < 0) {
            Flip();
        }

        rb.velocity = new Vector2(xVelocity, rb.velocity.y);

        if (isOnGround) {
            coyoteTime = Time.time + coyoteDuration;
        }
    }

    public void Jump() 
    {
        if (playerInputs.jumpPressed && !isJumping && (isOnGround || coyoteTime > Time.time)) {
            isOnGround = false;
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        } else {
            isJumping = false;
        }

        if (rb.velocity.y < maxFallSpeed) {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }

    private void Flip() 
    {
        direction *= -1;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale; 
    }

    private RaycastHit2D Raycast(Vector2 offset, Vector2 direction, float length) 
    {
        Vector2 currentPlayerPosition = transform.position;
        return Physics2D.Raycast(currentPlayerPosition + offset, direction, length, groundLayer);
        // Color color = hit ? Color.red : Color.green;
        // Debug.DrawRay(currentPlayerPosition + offset, direction * length, color);
        // return hit;
    }

    private void GroundCheck()
    {
        isOnGround = false;

		RaycastHit2D leftFootCheck = Raycast(new Vector2(-xOffset, yOffset), Vector2.down, groundDistance);
		RaycastHit2D rightFootCheck = Raycast(new Vector2(xOffset, yOffset), Vector2.down, groundDistance);
    
        if (leftFootCheck || rightFootCheck) {
            isOnGround = true;
        }
    }

}
