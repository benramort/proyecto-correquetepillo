using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointManager : MonoBehaviour
{

    [HideInInspector] public GameManager gameManager { get; set;}

    public int maxPoints = 100;
    public float tickTime; //Time to reduce 1 point in seconds
    public GameObject catchArea;
    public GameObject labelHolder;
    public Animator animator;

    [SerializeField] private bool _isTarget;
    public bool isTarget { get {return _isTarget;} set { _isTarget = value; labelHolder.SetActive(value); } }
    [SerializeField] private int points;
    private float timePassed;
    private Coroutine catchCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        points = maxPoints;
        //isTarget = true;
    }

    // Update is called once per frame
    void Update()
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
