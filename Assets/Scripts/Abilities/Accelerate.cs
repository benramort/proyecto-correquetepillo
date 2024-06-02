using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Accelerate : MonoBehaviour, Ability
{
    Movement movement;
    Coroutine coroutine;
    public int multiplier;
    public float abilityTime;
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
        rawimage = transform.parent.parent.Find("Interface(Clone)/Panel/Ability").GetComponent<RawImage>();
        rawimage.texture = texture;
        rawimage.color = readyColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AccelerateCoroutine()
    {
        movement.speed = movement.speed * multiplier;
        yield return new WaitForSeconds(abilityTime);
        rawimage.color = usedColor;
        movement.speed /= multiplier;
        yield return new WaitForSeconds(cooldownTime);
        rawimage.color = readyColor;
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
