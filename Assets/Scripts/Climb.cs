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
    // Start is called before the first frame update
    void Start()
    {
        inputActionMap = GetComponent<PlayerInput>().currentActionMap;
    }

    // Update is called once per frame
    void Update()
    {
        physics = GetComponent<Rigidbody>();
        MoveClimbing();
    }

    public void MoveClimbing() 
    {
        if (Input.GetButton("GamepadButtonEast") && Physics.Raycast(raycastOrigin.transform.position, this.transform.forward, 1.0f))
        {
            Debug.Log("Puedo escalar");
            physics.useGravity = false;
            Vector2 axis = inputActionMap.FindAction("HorizontalMovement").ReadValue<Vector2>();
            Vector3 step = new Vector3(0.0f, axis.y * Time.deltaTime * climbingSpeed, 0.0f);
            this.transform.Translate(step);
        }
        else
        {
            physics.useGravity = true;
        }

    }

}
