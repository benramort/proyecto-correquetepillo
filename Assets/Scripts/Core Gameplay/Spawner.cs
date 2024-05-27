using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float spawnTime;
    
    public GameObject[] spawnableItems;
    public GameObject spawnedItem;
    private GameObject spawnedItemModel;
    public Transform spawnPoint;
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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && spawnedItemModel != null)
        {
            if (other.transform.Find("Launchpoint").GetComponent<Launch>().pocket == null)
                other.transform.Find("Launchpoint").GetComponent<Launch>().pocket = spawnedItem;
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
