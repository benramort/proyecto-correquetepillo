using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum FreezeType { FALLFREEZE, CATCHFREEZE };

public class Movement : MonoBehaviour
{
    public delegate void JumpDel();
    private Animator animator;

    InputActionMap inputActionMap;
    public GameObject cam;
    public float speed;
    public float rotationSpeed;
    [Space(20)]
    public float jumpForce;
    //public bool canJump;

    public JumpDel jump;

    private Rigidbody physics;
    private bool grounded = true;
    private List<Collider> correctColliders = new List<Collider>();

    private Vector2 jumpDirectionInput = new Vector2 (0,0);
    private Vector3 jumpDirectionForward = new Vector3 (0,0,0);
    private bool freeze;
    
    private Vector3 velocity = new Vector3(1f, 1f, 1f);
    private Vector3 previousPosition = new Vector3(0,0,0);

    [Header("Aerial Movement")]
    [SerializeField] private float lerpPercentage;
    public bool lerping = true;

    public bool Grounded { get => grounded; set => grounded = value; }

    [Space(20)]
    //For slow surface
    private int slowed = 0;
    private float orignalSpeed;

    //For ice
    public int onIce { get; set; } = 0;
    //public GameObject camera;
    void Start()
    {
        //canJump = true;
        cam = transform.parent.Find("Camera").gameObject;
        animator = gameObject.GetComponentInChildren<Animator>();
        if (GetComponent<DobleJump>() == null && GetComponent<CoyoteTime>() == null)
        {
            jump = BasicJump;
        }
        
        inputActionMap = transform.parent.parent.GetComponent<PlayerInput>().actions.FindActionMap("Player");
        physics = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(physics.velocity);
        //Debug.Log(grounded);
        if (onIce == 0)
        {
            if (!freeze)
                manageHorizontalMovement();
        } else
        {
            MoveForwardIce();
        }
        manageCamera();
        
        CalculateVelocity();
        //Debug.Log("Velocity: " + velocity);
        //transform.rotation = Quaternion.identity;
        Debug.Log("Grounded: " + grounded);
    }

    private void manageHorizontalMovement()
    {
        Vector2 axis = inputActionMap.FindAction("HorizontalMovement").ReadValue<Vector2>();
        if (axis.y > 0.1 || axis.x > 0.1 || axis.x < -0.1)
        {
            animator.ResetTrigger("walkingBackwards");
            animator.SetTrigger("running");
        }
        else if (axis.y < -0.1)
        {
            animator.ResetTrigger("running");
            animator.SetTrigger("walkingBackwards");
        }
        else if (axis.y == 0f)
        {
            animator.ResetTrigger("walkingBackwards");
            animator.ResetTrigger("running");
        }
        //Debug.Log(Vector3.Angle(new Vector2(jumpDirection.x, jumpDirection.y), axis));
        //Debug.DrawRay(transform.position, new Vector2(jumpDirection.x, jumpDirection.z));
        //Debug.DrawRay(transform.position, axis);
        //Debug.Log(Vector3.Angle(jumpDirectionForward, transform.forward));
        //Debug.Log(transform.forward);

        Vector2 temporalSpeed = new Vector3(axis.x * speed, axis.y * speed);
        
        if (!grounded && lerping)
        {
            Vector3 localVelocity = transform.InverseTransformDirection(velocity); //Pasar la velocidad a coordenadas locales
            Vector2 targetSpeed = temporalSpeed;
            Debug.DrawRay(transform.position, new Vector3(localVelocity.x, 0, localVelocity.z), Color.red);

            Debug.DrawRay(transform.position, new Vector3(targetSpeed.x, 0, targetSpeed.y), Color.blue);
            //temporalSpeed = Vector3.Lerp(speed, new Vector3)
            temporalSpeed = Vector2.Lerp(targetSpeed, new Vector2(localVelocity.x, localVelocity.z), lerpPercentage); //Esto puede interactuar raro con el framerate
            Debug.DrawRay(transform.position, new Vector3(temporalSpeed.x, 0, temporalSpeed.y));

        }
        //Debug.Log(temporalSpeed);


        Vector3 step = new Vector3(Time.fixedDeltaTime * temporalSpeed.x, 0.0f, Time.fixedDeltaTime * temporalSpeed.y);
        this.transform.Translate(step);
        //transform.position += transform.forward;
        //Debug.Log(step);
        //physics.velocity = new Vector3(temporalSpeed.x, physics.velocity.y, temporalSpeed.y);
        //physics.MovePosition(physics.position + step);
        //this.transform.Translate()
        //this.transform.localPosition += transform.TransformDirection(step);

        //if (step.magnitude > 0.1)
        //{
        //    Quaternion newRotation = Quaternion.LookRotation(step);
        //    physics.rotation = Quaternion.Slerp(physics.rotation, newRotation, rotationSpeed * Time.fixedDeltaTime);
        //}


    }

    private void manageCamera()
    {
        //Debug.Log("asfdaf");
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

    private void CalculateVelocity()
    {
        velocity = (transform.position - previousPosition) / Time.fixedDeltaTime;
        previousPosition = transform.position;
        //Debug.Log(velocity);
        //Debug.Log(physics.velocity);
    }

    public void Freeze(FreezeType type)
    {
        switch (type)
        {
            case FreezeType.CATCHFREEZE:
                StartCoroutine(FreezeCoroutine(5));
                break;
            default: 
                break;
        }
    }

    private IEnumerator FreezeCoroutine(int time)
    {
        freeze = true;
        yield return new WaitForSeconds(time);
        freeze = false;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && slowed == 0 && onIce == 0)
        {
            jump();
        }
    }

    private void BasicJump()
    {
        if (grounded)
        {
            animator.ResetTrigger("grounded");
            animator.SetTrigger("jump");
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
        
        if (velocity.magnitude > 0.1 || velocity.magnitude < -0.1)
        {
            velocity = transform.InverseTransformDirection(velocity.x, 0, velocity.z);
            Vector3 axis = new Vector3(velocity.x, 0, velocity.z);
            axis.Normalize();
            Vector3 step = new Vector3(axis.x * Time.deltaTime * speed * 3, 0.0f, axis.z * Time.fixedDeltaTime * speed * 3);
            //physics.freezeRotation = true;
            this.transform.Translate(step);
        } else
        {
            manageHorizontalMovement();
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if(collision.GetContact(0).normal.y > 0.9f)
            {
                lerping = true;
                correctColliders.Add(collision.collider);
                animator.SetTrigger("grounded");
                Grounded = true;
            }
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && correctColliders.Contains(collision.collider))
        {
            correctColliders.Remove(collision.collider);
            animator.ResetTrigger("grounded");
            Grounded = false;
        }

    }

    IEnumerator MovementInAirCoroutine()
    {
        while (speed >= 4 && !Grounded)
        {
            speed -= 0.1f;
            yield return new WaitForFixedUpdate();
        }
    }
}
