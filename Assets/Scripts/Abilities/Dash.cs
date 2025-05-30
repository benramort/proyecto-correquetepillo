using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Dash : MonoBehaviour, Ability
{
    private Movement movement;
    private Rigidbody physics;
    Coroutine coroutine;
    public float dashImpulse;
    public float cooldownTime;
    [Space(20)]
    [Header("Icono en interfaz")]
    public RawImage rawimage;
    public Texture texture;
    public Color readyColor;
    public Color usedColor;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        physics = GetComponent<Rigidbody>();
        rawimage = transform.root.Find("Interface/Panel/Ability").GetComponent<RawImage>();
        rawimage.texture = texture;
        rawimage.color = readyColor;
    }

    IEnumerator DashCoroutine()
    {
        movement.lerping = false;
        physics.AddForce(this.transform.forward * dashImpulse, ForceMode.Impulse);
        physics.drag = 3;
        yield return new WaitForSeconds(1);
        physics.drag = 0;
        rawimage.color = usedColor;
        yield return new WaitForSeconds(cooldownTime);
        rawimage.color = readyColor;
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
