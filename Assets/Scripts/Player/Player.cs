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
    private float jumpForce = 7.8f;

    // Ground check
    [SerializeField] private LayerMask groundLayer;
    private bool isOnGround;
    private float xOffset = .35f; 
    private float yOffset = .05f;
    private float groundDistance = .2f;

    // Jump
    private float coyoteTime;
    private float coyoteDuration = 0.1f;    

    // Animation
    private Animator anim;

    private bool isAttacking, isJumping;

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
        GroundCheck();  
        GroundMovement();
        if (jumpPressed) JumpMovement();   
        if (attackPressed) AttackMovement();   
    }

    private RaycastHit2D Raycast(Vector2 offset, Vector2 direction, float length) 
    {
        Vector2 currentPlayerPosition = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(currentPlayerPosition + offset, direction, length, groundLayer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(currentPlayerPosition + offset, direction * length, color);
        return hit;
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

        if (direction * xVelocity < 0) {
            Flip();
        }

        rb.velocity = new Vector2(xVelocity, yVelocity);    

        anim.SetInteger("xVelocity", Mathf.Abs((int) xVelocity));
        anim.SetBool("isOnGround", isOnGround);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("yVelocity", (int) yVelocity);

        if (isOnGround) {
            coyoteTime = Time.time + coyoteDuration;    
            isJumping = false;
        } 

        if (isAttacking && isOnGround) {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    public void JumpMovement() 
    {
        if (!isJumping && (isOnGround || coyoteTime > Time.time)) {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        if (rb.velocity.y < maxFallSpeed) {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }

    public void AttackMovement() 
    {
        if (!isAttacking) {
            anim.SetTrigger("attack");
            StartCoroutine("OnAttackFinished");
        }
    }

    IEnumerator OnAttackFinished() 
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.6f);
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

        jumpPressed = Input.GetButtonDown("Jump");
        attackPressed = Input.GetButtonDown("Fire1");
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
