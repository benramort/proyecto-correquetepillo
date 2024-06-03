using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Timeline.TimelinePlaybackControls;

public class DobleJump : MonoBehaviour
{

    //public bool canJump = true;

    private Rigidbody physics;
    private bool canDoubleJump;
    public float jumpForce;
    public Movement movement;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        GetComponent<Movement>().jump = DoubleJump;
        //canJump = true;
        physics = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoubleJump()
    {
        if (movement.Grounded)
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
