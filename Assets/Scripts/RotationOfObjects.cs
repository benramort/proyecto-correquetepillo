using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationOfObjects : MonoBehaviour
{
    [SerializeField]
    private int rotationY = 1;
    [SerializeField]
    private float speed = 150;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkRotate();
    }

    private void checkRotate()
    {
        gameObject.transform.rotation *= Quaternion.Euler(0, rotationY * Time.deltaTime*speed, 0);
    }
}
