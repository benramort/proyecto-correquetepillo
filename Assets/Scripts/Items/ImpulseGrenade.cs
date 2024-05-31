using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseGrenade : Item
{
    public ParticleSystem explosion;
    public float explosionForce = 1000f; // Fuerza de la explosion
    public float explosionRadius = 10f; // Radio de la explosion
    public float upwardModifier = 1f; // Modificador de la fuerza hacia arriba
    public List<string> affectedTags; // Lista de etiquetas afectadas por la explosion

    public override void OnCollisionEnter(Collision collision)
    {
        Debug.Log(thrower.gameObject + " " +collision.gameObject);
        if (collision.gameObject != thrower.gameObject)
        {
            Debug.Log("He chocado");
            explosion.Play();
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
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
                    // Aplica la fuerza de explosión
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardModifier, ForceMode.Impulse);
                }
            }
        }

        // Efectos visuales y sonoros aquí

        // Destruye la granada después de la explosion
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        //Destroy(this.gameObject);
    }
}

