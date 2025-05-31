using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSetup :  NetworkBehaviour
{

    [SerializeField] private PlayerInput input;
    [SerializeField] private InputActionAsset inputActionAsset;
    [SerializeField] private List<GameObject> objectsToDestroy;

    [SerializeField] private Canvas canvas;

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
        input.actions.FindAction("Catch").performed += player.GetComponentInChildren<OnlineMultiplayer.PointManager>().Catch;
        input.actions.FindAction("Launch").started += player.GetComponentInChildren<Launch>().LauchGrenadeStart;
        input.actions.FindAction("Launch").canceled += player.GetComponentInChildren<Launch>().LaunchGrenadeEnd;
        input.actions.FindAction("Hability").performed += player.GetComponentInChildren<Ability>().UseAbility;
        player.GetComponentInChildren<InputHandler>().horizontal = input.actions.FindAction("Camera");

        //InitializeCanvas();


    }

    //public void InitializeCanvas()
    //{
    //    for (int j = 0; j < NetworkManager.ConnectedClients.Count; j++)
    //    {
    //        if ((int)NetworkManager.LocalClientId == j) continue;
    //        GameObject oponent = NetworkManager.ConnectedClients[(ulong)j].PlayerObject.gameObject;
    //        Canvas label = Instantiate(canvas, gameObject.GetComponentInChildren<Movement>().transform.GetChild(0));
    //        label.gameObject.layer = 6 + j;
    //        Debug.Log(oponent.transform.Find("Character/Camera").gameObject);
    //        label.GetComponent<LookAtCanvas>().player = oponent.transform.Find("Character").gameObject;
    //    }

    //}
}
