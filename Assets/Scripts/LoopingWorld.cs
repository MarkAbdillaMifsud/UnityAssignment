using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingWorld : MonoBehaviour
{
    public float speed = 4.0f;
    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }


    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if(transform.position.y < -10f)
        {
            transform.position = startPosition;
        }
    }
}
