using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoyoteTime : MonoBehaviour
{
    public float jumpForce;
    public float coyoteTime;
    private float coyoteTimeCounter;
    private bool canCoyoteTime;
    private Rigidbody physics;
    private bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Movement>().jump = CoyoteJump;
        physics = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canCoyoteTime && !grounded)
        {
            physics.useGravity = false;
            coyoteTimeCounter += Time.deltaTime;
            physics.velocity = new Vector3(physics.velocity.x, 0, physics.velocity.z);
            if(coyoteTimeCounter > coyoteTime)
            {
                canCoyoteTime = false;
                physics.useGravity = true;
            }
        }
        
        
    }

    public void CoyoteJump()
    {
        Debug.Log("Salto");
        Debug.Log("Grounded: " + grounded);
        Debug.Log("CoyoteTime: " + canCoyoteTime);
        if (grounded || canCoyoteTime)
        {
            physics.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canCoyoteTime = false;
            physics.useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = true;
            canCoyoteTime = true;
            coyoteTimeCounter = 0;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = false;
        }
        
    }
}
