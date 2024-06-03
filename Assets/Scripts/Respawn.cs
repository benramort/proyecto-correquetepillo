using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private GameObject respawns;
    public int pointsAdded;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        Debug.Log("Respawns: " + respawns);
    }

    // Update is called once per frame
    void Update()
    {
          
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Map")
        {
            respawns = GameObject.Find("Respawners");

        }
    }


    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Debo respawnear");
        if(other.gameObject.tag == "DeathZone")
        {
            this.GetComponent<Movement>().lerping = false;   
            int randomRespawn = Random.Range(0, 3);
            this.transform.position = respawns.transform.GetChild(randomRespawn).position;
            this.GetComponent<PointManager>().points += pointsAdded;
        }
    }
}
