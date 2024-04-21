using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager us;
    
    public List<GameObject> players;
    public PlayerManager playerManager; //Probablemente esto acabe siendo static

    [Space(20)]
    private double chrono = 0;
    public double gameTime = 300;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (us == null)
        {
            us = this;
        } else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        chrono += Time.deltaTime;
        if (chrono >= gameTime)
        {
            EndGame(null);
        }
    }

    public void UpdatePlayers()
    {
        foreach (PlayerInput playerInput in playerManager.players)
        {
            GameObject player = playerInput.gameObject;
            players.Add(player);
            DontDestroyOnLoad(player.transform.parent.gameObject);
            player.GetComponent<PointManager>().gameManager = this;
        }
    }

    public void StartGame()
    {
        players[Random.Range(0, players.Count)].GetComponent<PointManager>().isTarget = true;
    }

    public void EndGame(GameObject winner)
    {
        chrono = 0;
        Debug.Log("The game is over");
        //Poner los jugadores a 100 puntos
        SceneManager.LoadScene("Movement");
        Debug.Log(playerManager.players.Count);   
    }
}
