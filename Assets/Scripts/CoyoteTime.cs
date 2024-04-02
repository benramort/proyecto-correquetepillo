using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoyoteTime : MonoBehaviour
{
    public float jumpForce;
    public float coyoteTime;
    private float coyoteTimeCounter;
    private Rigidbody physics;
    private bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        physics = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CoyoteJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (grounded)
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }
            if (Input.GetButtonDown("GamepadButtonSouth") && coyoteTimeCounter > 0)
            {
                physics.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
}
