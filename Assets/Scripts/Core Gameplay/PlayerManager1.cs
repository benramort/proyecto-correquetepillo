using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerManager1 : MonoBehaviour
{
    public Canvas canvas;
    public GameManager gameManager;
    public int numeroDeJugadores = 2; //temporal

    //public GameObject character; //Temporal

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
        //GameObject playerParent = input.transform.Find("Character").gameObject;

        //input.actions.FindAction("NewSubmit").performed += (ctx => Debug.Log("holassss")); //Funciona de manera independiente a los unity events


        //input.actionEvents[0].AddListener(prueba);

        //foreach (PlayerInput.ActionEvent eventito in input.actionEvents)
        //{
        //    Debug.Log(eventito.actionName);
        //}

        //ManageLayers(input, playerParent);

        //initializeCamera();
        gameManager.UpdatePlayers();

        //if (players.Count == numeroDeJugadores)
        //{
        //    gameManager.StartGame();
        //}

        //playerParent.SetActive(false);

    }

    public void SetUpEvents()
    {
        foreach (PlayerInput input in players)
        {
            GameObject player = input.transform.Find("Character").gameObject;
            input.actions.FindAction("Jump").performed += player.GetComponentInChildren<Movement>().Jump;
            input.actions.FindAction("Catch").performed += player.GetComponentInChildren<PointManager>().Catch;
            input.actions.FindAction("Launch").started += player.GetComponentInChildren<Launch>().LauchGrenadeStart;
            input.actions.FindAction("Launch").canceled += player.GetComponentInChildren<Launch>().LaunchGrenadeEnd;
        }
    }

    public void ManageLayers()
    {
        int contador = 1;
        foreach (PlayerInput playerInput in players)
        {
            GameObject playerParent = playerInput.transform.Find("Character").gameObject;

            //Para que la cámara apunte al jugador correspondiente
            int layerToAdd = 5 + contador;
            contador++;
            playerParent.GetComponentInChildren<CinemachineFreeLook>().gameObject.layer = layerToAdd;
            playerParent.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;

            //Para controlar la cámara del jugador correspondiente
            playerParent.GetComponentInChildren<InputHandler>().horizontal = playerInput.actions.FindAction("Camera");

            //int layerToAdd2 = 9 + players.Count;
            //playerParent.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd2;

            Debug.Log(playerParent);
            //playerParent.transform.Find("YellowBoxer(Clone)/Launchpoint").gameObject.layer = layerToAdd;
            
            playerParent.transform.GetComponentInChildren<Launch>().gameObject.layer = layerToAdd;
        }

        //initializeCamera();
        
    }

    public void initializeCamera()
    {
        Debug.Log("Initializing camera. Player conunt" + players.Count);
        for (int i = 0; i < players.Count; i++)
        {
            for (int j = 0; j < players.Count; j++)
            {
                if (i == j) continue;
                GameObject player = players[i].gameObject;
                Canvas label = Instantiate(canvas, player.GetComponentInChildren<Movement>().transform.GetChild(0));
                label.gameObject.layer = 6 + j;
                Debug.Log(players[j].transform.Find("Character/Camera").gameObject);
                label.GetComponent<LookAtCanvas>().player = players[j].transform.Find("Character/Camera").gameObject;
            }
        }
    }
}
