using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 respawnPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Grounded: " + this.GetComponent<Movement>().Grounded);
        if (this.GetComponent<Movement>().Grounded == true)
        {
            respawnPosition = this.transform.position;
        }
    }



    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "DeathZone")
        {
            this.transform.position = respawnPosition;
            this.GetComponent<PointManager>().points += 20;
        }
    }
}
