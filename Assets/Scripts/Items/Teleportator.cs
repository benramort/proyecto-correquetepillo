using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportator : Item
{
    public float verticalOffset;

    public override void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(thrower+";"+collision.gameObject);
        if (collision.gameObject != thrower.gameObject)
        {
            thrower.transform.position = new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y + verticalOffset, collision.GetContact(0).point.z);
            Destroy(gameObject);
        }   
    }


}
