using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using static FullGameManager;

public class FullGameManager : NetworkBehaviour
{


    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private MP_GameManager MPgameManager;
    [Header("Character selection")]
    [SerializeField] private List<GameObject> characters;
    [SerializeField] private List<GameObject> podiumCharacters;

    public struct PlayerData : INetworkSerializable, IEquatable<PlayerData>
    {
        public ulong clientId;
        public int playerType;
        public bool isReady;
        public Vector3 playerPosition;
        public int playerPoints;

        public PlayerData(ulong id,int type, bool ready, Vector3 position, int points)
        {
            clientId = id;
            playerType = type;
            isReady = ready;
            playerPosition = position;
            playerPoints = points;
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

        if (INSTANCE == null)
        {
            INSTANCE = this;

            DontDestroyOnLoad(gameObject);

            playerDataList = new NetworkList<PlayerData>();
        } else
        {
            Destroy(gameObject);
        }

        //if(NetworkManager.Singleton != null && NetworkManager.Singleton.SceneManager != null)
        //{
        //    NetworkManager.Singleton.SceneManager.OnLoadComplete += PodiumSceneLoaded;
        //}

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
                playerDataList[i] = new PlayerData(clientId, playerType, false, new Vector3(), 100);
                return;
            }
            
        }
        playerDataList.Add(new PlayerData(clientId, playerType, false, new Vector3(), 100));
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
                    true, new Vector3(), 100);

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
                    "OnlineMultiplayer",
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

            if (SceneManager.GetActiveScene().name == "Podium")
            {
                PlaceWinners();
            }
        }

    }

    //private void PodiumSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    //{
    //    if(clientId == NetworkManager.ServerClientId)
    //    {
    //        if(SceneManager.GetActiveScene().name == "Podium")
    //        {
    //            PlaceWinners();
    //        }
    //    }

    //}

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
                false,
                new Vector3(0, 1, 0));
        }
    }


    private void PlaceWinners()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        List<PlayerData> playerDatas = new List<PlayerData>();
        for(int i = 0; i < players.Length; i++)
        {
            Destroy(players[i].gameObject);
        }

        foreach(PlayerData playerData in playerDataList)
        {
            playerDatas.Add(playerData);
        }
        playerDatas.Sort ((p1, p2) => p1.playerPoints - p2.playerPoints);
        playerDatas.Reverse();
        GameObject spawners = GameObject.Find("Positions");

        for(int i = 0; i < playerDatas.Count; i++)
        {
            NetworkObject characterObject = podiumCharacters[playerDatas[i].playerType].GetComponent<NetworkObject>();
            NetworkObject character = NetworkManager.SpawnManager.InstantiateAndSpawn(
            characterObject,
                playerDatas[i].clientId,
                false,
                true,
                false,
                spawners.transform.GetChild(i).position,
                spawners.transform.GetChild(i).rotation);
        }

    }

    [Rpc(SendTo.Server)]
    public void EndGameRpc(RpcParams rpcParams = new RpcParams())
    {
        //for (ulong i = 0; (int) i < NetworkManager.Singleton.ConnectedClients.Count; i++)
        //{
        //    players.Add(NetworkManager.Singleton.ConnectedClients[i].PlayerObject.gameObject);
        //}
        NetworkManager.Singleton.SceneManager.LoadScene(
            "Podium",
            LoadSceneMode.Single);
        Debug.Log("Client " + rpcParams.Receive.SenderClientId + " has won the game");
    }

    //private void OnEnable()
    //{
    //    if (NetworkManager.Singleton != null && NetworkManager.Singleton.SceneManager != null)
    //    {
    //        NetworkManager.Singleton.SceneManager.OnLoadComplete += PodiumSceneLoaded;
    //    }
    //}

    //private void OnDisable()
    //{
    //    if (NetworkManager.Singleton != null && NetworkManager.Singleton.SceneManager != null)
    //    {
    //        NetworkManager.Singleton.SceneManager.OnLoadComplete -= PodiumSceneLoaded;
    //    }
    //}

    //private void PodiumSceneLoaded(ulong clientId, string scene, LoadSceneMode mode)
    //{
    //    if (scene == "Podium" && clientId == NetworkManager.ServerClientId)
    //    {
    //        Debug.Log("Podium scene loaded");

    //        // Ordenamos los datos de jugadores según los puntos de mayor a menor
    //        var sortedPlayers = new List<PlayerData>((IEnumerable<PlayerData>)playerDataList);
    //        sortedPlayers.Sort((a, b) => b.playerPoints.CompareTo(a.playerPoints));

    //        // Buscamos los spawners
    //        GameObject spawners = GameObject.Find("Positions");

    //        for (int i = 0; i < sortedPlayers.Count && i < spawners.transform.childCount; i++)
    //        {
    //            var playerData = sortedPlayers[i];
    //            var networkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(playerData.clientId);
    //            if (networkObject != null)
    //            {
    //                var playerGO = networkObject.gameObject;

    //                // Movemos al jugador a su posición del podio
    //                playerGO.transform.position = spawners.transform.GetChild(i).position;
    //                playerGO.transform.rotation = spawners.transform.GetChild(i).rotation;

    //                // Bloquear movimiento y ajustar animación si aplica
    //                var movement = playerGO.GetComponentInChildren<Movement>();
    //                if (movement != null)
    //                {
    //                    movement.Freeze(FreezeType.CATCHFREEZE);
    //                }

    //                var animator = playerGO.GetComponentInChildren<Animator>();
    //                if (animator != null)
    //                {
    //                    animator.SetTrigger("grounded");
    //                    animator.ResetTrigger("running");
    //                    animator.ResetTrigger("aiming");
    //                    animator.ResetTrigger("throw");
    //                    animator.ResetTrigger("walkingBackwards");
    //                    animator.ResetTrigger("attack");
    //                }
    //            }
    //        }
    //    }
    //}

}
