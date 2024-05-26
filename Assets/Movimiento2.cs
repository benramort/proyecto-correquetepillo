using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movimiento2 : MonoBehaviour
{

    InputActionMap iaa;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        iaa = GetComponent<PlayerInput>().currentActionMap;
        //iaa.FindAction("Jump").performed += Hola;
        iaa.FindAction("Movement").ReadValue<Vector2>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 axis = iaa.FindAction("Movement").ReadValue<Vector2>();
        transform.position = new Vector3(transform.position.x + axis.x * speed * Time.deltaTime, transform.position.y, transform.position.z + axis.y * speed * Time.deltaTime);
    }

    //public void OnJump() //Para send messages
    //{
    //    Debug.Log("Hola");
    //}

    public void Movement(InputAction.CallbackContext context) //Para unity events
    {
        //Vector2 axis = context.ReadValue<Vector2>();
        //transform.position = new Vector3(transform.position.x + axis.x * speed * Time.deltaTime, transform.position.y, transform.position.z + axis.y * speed * Time.deltaTime);
    }

    public void Hola(InputAction.CallbackContext context) //Para unity events
    {
        if (context.performed) {
            Debug.Log("Hola");
        }
    }
}
