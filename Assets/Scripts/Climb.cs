using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Climb : MonoBehaviour
{
    InputActionMap inputActionMap;
    private Rigidbody physics;
    public float climbingSpeed;
    public GameObject raycastOrigin;
    public float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        inputActionMap = GetComponent<PlayerInput>().currentActionMap;
        physics = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveClimbing();
    }

    public void MoveClimbing() 
    {
        if (Input.GetButton("GamepadButtonEast") && Physics.Raycast(raycastOrigin.transform.position, this.transform.forward, 0.7f))
        {
            this.GetComponent<Climb>().enabled = true;
            this.GetComponent<Movement>().enabled = false;
            physics.useGravity = false;
            Vector2 axis = inputActionMap.FindAction("HorizontalMovement").ReadValue<Vector2>();
            Vector3 step = new Vector3(0.0f, axis.y * Time.deltaTime * climbingSpeed, 0.0f);
            this.transform.Translate(step);
            if (Input.GetButtonDown("GamepadButtonSouth"))
            {
                physics.AddForce(new Vector3(-1, 1, 0) * jumpForce, ForceMode.Impulse);
            }
        }
        else
        {
            physics.useGravity = true;
            this.GetComponent<Movement>().enabled = true;
        }

    }

}
