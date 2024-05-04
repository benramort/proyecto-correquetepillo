using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceGrenade : Item
{
    public GameObject surface;

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player") //Esto igual hacerlo ==map?
        { 
            ContactPoint cp = collision.GetContact(0);
            if (cp.normal.y > 0.9)
            {
                Instantiate(surface, cp.point, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }

    }
}
