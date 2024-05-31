using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float spawnTime;
    private GameObject spawnedItemModel;
    
    public GameObject[] spawnableItems;
    public GameObject spawnedItem;
    public Transform spawnPoint;

    public Texture teleporterTexture;
    public Texture iceGrenadeTexture;
    public Texture slowGrenadeTexture;
    public Texture impulseGrenadeTexture;
    private void Start()
    {
        SelectRandomItem();
    }

    private void SelectRandomItem()
    {
        int randomNumber = Random.Range(0, spawnableItems.Length);
        Debug.Log(randomNumber);
        spawnedItem = spawnableItems[randomNumber];
        //spawnedItemModel = spawnableModels[randomNumber];
        spawnedItemModel = Instantiate(spawnableItems[randomNumber], spawnPoint.position, Quaternion.identity);
        Rigidbody spawnedRigidbody = spawnedItemModel.GetComponent<Rigidbody>();
        if (spawnedRigidbody != null)
        {
            spawnedRigidbody.isKinematic = true;
            spawnedItemModel.GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && spawnedItemModel != null)
        {
            if (other.transform.Find("Launchpoint").GetComponent<Launch>().pocket == null)
            {
                other.transform.Find("Launchpoint").GetComponent<Launch>().pocket = spawnedItem;
            }
            if (spawnedItem.tag == "Teleportator")
            {
                GameObject.Find("Object").GetComponent<RawImage>().texture = teleporterTexture;

            }
            else if(spawnedItem.tag == "IceGrenade")
            {
                GameObject.Find("Object").GetComponent<RawImage>().texture = iceGrenadeTexture;


            }
            else if (spawnedItem.tag == "SlowGrenade")
            {
                GameObject.Find("Object").GetComponent<RawImage>().texture = slowGrenadeTexture;


            }
            else if (spawnedItem.tag == "ImpulseGrenade")
            {
                GameObject.Find("Object").GetComponent<RawImage>().texture = impulseGrenadeTexture;


            }
            Destroy(spawnedItemModel);
            spawnedItemModel = null;
            //SelectRandomItem();
            StartCoroutine(respawCoroutine());
        }
    }

    IEnumerator respawCoroutine()
    {
        yield return new WaitForSeconds(spawnTime);
        SelectRandomItem();
    }
}
