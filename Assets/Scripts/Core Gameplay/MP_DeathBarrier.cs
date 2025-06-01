using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_DeathBarrier : MonoBehaviour
{
    [SerializeField] private GameObject Spawn;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.position = Spawn.transform.position;
        }
    }
}
