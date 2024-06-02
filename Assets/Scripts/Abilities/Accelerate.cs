using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Accelerate : MonoBehaviour, Ability
{
    Movement movement;
    Coroutine coroutine;
    public int multiplier;
    public float abilityTime;
    public float cooldownTime;
    private AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        sound = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AccelerateCoroutine()
    {
        sound.loop = true;
        sound.Play();
        movement.speed = movement.speed * multiplier;
        yield return new WaitForSeconds(abilityTime);
        sound.Stop();
        movement.speed /= multiplier;
        yield return new WaitForSeconds(cooldownTime);
        coroutine = null;
    }

    public void UseAbility(InputAction.CallbackContext context)
    {
        if(coroutine == null)
        {
            coroutine = StartCoroutine(AccelerateCoroutine());
        }
    }


}
