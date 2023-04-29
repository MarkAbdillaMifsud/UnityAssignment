using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    public Transform[] wayPoints;

    public float speed;

    private int wayPointIndex;

    
    void Start()
    {
        transform.position = wayPoints[wayPointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(wayPointIndex <= wayPoints.Length - 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPoints[wayPointIndex].transform.position, speed * Time.deltaTime);

            if(transform.position == wayPoints[wayPointIndex].transform.position)
            {
                wayPointIndex += 1;
            }
        }

        if(wayPointIndex == wayPoints.Length)
        {
            wayPointIndex = 0;
        }
    }
}
