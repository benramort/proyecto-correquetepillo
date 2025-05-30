using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ImpulseGrenade : Item
{
    private AudioSource sound;
    public ParticleSystem explosion;
    public float explosionForce = 1000f; // Fuerza de la explosion
    public float explosionRadius = 10f; // Radio de la explosion
    public float upwardModifier = 1f; // Modificador de la fuerza hacia arriba
    public List<string> affectedTags; // Lista de etiquetas afectadas por la explosion

    public override void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return;
        Debug.Log(thrower.gameObject + " " +collision.gameObject);
        sound = this.GetComponent<AudioSource>();
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("He chocado");
            ExplodeRpc();
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void ExplodeRpc()
    {
        StartCoroutine(Explode());
    }


    IEnumerator Explode()
    {
        sound.Play(); 
        explosion.Play();
        // Encuentra todos los objetos en el radio de la explosion
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            // Comprueba si el objeto tiene una de las etiquetas afectadas
            if (affectedTags.Contains(hit.tag))
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    // Aplica la fuerza de explosi�n
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardModifier, ForceMode.Impulse);
                }
            }
        }

        // Efectos visuales y sonoros aqu�

        // Destruye la granada despu�s de la explosion
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<SphereCollider>().enabled = false;

        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}

