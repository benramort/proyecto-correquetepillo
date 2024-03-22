using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
   
    InputActionMap inputActionMap;
    public GameObject characterModel;
    public float speed;
    //public GameObject camera;
    void Start()
    {
        inputActionMap = GetComponent<PlayerInput>().currentActionMap;
    }

    // Update is called once per frame
    void Update()
    {
        manageHorizontalMovement();
    }

    private void manageHorizontalMovement()
    {
        Vector2 axis = inputActionMap.FindAction("HorizontalMovement").ReadValue<Vector2>();
        transform.Translate(new Vector3(axis.x* Time.deltaTime * speed, 0.0f, axis.y * Time.deltaTime * speed));
        
    }

    private void manageCamera()
    {
        Vector3 viewDirection = new Vector3(characterModel.transform.position.x - this.transform.position.x, 0, characterModel.transform.position.z - this.transform.position.z);
        viewDirection = viewDirection.normalized;
        float angle = Vector3.Angle(Vector3.forward, viewDirection);

    }
}
