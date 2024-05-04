using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private bool grounded;
    private Vector3 respawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RespawnIfFallen();
    }

    public void RespawnIfFallen() 
    {
        grounded = this.GetComponent<Movement>().Grounded;
        if(!grounded)
        {
            respawnPosition = this.transform.position; 
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "DeathBox")
        {
            this.transform.position = respawnPosition;
        }
    }
}
