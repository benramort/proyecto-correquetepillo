using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Colision");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Catch");
            other.GetComponent<PointManager>().isTarget = true;
            transform.parent.GetComponent<PointManager>().isTarget = false;
        }
    }
}
