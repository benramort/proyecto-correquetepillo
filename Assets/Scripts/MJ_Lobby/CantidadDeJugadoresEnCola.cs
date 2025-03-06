using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CantidadDeJugadoresEnCola : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI panelCant;
    // Update is called once per frame
    void Update()
    {

        panelCant.text =(NetworkManager.Singleton.ConnectedClients.Count.ToString()+"/4");

    }
}
