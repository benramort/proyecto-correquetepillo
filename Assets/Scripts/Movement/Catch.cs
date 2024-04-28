using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Colision");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Catch");
            if (other.GetComponent<PointManager>().isTarget)
            {
                other.GetComponent<PointManager>().isTarget = false;
                other.transform.Find("LabelHolder").gameObject.SetActive(false);
                other.GetComponent<Movement>().Freeze(FreezeType.CATCHFREEZE);
                transform.parent.Find("LabelHolder").gameObject.SetActive(true);
                transform.parent.GetComponent<PointManager>().isTarget = true;
            }
        }
    }
}
