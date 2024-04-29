using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSurface : MonoBehaviour
{
    [SerializeField] private float multiplier;
    private float originalSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Movement movement = other.gameObject.GetComponent<Movement>();
            originalSpeed = movement.speed;
            movement.speed *= multiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Movement movement = other.gameObject.GetComponent<Movement>();
            movement.speed = originalSpeed;
        }
    }

    //Qué hacer cuando desaparece?
}
