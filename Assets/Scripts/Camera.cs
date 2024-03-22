using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camera : MonoBehaviour
{
    public GameObject characterModel;

    InputActionMap inputActionMap;
    // Start is called before the first frame update
    void Start()
    {
        inputActionMap = GameObject.Find("CharacterModel").GetComponent<PlayerInput>().currentActionMap;   
    }

    // Update is called once per frame
    void Update()
    {
        manageCamera();
    }
    private void manageCamera() 
    {
        Vector3 viewDirection = new Vector3(characterModel.transform.position.x - this.transform.position.x, 0, characterModel.transform.position.z - this.transform.position.z);
        viewDirection = viewDirection.normalized;
        float angle = Vector3.Angle(Vector3.forward, viewDirection);
        



    }
}
