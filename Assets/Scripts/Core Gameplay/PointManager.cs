using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{

    [HideInInspector] public GameManager gameManager { get; set;}

    public int maxPoints = 3;
    public float tickTime; //Time to reduce 1 point in seconds
    public GameObject catchArea;
    public GameObject labelHolder;
    public Animator animator;

    [SerializeField] private bool _isTarget;
    public bool isTarget { get {return _isTarget;} set { _isTarget = value; labelHolder.SetActive(value); } }
    public int points { get; set; }
    private float timePassed;
    private Coroutine catchCoroutine;
    private TextMeshProUGUI pointsText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Scripter").GetComponent<GameManager>();
        pointsText = transform.parent.parent.Find("Interface(Clone)").GetComponentInChildren<TextMeshProUGUI>();
        points = maxPoints;
        animator = GetComponentInChildren<Animator>();
        
        //isTarget = true;
    }

    // Update is called once per frame
    void Update()
    {
        ReducePoints();
        pointsText.text = points.ToString();
    }

    private void ReducePoints()
    {
        if (isTarget)
        {
            if (timePassed >= tickTime)
            {
                timePassed = 0;
                if (points >= 1)
                {
                    points--;
                }
            }
            else
            {
                timePassed += Time.deltaTime;
            }
            

        }
        if (points <= 0)
        {
            gameManager.EndGame(gameObject);
        }
    }

    public void Catch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
            if (catchCoroutine == null)
            {
                animator.SetTrigger("attack");
                catchCoroutine = StartCoroutine(CatchCoroutine());
            } else
            {
                Debug.Log("Alredy catching");
            }
            
        }
    }

    IEnumerator CatchCoroutine()
    {
        catchArea.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        catchArea.SetActive(false);
        yield return new WaitForSeconds(0.01f);
        catchCoroutine = null;
    }
}
