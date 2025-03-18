using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FullGameManager : NetworkBehaviour
{
    [SerializeField] private GameObject[] playerPrefabs;
    private NetworkVariable<int> playersReady = new NetworkVariable<int>(0);

    public struct PlayerData : INetworkSerializable, IEquatable<PlayerData>
    {
        public ulong clientId;
        public int playerType;
        public bool isReady;
        public PlayerData(ulong id,int type, bool ready)
        {
            clientId = id;
            playerType = type;
            isReady = ready;
            
        }



        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref playerType);
            serializer.SerializeValue(ref isReady);
        }
        public bool Equals(PlayerData other)
        {
            return clientId == other.clientId && playerType == other.playerType && other.isReady;
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(clientId, playerType, isReady);
        }
    }



    public enum GAME_STATE
    {
        InitialScene = 0,
        LobbyScene = 1,
        PlayerSelection = 2,
        MainScene = 3,
    }


    public NetworkList<PlayerData> playerDataList;
    public GAME_STATE gameState;

    public static FullGameManager INSTANCE { get; private set; }

    private void Awake()
    {
        INSTANCE = this;

        DontDestroyOnLoad(gameObject);
        gameState = GAME_STATE.LobbyScene;

        playerDataList = new NetworkList<PlayerData>();
    }

    public void SelectPlayer(int player)
    {
        ulong localclientID = NetworkManager.Singleton.LocalClientId;
        SelectPlayerRpc(localclientID, player);
    }

    [Rpc(SendTo.Server)]
    public void SelectPlayerRpc(ulong clientId, int playerType)
    {
        for(int i =0;i<playerDataList.Count; i++)
        {
            if (playerDataList[i].clientId == clientId)
            {
                playerDataList[i] = new PlayerData(clientId, playerType,false);
                return;
            }
            
        }
        playerDataList.Add(new PlayerData(clientId, playerType,false));
    }

    public void GoToGame()
    {

            ulong localclientID = NetworkManager.Singleton.LocalClientId;
            GoToGameRpc(localclientID);
   
    }

    [Rpc(SendTo.Server)]
    public void GoToGameRpc(ulong clientId)
    {
        for (int i = 0; i < playerDataList.Count; i++)
        {
            if (playerDataList[i].clientId == clientId)
            {
                PlayerData newPlayerData = new PlayerData(
                    playerDataList[i].clientId,
                    playerDataList[i].playerType,
                    true);

                playerDataList[i] = newPlayerData;
                break;
            }

        }

        if (NetworkManager.ConnectedClientsList.Count>1 && playerDataList.Count>1)
        {
            bool allready = true;

            for(int i=0; i<playerDataList.Count;i++)
            {
                if (!playerDataList[i].isReady)
                {
                    allready = false;
                    break;
                }
            }

            if(allready)
            {
                NetworkManager.Singleton.SceneManager.OnLoadComplete += GameSceneLoaded;
                NetworkManager.Singleton.SceneManager.LoadScene(
                    GAME_STATE.MainScene.ToString(),
                    LoadSceneMode.Single);
            }

        }

    }

    private void GameSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        gameState = GAME_STATE.MainScene;

        foreach(PlayerData playerData in playerDataList)
        {
            GameObject playerGo = Instantiate(playerPrefabs[playerData.playerType]);
            playerGo.GetComponent<NetworkObject>().SpawnAsPlayerObject(playerData.clientId, true);
        }
    }
}
