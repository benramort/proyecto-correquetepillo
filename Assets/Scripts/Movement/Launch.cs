using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Launch : MonoBehaviour
{
    public GameObject pocket;

    [Space(20)]

    public Vector3 launchDirection;
    public Transform launchpoint;
    public GameObject camera;
    public GameObject grenade;
    public GameObject punto;

    public float launchForce;
    public float correctionMultiplier;
    public float verticalOffset;
    private float defaulHeight;

    [Space(20)]

    public float projectedVelocity;
    public List<Vector3> puntosVector;
    public List<GameObject> puntosObjeto;

    [Space(20)]

    public LineRenderer lineRenderer;
    public float skew;
    private GameObject puntazo;

    // Start is called before the first frame update
    void Start()
    {
        launchDirection = new Vector3(transform.position.x - camera.transform.position.x, transform.position.y - camera.transform.position.y, transform.position.z - camera.transform.position.z);
        defaulHeight = launchDirection.y; //Esto no funciona cuando saltas
        //Debug.Log(Physics.gravity);
    }

    // Update is called once per frame
    void Update()
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

        int segments = 0;
        for (float t = 0; t < 3; t+=0.025f) 
        {
            float x = launchpoint.position.x + launchForce * launchDirection.x * t;
            float y = launchpoint.position.y + launchForce * launchDirection.y * t + 0.5f* Physics.gravity.y* t*t;
            float z = launchpoint.position.z + launchForce * launchDirection.z * t;

            RaycastHit hit;
            Physics.Raycast(new Vector3(x, y, z), Vector3.down, out hit, 1000f);
            Debug.DrawRay(new Vector3(x, y, z), Vector3.down, Color.green, Time.deltaTime);
            //Debug.Log(hit.distance);
            if (hit.distance < 0.01)
            {
                //Debug.Log("Suelo abajo");
                break;
            }
            segments++;
            puntosVector.Add(new Vector3(x, y, z));
        }

        lineRenderer.positionCount = segments;
        float skewUnit = skew /segments;
        Debug.Log(skewUnit);
        //Vector3 skewVector = transform.forward ;

        for (int i = 0; i < segments; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(puntosVector[i].x+skewUnit * (segments-i), puntosVector[i].y, puntosVector[i].z));
        }

        Destroy(puntazo);
        puntazo = Instantiate(punto, puntosVector[segments-1], Quaternion.identity);

        //puntosObjeto.ForEach(punto => Destroy(punto));
        //puntosObjeto.Clear();

        //foreach (Vector3 puntoVector in puntosVector)
        //{
        //    GameObject puntoInstancia = Instantiate(punto, puntoVector, Quaternion.identity);
        //    puntosObjeto.Add(puntoInstancia);
        //}

        //Debug.Log("Launch: "+ launchDirection);
    }

    public void LaunchGrenade(InputAction.CallbackContext context)
    {
        if (context.performed && pocket != null)
        {
            Debug.Log(launchDirection);
            GameObject go = Instantiate(pocket);
            //pocket = null;
            go.transform.position = launchpoint.position;
            go.GetComponent<Item>().Use(launchDirection, launchForce, transform.parent.gameObject);
        }
    }
}
