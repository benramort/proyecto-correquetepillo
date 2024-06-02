using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SetUpEvents : MonoBehaviour
{
    private void Awake()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        input.uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
    }
}
