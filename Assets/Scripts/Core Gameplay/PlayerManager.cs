using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Canvas canvas;
    public GameManager gameManager;
    public int numeroDeJugadores = 2; //temporal

    public List<PlayerInput> players { get; } = new List<PlayerInput>();
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

        //int layerToAdd2 = 9 + players.Count;
        //playerParent.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd2;

        playerParent.transform.Find("CharacterModel").Find("Launchpoint").gameObject.layer = layerToAdd;

        initializeCamera();
        gameManager.UpdatePlayers();

        if (players.Count == numeroDeJugadores)
        {
            gameManager.StartGame();
        }

    }

    private void initializeCamera()
    {
        Debug.Log("Initializing camera. Player conunt" + players.Count);
        for (int i = 0; i < players.Count; i++)
        {
            for (int j = 0; j < players.Count; j++)
            {
                if (i == j) continue;
                GameObject player = players[i].gameObject;
                Canvas label = Instantiate(canvas, player.transform.Find("LabelHolder"));
                label.gameObject.layer = 6 + j;
                Debug.Log(players[j].transform.parent.Find("Camera").gameObject);
                label.GetComponent<LookAtCanvas>().player = players[j].transform.parent.Find("Camera").gameObject;
            }
        }
    }
}
