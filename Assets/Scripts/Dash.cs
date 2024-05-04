using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    public Transform position;
    private Rigidbody physics;
    Coroutine coroutine;
    public float dashImpulse;
    public float cooldownTime;
    // Start is called before the first frame update
    void Start()
    {
        physics = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DashCoroutine()
    {
        physics.AddForce(position.forward * dashImpulse, ForceMode.Impulse);
        physics.drag = 3;
        yield return new WaitForSeconds(1);
        physics.drag = 0;
        yield return new WaitForSeconds(cooldownTime);
        coroutine = null;
    }

    public void UseAbility(InputAction.CallbackContext context)
    {
        if(coroutine == null) 
        {
            coroutine = StartCoroutine(DashCoroutine());
        }
    }
}
