using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class MP_GameManager : NetworkBehaviour
{

    [SerializeField] TextMeshProUGUI connectionText;
    [SerializeField] TextMeshProUGUI playerInGameText;
    [SerializeField] GameObject waitingForPlayersPanel;
    [SerializeField] GameObject PlayerSelectPanel;

    private NetworkVariable<int> playersInGame = new NetworkVariable<int>(0);
  

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += onClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += onClientDisconnected;

        playersInGame.OnValueChanged += (precious, next) =>
        {
            playerInGameText.text = playersInGame.Value.ToString()+ "/4";
            if(playersInGame.Value == 4)
            {
                waitingForPlayersPanel.SetActive(false);
                PlayerSelectPanel.SetActive(true);
            }
            else
            {
                waitingForPlayersPanel.SetActive(true);
                PlayerSelectPanel.SetActive(false);
            }
        };

       

        base.OnNetworkSpawn();
    }


    private void onClientConnected(ulong clientID)
    {
        if(IsServer)
        {
            playersInGame.Value = NetworkManager.Singleton.ConnectedClients.Count;
            updateClientIdRpc(clientID, RpcTarget.Single(clientID, RpcTargetUse.Temp));
        }


    }

    //a cada cliente qeu se conecta se le manda el texto solo con su id
    [Rpc(SendTo.SpecifiedInParams)]
    private void updateClientIdRpc(ulong clientID, RpcParams baseRpcTarget)
    {
        connectionText.text += " " + clientID.ToString();
    }

    private void onClientDisconnected(ulong clientID)
    {
        if(IsServer)
            playersInGame.Value = NetworkManager.Singleton.ConnectedClients.Count;
        

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
