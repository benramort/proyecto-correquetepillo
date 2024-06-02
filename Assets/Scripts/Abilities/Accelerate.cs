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

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AccelerateCoroutine()
    {
        movement.speed = movement.speed * multiplier;
        yield return new WaitForSeconds(abilityTime);
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
