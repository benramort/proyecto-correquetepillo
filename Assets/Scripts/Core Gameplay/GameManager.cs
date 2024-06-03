using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public int desiredFramerate;
    [Space(20)]
    public static GameManager instance;
    
    public List<GameObject> players;
    public PlayerManager1 playerManager; //Probablemente esto acabe siendo static

    //public GameObject character;
    public GameObject playerGui;
    private GameObject timeGui;

    [Space(20)]
    private float chrono = 0;
    public float gameTime;

    private float minutes;
    private float seconds;
    private string timeText;

    private bool onGame = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this.gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = desiredFramerate; //Igual sólo hay que usar uno de los dos
        QualitySettings.vSyncCount = 1; 
    }

    private void Update()
    {
        if(!onGame)
        {
            return;
        }

        //timeGui = GameObject.Find("TimeInterface/RemainingTime");
        chrono += Time.deltaTime;
        if (chrono >= gameTime)
        {
            EndGame(null);
        }
        if(timeGui != null)
        {
            ManageTime();
        }
    }

    public void ManageTime()
    {
        minutes = (int) ((gameTime - chrono) / 60);
        seconds = (int) ((gameTime - chrono) % 60);
        string secondsString = seconds.ToString();
        if(secondsString.Length == 1)
        {
            secondsString = "0" + secondsString;
        }
        timeText = minutes + ":" + secondsString;
        timeGui.GetComponent<TextMeshProUGUI>().text = timeText;
    }

    public void UpdatePlayers()
    {
        players.Clear();
        foreach (PlayerInput playerInput in playerManager.players)
        {
            //Debug.Log(playerInput.name);
            GameObject player = playerInput.gameObject;
            //Debug.Log(player.name);
            players.Add(player);
            //DontDestroyOnLoad(player.transform.parent.gameObject);
            DontDestroyOnLoad(player);
            
        }
    }

    public void ChangeToGameScene()
    {

        GetComponent<PlayerInputManager>().DisableJoining();
        AddCharacters();
        playerManager.ManageLayers();
        playerManager.initializeCamera();
        playerManager.SetUpEvents();
        AddInterface();
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            player.transform.Find("Character").gameObject.SetActive(true);
            player.transform.Find("UIControl").gameObject.SetActive(false);

        }
        SceneManager.LoadScene("Map");
        timeGui = GameObject.Find("TimeInterface/RemainingTime");
        onGame = true;
        chrono = 0;
        //Provisional

        StartGame();

        //foreach (GameObject player in players)
        //{
        //    player.GetComponent<Movement>().transform.position = new Vector3(0, 10, 0);
        //}
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Map")
        {
            timeGui = GameObject.Find("TimeInterface/RemainingTime");
            GameObject spawners = GameObject.Find("SpawnPoints");
            for (int i = 0; i < players.Count; i++)
            {
                players[i].transform.position = spawners.transform.GetChild(i).position;
                players[i].transform.rotation = spawners.transform.GetChild(i).rotation;
            }
        }
        if(scene.name == "Podium")
        {
            players.Sort((p1, p2) => p1.GetComponentInChildren<PointManager>().points - p2.GetComponentInChildren<PointManager>().points);
            GameObject spawners = GameObject.Find("Positions");
            for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponentInChildren<Rigidbody>().isKinematic = true;
                players[i].transform.position = spawners.transform.GetChild(i).position;
                players[i].GetComponentInChildren<Movement>().transform.position = spawners.transform.GetChild(i).position;
                players[i].transform.rotation = spawners.transform.GetChild(i).rotation;
                players[i].GetComponentInChildren<Movement>().transform.rotation = spawners.transform.GetChild(i).rotation;
                Animator animator = players[i].GetComponentInChildren<Animator>();
                animator.ResetTrigger("running");
                animator.SetTrigger("grounded");
                animator.ResetTrigger("aiming");
                animator.ResetTrigger("throw");
                animator.ResetTrigger("walkingBackwards");
                animator.ResetTrigger("attack");
                //StartCoroutine(RestartGame());
            }
        }
    }

    public void AddInterface()
    {
        
        foreach (GameObject player in players)
        {
            GameObject guiInstance = Instantiate(playerGui, player.transform);
            guiInstance.GetComponent<Canvas>().worldCamera = player.GetComponentInChildren<Camera>();
            guiInstance.GetComponent<Canvas>().planeDistance = 0.3f;
            //guiInstance.transform.Find("Panel").GetComponent<RectTransform>().
        }
    }

    public void AddCharacters()
    {
        for (int i = 0; i < players.Count; i++)
        {
            Debug.Log(players[i].GetComponentInChildren<UIControl>().selectedCharacter);
            Debug.Log(players[i].transform.Find("Character"));

            GameObject instance = Instantiate(players[i].GetComponentInChildren<UIControl>().selectedCharacter, players[i].transform.Find("Character"));
            //Transform target = players[i].transform.Find("Character/YellowBoxer(Clone)");
            players[i].GetComponentInChildren<CinemachineFreeLook>().Follow = instance.transform;
            players[i].GetComponentInChildren<CinemachineFreeLook>().LookAt = instance.transform;
        }
    }

    public void StartGame()
    {
        players[Random.Range(0, players.Count)].GetComponentInChildren<PointManager>().isTarget = true;
    }

    public void EndGame(GameObject winner)
    {
        chrono = 0;
        Debug.Log("The game is over");
        //Poner los jugadores a 100 puntos
        SceneManager.LoadScene("Podium");
        onGame = false;
        foreach(GameObject player in players) 
        {
            player.GetComponentInChildren<Camera>().gameObject.SetActive(false);
            player.GetComponentInChildren<CinemachineFreeLook>().gameObject.SetActive(false);
            player.GetComponentInChildren<Movement>().enabled = false;
            player.GetComponentInChildren<PointManager>().enabled = false;
            player.GetComponentInChildren<Launch>().enabled = false;
            player.GetComponentInChildren<PlayerInput>().enabled = false;

        }
        Debug.Log(playerManager.players.Count);   
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(10);
        foreach(GameObject player in players)
        {
            Destroy(player);
        }
        players.Clear();
        SceneManager.LoadScene("MainMenu");
        Destroy(this.gameObject);
    }

    public void Exit()
    {
        Debug.Log("Cerrar aplicación");
        Application.Quit();
    }
}
