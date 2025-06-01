using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SurfaceGrenade : Item
{
    public NetworkObject surface;
    public override void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return;
        if (collision.gameObject.tag != "Player")
        {
            ContactPoint cp = collision.GetContact(0);
            if (cp.normal.y > 0.9)
            {
                NetworkManager.SpawnManager.InstantiateAndSpawn(surface, position:cp.point, rotation:Quaternion.identity);
                GetComponent<NetworkObject>().Despawn(true);
            }
        }

    }
}
