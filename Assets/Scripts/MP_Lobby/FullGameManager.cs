using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FullGameManager : NetworkBehaviour
{
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private MP_GameManager MPgameManager;
    [Header("Character selection")]
    [SerializeField] private List<GameObject> characters;

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


    public NetworkList<PlayerData> playerDataList;

    public static FullGameManager INSTANCE { get; private set; }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        Debug.Log("OnNetworkSpawn");
        NetworkManager.Singleton.SceneManager.OnLoadComplete += GameSceneLoaded;
    }

    private void Awake()
    {
        INSTANCE = this;

        DontDestroyOnLoad(gameObject);

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
        Debug.Log("Se ha llamado al GoToGAmeRCP. Con "+MPgameManager.getPlayerAmmountToPlay()+" jugadores");
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

        if (playerDataList.Count < MPgameManager.getPlayerAmmountToPlay()) return;

        

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
            NetworkManager.Singleton.SceneManager.LoadScene(
                    "TestScene",
                    LoadSceneMode.Single);
        }

        

    }

    private void GameSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        if (clientId == NetworkManager.ServerClientId)
        {
            if (SceneManager.GetActiveScene().name == "OnlineMultiplayer")
            {
                SpawnCharacters();
            }


        }

    }

    private void SpawnCharacters()
    {
        foreach (PlayerData playerData in playerDataList)
        {

            NetworkObject characterObject = characters[playerData.playerType].GetComponent<NetworkObject>();
            NetworkObject character = NetworkManager.SpawnManager.InstantiateAndSpawn(
                characterObject,
                playerData.clientId,
                false,
                true,
                false);
        }
    }
}
