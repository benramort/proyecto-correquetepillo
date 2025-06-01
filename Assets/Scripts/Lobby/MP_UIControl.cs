using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MP_UIControl : MonoBehaviour
{
    [SerializeField]private GameObject selfPanel;
    [SerializeField] private FullGameManager fullGameManager;
    
    public List<Texture> cards;
    public List<GameObject> characters;

    private PlayerInput playerInput;
    private RawImage image;



    public bool isReady { get; set; } = false;
    public GameObject selectedCharacter { get; set; }

    int index = 0;

    // Start is called before the first frame update
    void Start()
    {

        fullGameManager = FullGameManager.INSTANCE;
        playerInput = transform.parent.GetComponent<PlayerInput>();
        Debug.Log("Player index: " + playerInput.playerIndex);
        playerInput.SwitchCurrentActionMap("UI");
        playerInput.actions.Enable();
      
        //selfPanel.GetComponent<Image>().color = Color.blue;
        image = selfPanel.transform.Find("Cardholder").GetComponentInChildren<RawImage>();
        image.texture = cards[0];
        //GameManager.instance.ChangeToGameScene();
    }

    public void MoveLeft(InputAction.CallbackContext context)
    {
        if (isReady) return;
        if (context.performed)
        {
            Debug.Log(index);
            //Debug.Log(--index);
            //Debug.Log(selfPanel.transform.childCount);
            //TextMeshProUGUI tmp = selfPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            //Debug.Log(tmp);
            //tmp.text = index.ToString();
            
            index--;
            if (index < 0)
            {
                index = cards.Count - 1;
            }
            image.texture = cards[index];

        }

    }

    public void MoveRight(InputAction.CallbackContext context)
    {
        if (isReady) return;
        if (context.performed)
        {
            Debug.Log(index);
            Debug.Log(cards.Count);
            index++;
            if (index >=  cards.Count)
            {
                index = 0;
            }
            image.texture = cards[index];
        }

    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            selectedCharacter = characters[index];
            //selfPanel.GetComponent<Image>().color = Color.green;
            isReady = true;
            selfPanel.transform.Find("CheckMark").gameObject.SetActive(true);
            fullGameManager.SelectPlayer(index);
            fullGameManager.GoToGame();
        }
    }

}
