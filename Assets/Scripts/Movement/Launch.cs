using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Unity.Netcode;

public class Launch : NetworkBehaviour
{
    public int pocket = -1;
    public Animator animator;

    [Space(20)]
    [HideInInspector] public Vector3 launchDirection;
    public Transform launchpoint;
    public GameObject cam;
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

    [Space(20)]
    public Texture noObjectTexture;
    private bool showLine = false;
    private GameObject objectImage;

    //[SerializeField] private GameObject spawnedItemPrefab;
    //private List<NetworkObject> itemList;
    //private GameObject spawnedObject;

    public override void OnNetworkSpawn()
    {
        //itemList = new List<NetworkObject>();
        base.OnNetworkSpawn();
    }

    void Start()
    {
        cam = transform.parent.parent.Find("Camera").gameObject;
        objectImage = transform.root.Find("Interface/Panel/Object").gameObject;
        launchDirection = transform.position - cam.transform.position;
        defaulHeight = launchDirection.y;
    }

    void FixedUpdate()
    {
        if (!showLine) return;

        launchDirection = transform.position - cam.transform.position;
        float correction = correctionMultiplier * (launchDirection.y - defaulHeight);
        launchDirection.y += correction + verticalOffset;
        launchDirection.Normalize();

        Debug.DrawRay(launchpoint.position, launchDirection, Color.red, Time.deltaTime);
        puntosVector.Clear();

        int segments = 0;
        for (float t = 0; t < 3; t += 0.025f)
        {
            float x = launchpoint.position.x + launchForce * launchDirection.x * t;
            float y = launchpoint.position.y + launchForce * launchDirection.y * t + 0.5f * Physics.gravity.y * t * t;
            float z = launchpoint.position.z + launchForce * launchDirection.z * t;

            RaycastHit hit;
            bool boolHit = Physics.Raycast(new Vector3(x, y, z), Vector3.down, out hit, 1000f);
            if (hit.distance < 0.01f && boolHit) break;

            segments++;
            puntosVector.Add(new Vector3(x, y, z));
        }

        lineRenderer.positionCount = segments;
        float skewUnit = skew / segments;
        Vector2 normalized = new Vector2(transform.forward.x, transform.forward.z).normalized;
        Vector3 skewVector = new Vector3(skewUnit * normalized.y, 0, skewUnit * -normalized.x);

        for (int i = 0; i < segments; i++)
        {
            lineRenderer.SetPosition(i, puntosVector[i] + skewVector * (segments - i));
        }
    }

    public void LauchGrenadeStart(InputAction.CallbackContext context)
    {
        if (pocket != -1)
        {
            animator.SetTrigger("aiming");
            lineRenderer.enabled = true;
            showLine = true;
        }
    }

    public void LaunchGrenadeEnd(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        if (pocket != -1)
        {
            animator.ResetTrigger("aiming");
            lineRenderer.enabled = false;
            showLine = false;

            // Enviamos datos al servidor
            LaunchItemRpc(pocket, launchDirection, launchForce, launchpoint.position);

            // Eliminamos el objeto de la mano
            pocket = -1;
            objectImage.GetComponent<RawImage>().texture = noObjectTexture;
            Debug.Log("Ha llegado");
        }
    }

    [Rpc(SendTo.Server)]
    public void LaunchItemRpc(int itemId, Vector3 direction, float force, Vector3 position, RpcParams rpcParams = default)
    {
        //Debug.Log("MP_Spawner.SpawnableItems: " + MP_Spawner.SpawnableItems.ToString());
        GameObject gameObjectPrefab = MP_Spawner.SpawnableItems[itemId];
        GameObject launchedItem = Instantiate(gameObjectPrefab, position, Quaternion.identity);
        var netObj = launchedItem.GetComponent<NetworkObject>();
        netObj.Spawn(true);
        Debug.Log(NetworkManager.ConnectedClients[rpcParams.Receive.SenderClientId].PlayerObject);
        GameObject player = NetworkManager.ConnectedClients[rpcParams.Receive.SenderClientId].PlayerObject.gameObject;
        launchedItem.GetComponent<Item>().Use(direction, force, player);
    }
}
