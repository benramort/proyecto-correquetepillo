using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected GameObject thrower;
    public virtual void Use(Vector3 launchDirection, float launchForce, GameObject thrower)
    {
        //Debug.Log(launchDirection);
        this.thrower = thrower;
        GetComponent<Rigidbody>().AddForce(launchDirection * launchForce, ForceMode.Impulse);
        StartCoroutine(ProgrammedDestruction());
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Item collided with " + collision.gameObject.name);
    }

    private IEnumerator ProgrammedDestruction()
    {
        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);
    }
}
