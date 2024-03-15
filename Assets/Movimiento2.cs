using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movimiento2 : MonoBehaviour
{

    InputActionMap iaa;

    // Start is called before the first frame update
    void Start()
    {
        iaa = GetComponent<PlayerInput>().currentActionMap;
        iaa.FindAction("Jump").performed += Hola;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnJump() //Para send messages
    {
        Debug.Log("Hola");
    }

    public void Hola(InputAction.CallbackContext context)
    {
        Debug.Log("Hola");
    }
}
