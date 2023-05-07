using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnDelay = 1.0f;
    public float spawnRadius = 10.0f;
    public Transform[] wayPoints;
    public float speed = 5.0f;
    
    private Camera gameCamera;
    private float timeSinceLastSpawn = 0.0f;

    private void Start()
    {
        gameCamera = Camera.main;
    }

    void Update()
    {
        // Increment timeSinceLastSpawn by the amount of time since the last frame
        timeSinceLastSpawn += Time.deltaTime;

        // If enough time has passed, spawn a new enemy
        if (timeSinceLastSpawn >= spawnDelay)
        {
            // Reset timeSinceLastSpawn
            timeSinceLastSpawn = 0.0f;

            // Choose a random position within spawnRadius of the camera
            Vector3 cameraPos = gameCamera.transform.position;
            Vector3 spawnPos = GetOffscreenSpawnPosition(cameraPos);

            // Spawn a new enemy at the chosen position
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            EnemyMovement(enemy.transform);
        }
    }

    void EnemyMovement(Transform enemyTransform)
    {
        if(enemyTransform != null)
        {
            StartCoroutine(FollowWaypoints(enemyTransform));
        }
    }

    IEnumerator FollowWaypoints(Transform enemyTransform)
    {
        int currentWaypointIndex = 0;

        while(currentWaypointIndex < wayPoints.Length && enemyTransform != null)
        {
            Vector3 currentWaypoint = wayPoints[currentWaypointIndex].position;
            Vector3 direction = (currentWaypoint - enemyTransform.position).normalized;
            enemyTransform.position += direction * speed * Time.deltaTime;
            if(Vector3.Distance(enemyTransform.position, currentWaypoint) < 0.1f)
            {
                currentWaypointIndex++;
            }

            yield return null;
        }

        if(enemyTransform != null)
        {
            Destroy(enemyTransform.gameObject);
        }
    }

    Vector3 GetOffscreenSpawnPosition(Vector3 cameraPos)
    {
        float cameraHeight = 2.0f * gameCamera.orthographicSize;
        float cameraWidth = cameraHeight * gameCamera.aspect;
        float spawnOffset = Mathf.Max(cameraWidth, cameraHeight) + spawnRadius;

        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnOffset;
        Vector3 spawnPos = new Vector3(randomCircle.x, cameraPos.y + spawnOffset, 0f);

        return spawnPos;
    }
}
