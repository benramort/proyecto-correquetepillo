using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Climb : MonoBehaviour
{
    InputActionMap inputActionMap;
    private Rigidbody physics;
    public float climbingSpeed;
    // Start is called before the first frame update
    void Start()
    {
        inputActionMap = GetComponent<PlayerInput>().actions.FindActionMap("Climb");
        physics = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveClimbing();
    }

    public void MoveClimbing() 
    {
        if (Physics.Raycast(this.transform.position, this.transform.forward, 1.0f))
        {
            physics.useGravity = false;
            Vector2 axis = inputActionMap.FindAction("MovementClimbing").ReadValue<Vector2>();
            Vector3 step = new Vector3(axis.x * Time.deltaTime * climbingSpeed, axis.y * Time.deltaTime * climbingSpeed, 0.0f);
            this.transform.Translate(step);
        }
        else
        {
            physics.useGravity = true;
            inputActionMap = GetComponent<PlayerInput>().actions.FindActionMap("Player");   
        }

    }
}
