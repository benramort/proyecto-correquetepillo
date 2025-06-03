using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSurface : MonoBehaviour
{
    [SerializeField] private float timeToDissapear;
    //private float originalSpeed;
    [SerializeField] private List<Movement> playersInside = new List<Movement>();

    private void Start()
    {
        StartCoroutine(DissapearCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Movement movement = other.gameObject.GetComponent<Movement>();
            playersInside.Add(movement);
            movement.Slow();
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Movement movement = other.gameObject.GetComponent<Movement>();
            movement.UnSlow();
            playersInside.Remove(movement);
        }
    }

    private IEnumerator DissapearCoroutine()
    {
        yield return new WaitForSeconds(timeToDissapear);
        foreach (Movement movement in playersInside)
        {
            movement.UnSlow();
            movement.Grounded = true;
        }
    
        playersInside.Clear();

        Destroy(gameObject);
    }

    //Qu� hacer cuando desaparece?
}
