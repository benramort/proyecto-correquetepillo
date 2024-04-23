using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    InputActionMap inputActionMap;
    public GameObject camera;
    public float speed;
    public float rotationSpeed;
    public float jumpForce;
    private Rigidbody physics;
    private bool grounded;
    //public GameObject camera;
    void Start()
    {
        inputActionMap = GetComponent<PlayerInput>().actions.FindActionMap("Player");
        physics = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        manageHorizontalMovement();
        manageCamera();
    }

    private void manageHorizontalMovement()
    {
        Vector2 axis = inputActionMap.FindAction("HorizontalMovement").ReadValue<Vector2>();
        Vector3 step = new Vector3(axis.x * Time.deltaTime * speed, 0.0f, axis.y * Time.fixedDeltaTime * speed);
        this.transform.Translate(step);
        if (step != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(step);
            physics.rotation = Quaternion.Slerp(physics.rotation, newRotation, rotationSpeed * Time.fixedDeltaTime);

        }


    }

    private void manageCamera()
    {
        Vector3 viewDirection = new Vector3(this.transform.position.x - camera.transform.position.x, 0, this.transform.position.z - camera.transform.position.z);
        viewDirection = viewDirection.normalized;
        float angle = Mathf.Atan2(viewDirection.x, viewDirection.z) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, angle, 0);
        //float angle = Vector3.Angle(viewDirection, transform.forward);
        //Debug.Log(angle);
        //characterDirection.Rotate(Vector3.up, angle);
        //if (angle > Mathf.PI)
        //{
        //    angle -= Mathf.PI;
        //}
        //transform.Rotate(Vector3.up, angle);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (grounded)
            {
                physics.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = true;
            speed = 8;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = false;
            speed = 2;
        }

    }
}
