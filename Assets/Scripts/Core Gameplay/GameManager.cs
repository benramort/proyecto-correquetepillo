using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public List<GameObject> players;
    public PlayerManager playerManager;

    private long chrono = 0;

    public void UpdatePlayers()
    {
        foreach (PlayerInput playerInput in playerManager.players)
        {
            GameObject player = playerInput.gameObject;
            players.Add(player);
            player.GetComponent<PointManager>().gameManager = this;
        }
    }

    public void StartGame()
    {
        players[Random.Range(0, players.Count)].GetComponent<PointManager>().isTarget = true;
    }

    public void EndGame(GameObject winner)
    {
        Debug.Log("The game is over");
    }
}
