using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerInput> players = new List<PlayerInput>();
    private List<LayerMask> playerLayers;

    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log(input.gameObject.name);
        players.Add(input);
        GameObject playerParent = input.transform.parent.gameObject;


        //Para que la cámara apunte al jugador correspondiente
        int layerToAdd = 5 + players.Count;
        playerParent.GetComponentInChildren<CinemachineFreeLook>().gameObject.layer = layerToAdd;
        playerParent.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;

        //Para controlar la cámara del jugador correspondiente
        playerParent.GetComponentInChildren<InputHandler>().horizontal = input.actions.FindAction("Camera");

        //iniciarPartida();

    }

    //public void iniciarPartida()
    //{
    //    int random = Random.Range(0, players.Count-1);
    //    players[random].gameObject.GetComponent<PointManager>().isTarget = true;
    //}
}
