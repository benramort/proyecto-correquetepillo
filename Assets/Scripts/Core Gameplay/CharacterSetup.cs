using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class CharacterSetup : MonoBehaviour
{

    [SerializeField] private PlayerInput input;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = gameObject;
        input.actions.FindAction("Jump").performed += player.GetComponentInChildren<Movement>().Jump;
        input.actions.FindAction("Catch").performed += player.GetComponentInChildren<PointManager>().Catch;
        input.actions.FindAction("Launch").started += player.GetComponentInChildren<Launch>().LauchGrenadeStart;
        input.actions.FindAction("Launch").canceled += player.GetComponentInChildren<Launch>().LaunchGrenadeEnd;
        input.actions.FindAction("Hability").performed += player.GetComponentInChildren<Ability>().UseAbility;
        player.GetComponentInChildren<InputHandler>().horizontal = input.actions.FindAction("Camera");
    }
}
