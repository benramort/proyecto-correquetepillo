using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface Ability
{
    public void UseAbility(InputAction.CallbackContext context);

}
