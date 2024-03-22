using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
   
    InputActionMap inputActionMap;
    public GameObject camera;
    public float speed;
    public Transform characterDirection;
    //public GameObject camera;
    void Start()
    {
        inputActionMap = GetComponent<PlayerInput>().currentActionMap;
    }

    // Update is called once per frame
    void Update()
    {
        manageHorizontalMovement();
        manageCamera();
        Debug.Log(transform.forward);
    }

    private void manageHorizontalMovement()
    {
        Vector2 axis = inputActionMap.FindAction("HorizontalMovement").ReadValue<Vector2>();
        Vector3 step = new Vector3(axis.x * Time.deltaTime * speed, 0.0f, axis.y * Time.deltaTime * speed);
        transform.Translate(step);
        
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
}
