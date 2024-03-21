using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camera : MonoBehaviour
{
    public Transform orientation; 
    public Transform character; //player
    public Transform characterModel; //playerObj
    public float rotationSpeed;
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
        Vector3 viewDir = character.position - new Vector3(this.transform.position.x, character.position.y, this.transform.position.z);
        orientation.forward = viewDir.normalized;

        Vector2 axis = inputActionMap.FindAction("Camera").ReadValue<Vector2>();
        Vector3 inputDir = orientation.forward * axis.x + orientation.forward * axis.y;
        
        if (inputDir != Vector3.zero)
        {
            characterModel.forward = Vector3.Slerp(characterModel.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
