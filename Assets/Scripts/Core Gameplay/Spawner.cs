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

    public Texture[] objectRenders;
    private Texture spawnedTexture;

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
        spawnedTexture = objectRenders[randomNumber];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && spawnedItemModel != null)
        {
            if (other.transform.Find("Launchpoint").GetComponent<Launch>().pocket == null)
            {
                other.transform.Find("Launchpoint").GetComponent<Launch>().pocket = spawnedItem;
            }
            other.transform.parent.parent.Find("Interface(Clone)/Panel/Object").GetComponent<RawImage>().texture = spawnedTexture;
            

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
