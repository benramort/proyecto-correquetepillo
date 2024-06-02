using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    private PlayerInput playerInput;
    private GameObject sharedPanel;
    private GameObject selfPanel;
    private RawImage image;

    private CharacterSelection characterSelection;


    public bool isReady { get; set; } = false;
    public GameObject selectedCharacter { get; set; }

    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        characterSelection = FindObjectOfType<CharacterSelection>();
        characterSelection.AddUIController(this);
        playerInput = transform.parent.GetComponent<PlayerInput>();
        //Debug.Log("Player index: " + playerInput.playerIndex);
        playerInput.SwitchCurrentActionMap("UI");
        sharedPanel = GameObject.Find("CharacterSelectionPanel");
        Debug.Log(playerInput.playerIndex);
        selfPanel = sharedPanel.transform.GetChild(playerInput.playerIndex).gameObject;
        //selfPanel.GetComponent<Image>().color = Color.blue;
        image = selfPanel.transform.Find("Cardholder").GetComponentInChildren<RawImage>();
        image.texture = characterSelection.cards[0];
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
                index = characterSelection.cards.Count - 1;
            }
            image.texture = characterSelection.cards[index];

        }

    }

    public void MoveRight(InputAction.CallbackContext context)
    {
        if (isReady) return;
        if (context.performed)
        {
            Debug.Log(index);
            Debug.Log(characterSelection.cards.Count);
            index++;
            if (index >=  characterSelection.cards.Count)
            {
                index = 0;
            }
            image.texture = characterSelection.cards[index];
        }

    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            selectedCharacter = characterSelection.characters[index];
            //selfPanel.GetComponent<Image>().color = Color.green;
            isReady = true;
            selfPanel.transform.Find("CheckMark").gameObject.SetActive(true);
            characterSelection.NotifyReady();
        }
    }

}
