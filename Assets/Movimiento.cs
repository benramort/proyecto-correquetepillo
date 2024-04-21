using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movimiento : MonoBehaviour
{
    //InputActionAsset inputAction;
    private Prueba prueba;
    private InputAction desplazar;
    private InputAction hability;

    // Start is called before the first frame update
    private void OnEnable()
    {
        prueba.Player.Movement.Enable();
        desplazar = prueba.Player.Movement;
        prueba.Player.Jump.Enable();
        prueba.Player.Jump.started += Salto;

        hability = prueba.Player.Hability;
        hability.Enable();
    }

    private void Salto(InputAction.CallbackContext context)
    {
        Debug.Log("salto");
    }

    private void Awake()
    {
        prueba = new Prueba();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(desplazar.ReadValue<Vector2>());
        Debug.Log(hability.ReadValue<float>());

    }


}
