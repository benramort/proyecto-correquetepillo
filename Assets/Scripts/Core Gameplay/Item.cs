using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected GameObject thrower;
    public virtual void Use(Vector3 launchDirection, float launchForce, GameObject thrower)
    {
        this.thrower = thrower;
        GetComponent<Rigidbody>().AddForce(launchDirection * launchForce, ForceMode.Impulse);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Item collided with " + collision.gameObject.name);
    }
}
