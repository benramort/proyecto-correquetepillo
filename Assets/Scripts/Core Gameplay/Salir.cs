using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salir : MonoBehaviour
{
    public void Exit()
    {
        Debug.Log("Cerrar aplicación");
        Application.Quit();
    }
}
