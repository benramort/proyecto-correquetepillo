using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
   
    InputActionMap inputActionMap;
    public float speed;
    //public GameObject camera;
    void Start()
    {
        inputActionMap = GetComponent<PlayerInput>().currentActionMap;
    }

    // Update is called once per frame
    void Update()
    {
        manageHorizontalMovement();
    }

    private void manageHorizontalMovement()
    {
        Vector2 axis = inputActionMap.FindAction("HorizontalMovement").ReadValue<Vector2>();
        transform.Translate(new Vector3(axis.x* Time.deltaTime * speed, 0.0f, axis.y * Time.deltaTime * speed));
        
    }

    //private void manageCamera()
    //{
    //    Vector2 axis = inputActionMap.FindAction("Camera").ReadValue<Vector2>();
    //    Debug.Log(axis);
    //    camera.transform.RotateAround(this.transform.position, Vector3.up, axis.x * Time.deltaTime * cameraSensibility);
    //    camera.transform.RotateAround(this.transform.position, Vector3.right, axis.y * Time.deltaTime * cameraSensibility);
    //}




}
