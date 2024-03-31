using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Launch : MonoBehaviour
{
    public Vector3 launchDirection;
    public Transform launchpoint;
    public GameObject camera;
    public GameObject grenade;
    public GameObject punto;

    public float launchForce;
    public float correctionMultiplier;
    public float verticalOffset;
    private float defaulHeight;

    public float projectedVelocity;
    public List<Vector3> puntosVector;
    public List<GameObject> puntosObjeto;

    // Start is called before the first frame update
    void Start()
    {
        launchDirection = new Vector3(transform.position.x - camera.transform.position.x, transform.position.y - camera.transform.position.y, transform.position.z - camera.transform.position.z);
        defaulHeight = launchDirection.y; //Esto no funciona cuando saltas
        Debug.Log(Physics.gravity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        launchDirection = new Vector3(transform.position.x - camera.transform.position.x, transform.position.y - camera.transform.position.y, transform.position.z - camera.transform.position.z);
        float correction = correctionMultiplier * (launchDirection.y - defaulHeight);
        launchDirection.y += correction;
        launchDirection.y += verticalOffset;
        launchDirection.Normalize();

        Debug.DrawRay(launchpoint.position, launchDirection, Color.red, Time.deltaTime);

        //x = x0 + v * t
        //z = z0 + v * t
        //y = y0 + v0 * t + 1/2*a*t^2

        puntosVector.Clear();

        for (float t = 0; t < 1; t+=0.05f) 
        {
            float x = launchpoint.position.x + launchForce * launchDirection.x * t;
            float y = launchpoint.position.y + launchForce * launchDirection.y * t + 0.5f* Physics.gravity.y* t*t;
            float z = launchpoint.position.z + launchForce * launchDirection.z * t;
            puntosVector.Add(new Vector3 (x, y, z));
        }

        puntosObjeto.ForEach(punto => Destroy(punto));
        puntosObjeto.Clear();

        foreach (Vector3 puntoVector in puntosVector)
        {
            GameObject puntoInstancia = Instantiate(punto, puntoVector, Quaternion.identity);
            puntosObjeto.Add(puntoInstancia);
        }
    }

    public void LaunchGrenade(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject go = Instantiate(grenade);
            go.transform.position = launchpoint.position;
            go.GetComponent<Rigidbody>().AddForce(launchDirection * launchForce, ForceMode.Impulse);
        }
    }
}
