using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace OnlineMultiplayer
{
    public class Catch : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!IsOwner) return;
            //Debug.Log("Colision");
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("Catch");
                if (other.GetComponent<PointManager>().isTarget.Value)
                {
                    //Esto tiene que ser una llamada RCP. Cuál es la mejor manera de hacerlo?
                    //other.GetComponent<OnlineMultiplayer.PointManager>().remoteIsTarget.Value = false;
                    //other.transform.Find("LabelHolder").gameObject.SetActive(false);
                    //other.GetComponent<Movement>().Freeze(FreezeType.CATCHFREEZE);
                    Debug.Log("Haciendo llamada RPC");
                    other.GetComponent<PointManager>().GetCatchedRpc();
                    transform.parent.Find("LabelHolder").gameObject.SetActive(true);
                    transform.parent.GetComponent<PointManager>().isTarget.Value = true;
                }
            }
        }
    }
}
