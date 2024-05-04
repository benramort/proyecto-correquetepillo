using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    InputActionMap inputActionMap;
    public GameObject cam;
    public float speed;
    public float rotationSpeed;
    public float jumpForce;
    private Rigidbody physics;
    private bool grounded;

    private Vector2 jumpDirectionInput = new Vector2 (0,0);
    private Vector3 jumpDirectionForward = new Vector3 (0,0,0);
    //public GameObject camera;
    void Start()
    {
        inputActionMap = GetComponent<PlayerInput>().actions.FindActionMap("Player");
        physics = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(Vector3.Angle(jumpDirection, transform.forward));
        manageHorizontalMovement();
        manageCamera();
        //transform.rotation = Quaternion.identity;
    }

    private void manageHorizontalMovement()
    {
        Vector2 axis = inputActionMap.FindAction("HorizontalMovement").ReadValue<Vector2>();

        //Debug.Log(Vector3.Angle(new Vector2(jumpDirection.x, jumpDirection.y), axis));
        //Debug.DrawRay(transform.position, new Vector2(jumpDirection.x, jumpDirection.z));
        //Debug.DrawRay(transform.position, axis);
        Debug.Log(Vector3.Angle(jumpDirectionForward, transform.forward));
        //Debug.Log(transform.forward);

        float temporalSpeed = speed;
        if (!grounded)
        {
            //Debug.Log("Hola1");
            if (Vector3.Angle(jumpDirectionInput, axis) > 45 || Vector3.Angle(jumpDirectionForward, transform.forward) > 45)
            {
                temporalSpeed *= 0.4f;
                //Debug.Log("Hola2");
            }
        }
        Debug.Log(temporalSpeed);


        Vector3 step = new Vector3(axis.x * Time.deltaTime * temporalSpeed, 0.0f, axis.y * Time.fixedDeltaTime * temporalSpeed);
        this.transform.Translate(step);
        //this.transform.position += step;

        if (step != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(step);
            physics.rotation = Quaternion.Slerp(physics.rotation, newRotation, rotationSpeed * Time.fixedDeltaTime);

        }

        //}
      

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
        if (context.performed)
        {
            if (grounded)
            {
                physics.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpDirectionInput = inputActionMap.FindAction("HorizontalMovement").ReadValue<Vector2>();
                jumpDirectionForward = transform.forward;
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
            //StartCoroutine(MovementInAirCoroutine());
        }

    }

    IEnumerator MovementInAirCoroutine()
    {
        while (speed >= 4 && !grounded)
        {
            speed -= 0.1f;
            yield return new WaitForFixedUpdate();
        }
    }
}
