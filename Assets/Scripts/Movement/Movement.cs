using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public delegate void JumpDel();

    InputActionMap inputActionMap;
    public GameObject cam;
    public float speed;
    public float rotationSpeed;
    public float jumpForce;
    //public bool canJump;

    public JumpDel jump;

    private Rigidbody physics;
    private bool grounded;

    //For slow surface
    private int slowed = 0;
    private float orignalSpeed;

    //For ice
    private bool _onIce = false;
    private Vector3 iceDircetion;
    public bool OnIce { get => _onIce; set { _onIce = value; iceDircetion = physics.velocity;} }
    //public GameObject camera;
    void Start()
    {
        //canJump = true;
        if (GetComponent<DobleJump>() == null)
            jump = BasicJump;
        inputActionMap = GetComponent<PlayerInput>().actions.FindActionMap("Player");
        physics = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(physics.velocity);
        //Debug.Log(grounded);
        if (!_onIce)
        {
            manageHorizontalMovement();
        } else if (_onIce)
        {
            MoveForwardIce();
        }
        manageCamera();
        //transform.rotation = Quaternion.identity;
    }

    private void manageHorizontalMovement()
    {
        Vector2 axis = inputActionMap.FindAction("HorizontalMovement").ReadValue<Vector2>();
        Vector3 step = new Vector3(axis.x * speed, 0.0f, axis.y * speed);
        //physics.freezeRotation = true;
        //this.transform.Translate(step);
        //if(step != Vector3.zero)
        //{
        //    Quaternion newRotation = Quaternion.LookRotation(step);
        //    physics.rotation = Quaternion.Slerp(physics.rotation, newRotation, rotationSpeed * Time.fixedDeltaTime);

        //}
        physics.AddForce(step, ForceMode.Acceleration);


    }

    private void manageCamera()
    {
        Vector3 viewDirection = new Vector3(this.transform.position.x - cam.transform.position.x, 0, this.transform.position.z - cam.transform.position.z);
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
        if (context.performed && slowed == 0 && !_onIce)
        {
            jump();
        }
    }

    private void BasicJump()
    {
        if (grounded)
        {
            physics.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void Slow()
    {
        slowed++;
        if (slowed == 1)
        {
            orignalSpeed = speed;
            speed *= 0.2f;
        }
    }

    public void UnSlow()
    {
        slowed--;
        if (slowed <= 0)
        {
            speed = orignalSpeed;
        }
    }

    private void MoveForwardIce()
    {
        Vector3 axis = new Vector3(iceDircetion.x, 0, iceDircetion.z);
        axis.Normalize();
        Vector3 step = new Vector3(axis.x * Time.deltaTime * speed * 3, 0.0f, axis.z * Time.fixedDeltaTime * speed * 3);
        //physics.freezeRotation = true;
        this.transform.Translate(step);
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = false;
        }

    }
}
