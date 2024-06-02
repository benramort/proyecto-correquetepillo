using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    public Transform position;
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
        physics = GetComponent<Rigidbody>();
        rawimage = transform.parent.parent.Find("Interface(Clone)/Panel/Ability").GetComponent<RawImage>();
        rawimage.texture = texture;
        rawimage.color = readyColor;
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
