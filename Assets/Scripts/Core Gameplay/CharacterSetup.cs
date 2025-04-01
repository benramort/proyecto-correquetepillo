using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class CharacterSetup :  NetworkBehaviour
{

    [SerializeField] private PlayerInput input;
    [SerializeField] private InputActionAsset inputActionAsset;
    [SerializeField] private List<GameObject> objectsToDestroy;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
        {
            objectsToDestroy.ForEach(obj => Destroy(obj));
            return;
        }
        GameObject player = gameObject;
        input.actions = inputActionAsset;
        input.actions.FindAction("Jump").performed += player.GetComponentInChildren<Movement>().Jump;
        input.actions.FindAction("Catch").performed += player.GetComponentInChildren<PointManager>().Catch;
        input.actions.FindAction("Launch").started += player.GetComponentInChildren<Launch>().LauchGrenadeStart;
        input.actions.FindAction("Launch").canceled += player.GetComponentInChildren<Launch>().LaunchGrenadeEnd;
        input.actions.FindAction("Hability").performed += player.GetComponentInChildren<Ability>().UseAbility;
        player.GetComponentInChildren<InputHandler>().horizontal = input.actions.FindAction("Camera");
    }
}
