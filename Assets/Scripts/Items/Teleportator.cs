using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Teleportator : Item
{
    public float verticalOffset;
    private AudioSource sound;
    public override void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return;
        //Debug.Log(thrower+";"+collision.gameObject);
        if (collision.gameObject.tag != "Player")
        {
            sound = this.GetComponent<AudioSource>();
            sound.Play();
            Debug.Log("Punto de colisión : " + collision.GetContact(0).point);
            Vector3 destinationPosition = new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y + verticalOffset, collision.GetContact(0).point.z);
            TeleportRpc(destinationPosition.x, destinationPosition.y, destinationPosition.z, RpcTarget.Single(thrower.GetComponent<NetworkObject>().OwnerClientId, RpcTargetUse.Temp));
            //StartCoroutine(DissapearCoroutine());
            Destroy(gameObject);
        }   
    }

    [Rpc(SendTo.SpecifiedInParams)]
    public void TeleportRpc(float positionX, float positionY, float positionZ, RpcParams rpcParams)
    {
        Debug.Log("Hay que teletransportase. Destino: " + positionX + ":" + positionY + ":" + positionZ);
        GameObject thrower = NetworkManager.LocalClient.PlayerObject.gameObject;
        thrower.transform.position = new Vector3(positionX, positionY, positionZ);
        thrower.GetComponentInChildren<Movement>().lerping = false;

    }

    private IEnumerator DissapearCoroutine()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }


}
