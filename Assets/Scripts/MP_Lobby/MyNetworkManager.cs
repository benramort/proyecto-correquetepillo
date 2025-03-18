using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public class MyNetworkManager : MonoBehaviour
{
    //public void StartServer()
    //{
    //     NetworkManager.Singleton.StartServer();
    //}

    private const int MAX_PLAYERAMOUNT = 2;
    public UnityEvent OnFailedToJoin = new UnityEvent(); 

   public void StartHostt()
   {
        //connection approval
        NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionApprovalCallback;

        //Start host
        NetworkManager.Singleton.StartHost();

        //FullGameManager.INSTANCE.gameState = FullGameManager.GAME_STATE.PlayerSelection;
        //NetworkManager.Singleton.SceneManager.LoadScene(
        //    FullGameManager.INSTANCE.gameState.ToString(),LoadSceneMode.Single);



    }

    private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (SceneManager.GetActiveScene().name == FullGameManager.GAME_STATE.MainScene.ToString()
           || NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYERAMOUNT)
            response.Approved = false;
        else
            response.Approved = true;
    }

    public void StartClient()
   {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectedCallback;
        NetworkManager.Singleton.StartClient();


    }

    private void OnClientDisconnectedCallback(ulong obj)
    {
        OnFailedToJoin.Invoke();
    }

    public void GoBack()
    {
        FullGameManager.INSTANCE.gameState = FullGameManager.GAME_STATE.InitialScene;
        SceneManager.LoadScene(FullGameManager.INSTANCE.gameState.ToString());
    }
}
