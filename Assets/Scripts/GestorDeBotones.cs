using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GestorDeBotones : MonoBehaviour
{
    [SerializeField] private GameObject firstButton;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    // Update is called once per frame

}
