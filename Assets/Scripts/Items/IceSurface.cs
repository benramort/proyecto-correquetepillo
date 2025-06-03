using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSurface : MonoBehaviour
{
    [SerializeField] private float timeToDissapear;
    //private float originalSpeed;
    [SerializeField] private List<Movement> playersInside = new List<Movement>();
    private AudioSource sound;
    private void Start()
    {
        sound = this.GetComponent<AudioSource>();
        sound.Play();
        StartCoroutine(DissapearCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Movement movement = other.gameObject.GetComponent<Movement>();
            playersInside.Add(movement);
            movement.onIce = 1;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Movement movement = other.gameObject.GetComponent<Movement>();
            movement.onIce--;
            playersInside.Remove(movement);
        }
    }

    private IEnumerator DissapearCoroutine()
    {
        yield return new WaitForSeconds(timeToDissapear);
        foreach (Movement movement in playersInside)
        {
            movement.onIce = 0;
        }
        playersInside.Clear();

        //playersInside.Clear();
        Destroy(gameObject);
    }

    //Qu� hacer cuando desaparece?
}
