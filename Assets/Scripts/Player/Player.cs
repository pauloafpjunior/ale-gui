using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Inputs
    private float horizontal;
    private bool jumpPressed, attackPressed;

    // Touch inputs
    private bool isLeftButtonPressed, isRightButtonPressed, isJumpButtonPressed, isAttackButtonPressed;
    
    [SerializeField] private float speed = 5;
    private Rigidbody2D rb;
    private int direction = 1;
    private float maxFallSpeed = -10f;
    private float jumpForce = 8f;

    // Ground check
    [SerializeField] private LayerMask groundLayer;
    private bool isOnGround;
    private float xOffset = .4f; 
    private float yOffset = .05f;
    private float groundDistance = .2f;

    // Jump
    private float coyoteTime;
    private float coyoteDuration = 0.1f;    

    // Animation
    private Animator anim;

    // Attack
    private bool isAttacking;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        ProcessInputs();   
    }

    void FixedUpdate()
    {
        GroundMovement();
        GroundCheck();  
        JumpMovement();   
        AttackMovement();   
    }

    private RaycastHit2D Raycast(Vector2 offset, Vector2 direction, float length) 
    {
        Vector2 currentPlayerPosition = transform.position;
        return Physics2D.Raycast(currentPlayerPosition + offset, direction, length, groundLayer);
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

    private void GroundMovement() 
    {
        float xVelocity = speed * horizontal;
        float yVelocity = rb.velocity.y;

        anim.SetInteger("xVelocity", Mathf.Abs((int) xVelocity));

        if (direction * xVelocity < 0) {
            Flip();
        }

        rb.velocity = new Vector2(xVelocity, yVelocity);    

        if (isOnGround) {
            coyoteTime = Time.time + coyoteDuration;
        }   
    }

    public void JumpMovement() 
    {
        if (jumpPressed && (isOnGround || coyoteTime > Time.time)) {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        if (rb.velocity.y < maxFallSpeed) {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }

    private void AttackMovement() 
    {
        if (attackPressed && !isAttacking) {
            anim.SetTrigger("attack");
            isAttacking = true;
        }
    }

    private void OnAttackFinished() 
    {
        isAttacking = false;
    }

    private void Flip() 
    {
        direction *= -1;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale; 
    }

    private void ProcessInputs() 
    {
        if (isLeftButtonPressed)  horizontal = -1.0f;
        else if (isRightButtonPressed) horizontal = 1.0f;
        else horizontal = Input.GetAxisRaw("Horizontal");

        jumpPressed = isJumpButtonPressed || Input.GetButtonDown("Jump");
        attackPressed = isAttackButtonPressed || Input.GetButtonDown("Fire1");
    }

    public void OnLeftButtonPressed() { isLeftButtonPressed = true; }
    public void OnLeftButtonReleased() { isLeftButtonPressed = false; }

    public void OnRightButtonPressed() { isRightButtonPressed = true; }
    public void OnRightButtonReleased() { isRightButtonPressed = false; }

    public void OnJumpButtonPressed() { isJumpButtonPressed = true; }
    public void OnJumpButtonReleased() { isJumpButtonPressed = false; }

    public void OnAttackButtonPressed() { isAttackButtonPressed = true; }
    public void OnAttackButtonReleased() { isAttackButtonPressed = false; }
}
