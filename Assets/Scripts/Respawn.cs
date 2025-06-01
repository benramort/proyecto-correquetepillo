using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private GameObject respawn;
    public int pointsAdded;
    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        
    }

    // Update is called once per frame
    void Update()
    {
          
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneName)
        {
            respawn = GameObject.Find("PlayerSpawns");
            Debug.Log("Respawns: " + respawn);
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "DeathZone")
        {
            this.GetComponent<Movement>().lerping = false;   
            this.transform.position = respawn.transform.GetChild(0).position;
            this.GetComponent<PointManager>().points += pointsAdded;
        }
    }
}
