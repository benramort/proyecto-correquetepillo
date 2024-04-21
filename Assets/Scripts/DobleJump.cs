using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DobleJump : MonoBehaviour
{
    private Rigidbody physics;
    private bool grounded;
    private bool canDoubleJump;
    public float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        physics = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (grounded)
            {
                physics.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canDoubleJump = true;

            }
            else if (canDoubleJump)
            {
                physics.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canDoubleJump = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
}
