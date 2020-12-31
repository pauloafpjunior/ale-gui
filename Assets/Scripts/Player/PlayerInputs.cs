using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerInputs : MonoBehaviour
{
    public float horizontal { get; private set; }
    public bool jump { get; private set; }

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
        jump = false;
        readyToClear = false;
    }

    private void ProcessInputs() 
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        jump =  jump || Input.GetButtonDown("Jump");

        print(horizontal + " - " + jump);
    }
}
