using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Launch : MonoBehaviour
{
    public GameObject pocket;
    public Animator animator;

    [Space(20)]

    public Vector3 launchDirection;
    public Transform launchpoint;
    public GameObject cam;
    //public GameObject grenade;
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
    //private GameObject puntazo;
    private bool showLine = false;

    // Start is called before the first frame update
    void Start()
    {
        launchDirection = new Vector3(transform.position.x - cam.transform.position.x, transform.position.y - cam.transform.position.y, transform.position.z - cam.transform.position.z);
        defaulHeight = launchDirection.y; //Esto no funciona cuando saltas
        //Debug.Log(Physics.gravity);
    }

    // Update is called once per frame
    void Update()
    {
        if (!showLine)
        {
            return;
        }
        launchDirection = new Vector3(transform.position.x - cam.transform.position.x, transform.position.y - cam.transform.position.y, transform.position.z - cam.transform.position.z);
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
            bool boolHit = Physics.Raycast(new Vector3(x, y, z), Vector3.down, out hit, 1000f);
            Debug.DrawRay(new Vector3(x, y, z), Vector3.down, Color.green, Time.deltaTime);
            //Debug.Log(hit.distance);
            if (hit.distance < 0.01 && boolHit)
            {
                //Debug.Log("Suelo abajo");
                break;
            }
            segments++;
            puntosVector.Add(new Vector3(x, y, z));
        }

        lineRenderer.positionCount = segments;
        float skewUnit = skew /segments;
        Vector2 normalized = new Vector2(transform.forward.x, transform.forward.z).normalized;
        Vector3 skewVector = new Vector3(skewUnit * normalized.y, 0, skewUnit*-normalized.x); //Más o menos
        //Vector3 skewVector = transform.forward ;
        //Debug.Log("Direction: "+transform.forward);
        //Debug.Log("Skew: "+skewVector);

        for (int i = 0; i < segments; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(puntosVector[i].x+skewVector.x * (segments-i), puntosVector[i].y, puntosVector[i].z + skewVector.z * (segments-i)));
        }

        //Destroy(puntazo);
        //puntazo = Instantiate(punto, puntosVector[segments - 1], Quaternion.identity);

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
        if (context.started && pocket != null)
        {
            lineRenderer.enabled = true;
            showLine = true;
        }

        if (context.canceled && pocket != null)
        {
            lineRenderer.enabled = false;
            showLine = false;
            Debug.Log(launchDirection);
            GameObject go = Instantiate(pocket);
            pocket = null;
            go.transform.position = launchpoint.position;
            animator.SetTrigger("launch");
            go.GetComponent<Item>().Use(launchDirection, launchForce, transform.parent.gameObject);
        }
    }
}
