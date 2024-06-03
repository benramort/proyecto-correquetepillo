using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportator : Item
{
    public float verticalOffset;
    private AudioSource sound;
    public override void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(thrower+";"+collision.gameObject);
        if (collision.gameObject != thrower.gameObject)
        {
            sound = this.GetComponent<AudioSource>();
            sound.Play();
            thrower.transform.position = new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y + verticalOffset, collision.GetContact(0).point.z);
            //thrower.GetComponent<Movement>().Grounded = true;
            thrower.GetComponent<Movement>().lerping = false;
            StartCoroutine(DissapearCoroutine());
        }   
    }

    private IEnumerator DissapearCoroutine()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }


}
