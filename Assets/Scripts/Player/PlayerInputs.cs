using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerInputs : MonoBehaviour
{
    public float horizontal { get; private set; }
    public bool jumpPressed { get; private set; }

    private bool readyToClear;

    void FixedUpdate()
    {
        readyToClear = true;
    }


    void Update()
    {
        ClearInputs();
        ProcessInputs();    
    }

    private void ClearInputs() {
        if (!readyToClear) {
            return;
        }

        horizontal = 0f;
        jumpPressed = false;
        readyToClear = false;
    }

    private void ProcessInputs() 
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        jumpPressed =  jumpPressed || Input.GetButtonDown("Jump");
    }

    public void OnLeftButtonPressed() 
    {
        horizontal = -1f;
    }

    public void OnRightButtonPressed() 
    {
        horizontal = 1f;
    }

    public void JumpPressed() 
    {
        jumpPressed = true;
    }

    public void JumpReleased() 
    {
        jumpPressed = false;
    }
}
