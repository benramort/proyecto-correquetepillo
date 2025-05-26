using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class MP_Spawner : NetworkBehaviour
{
    [SerializeField] private float spawnTime;
    private GameObject spawnedItemModel;
    
    public GameObject[] spawnableItems;
    public GameObject spawnedItem;
    public Transform spawnPoint;

    public Texture[] objectRenders;
    private Texture spawnedTexture;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
            SelectRandomItem();
    }

    private void SelectRandomItem()
    {
        if (IsServer)
        {
            int randomNumber = Random.Range(0, spawnableItems.Length);
            Debug.Log(randomNumber);
            SpawnItemRpc(randomNumber);
        }

    }

    private void SpawnItem(int randomNumber)
    {
        spawnedItem = spawnableItems[randomNumber];
        //spawnedItemModel = spawnableModels[randomNumber];
        spawnedItemModel = Instantiate(spawnableItems[randomNumber], spawnPoint.position, Quaternion.identity);
        Rigidbody spawnedRigidbody = spawnedItemModel.GetComponent<Rigidbody>();
        if (spawnedRigidbody != null)
        {
            spawnedRigidbody.isKinematic = true;
            spawnedItemModel.GetComponent<Collider>().enabled = false;
        }
        spawnedTexture = objectRenders[randomNumber];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && spawnedItemModel != null)
        {
            if (other.transform.Find("Launchpoint").GetComponent<Launch>().pocket == null)
            {
                other.transform.Find("Launchpoint").GetComponent<Launch>().pocket = spawnedItem;
                other.transform.root.GetComponentInChildren<Canvas>().transform.Find("Panel/Object").GetComponent<RawImage>().texture = spawnedTexture;

            }


            destroyBallRpc();

            //SelectRandoif(IsServer)
            StartCoroutine(respawCoroutine());
        }
    }

    IEnumerator respawCoroutine()
    {
        yield return new WaitForSeconds(spawnTime);
        SelectRandomItemrRpc();
    }


    [Rpc(SendTo.ClientsAndHost)]
    public void SpawnItemRpc(int randNum)
    {
        Debug.Log("Numero item recivido por cliente" +OwnerClientId +" = "+randNum);
        SpawnItem(randNum);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void destroyBallRpc()
    {
        Destroy(spawnedItemModel);
        spawnedItemModel = null;
    }

    [Rpc(SendTo.Server)]
    public void SelectRandomItemrRpc()
    {
        SelectRandomItem();

    }
}
