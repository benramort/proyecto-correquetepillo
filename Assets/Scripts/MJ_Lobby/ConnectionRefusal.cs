using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionRefusal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<MyNetworkManager>().OnFailedToJoin.AddListener(() =>
        gameObject.SetActive(true));

        gameObject.SetActive(false);
    }


}
