using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] wayPoints;
    public GameObject[] enemyTypes;
    public int enemyAmount = 5;
    public float timeBetweenEnemies = 2f;

    public float speed = 3.0f;

    private int wayPointIndex;

    
    void Start()
    {
        transform.position = wayPoints[wayPointIndex].transform.position;
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
     
    }

    private IEnumerator FollowPath(GameObject spawnedEnemy)
    {
        Transform[] childWayPoints = wayPoints[0].GetComponentsInChildren<Transform>();
        wayPointIndex = 0;

        while (wayPointIndex < childWayPoints.Length)
        {
            if(Object.ReferenceEquals(spawnedEnemy, null)) //runs only if the enemy is destroyed and avoids MissingReferenceException
            {
                yield break;
            }

            Vector3 targetPosition = childWayPoints[wayPointIndex].position;
            spawnedEnemy.transform.position = Vector3.MoveTowards(spawnedEnemy.transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(spawnedEnemy.transform.position, targetPosition) < 0.1f)
            {
                Debug.Log("Reached waypoint " + wayPointIndex);
                wayPointIndex++;

                if (wayPointIndex >= childWayPoints.Length)
                {
                    wayPointIndex = 0;
                }
            }

            yield return null;
        }

        Debug.Log("Finished path");
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemyAmount; i++)
        {
            GameObject spawnedEnemy = Instantiate(enemyTypes[0], wayPoints[0].transform.GetChild(0).transform.position, enemyTypes[0].transform.rotation);
            yield return StartCoroutine(FollowPath(spawnedEnemy));
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }
}
