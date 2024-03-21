using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform camera;
    public Transform character;
    public float rotationSpeed;

    InputActionMap inputActionMap;
    public float speed;
    //public GameObject camera;
    public float cameraSensibility;
    void Start()
    {
        inputActionMap = GetComponent<PlayerInput>().currentActionMap;
    }

    // Update is called once per frame
    void Update()
    {
        manageHorizontalMovement();
        //manageCamera();
    }

    private void manageHorizontalMovement()
    {
        Vector2 axis = inputActionMap.FindAction("HorizontalMovement").ReadValue<Vector2>();
        Vector3 playerOrientation = character.transform.position - new Vector3(transform.position.x, character.transform.position.y, transform.position.z);
        playerOrientation.Normalize();
        Debug.Log(playerOrientation);
        //Debug.Log(axis);
        transform.Translate(new Vector3(axis.x* Time.deltaTime * speed * playerOrientation.x, 0.0f, axis.y * Time.deltaTime * speed * playerOrientation.z)) ;
        
    }

    //private void manageCamera()
    //{
    //    Vector2 axis = inputActionMap.FindAction("Camera").ReadValue<Vector2>();
    //    Debug.Log(axis);
    //    camera.transform.RotateAround(this.transform.position, Vector3.up, axis.x * Time.deltaTime * cameraSensibility);
    //    camera.transform.RotateAround(this.transform.position, Vector3.right, axis.y * Time.deltaTime * cameraSensibility);
    //}




}
