using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float timeBetweenEnemies = 4.0f;
    public float spawnRadius = 10.0f;

    public GameObject[] enemyWaves;
    public float speed = 5.0f;
    
    private Camera gameCamera;
    private Transform[] wayPoints;
    private float timeSinceLastSpawn = 0.0f;
    private int numEnemiesSpawned;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        numEnemiesSpawned = 0;
        gameCamera = Camera.main;
        if(enemyWaves != null && enemyWaves.Length > 0)
        {
           int randomWaveIndex = Random.Range(0, enemyWaves.Length);
            Transform waveTransform = enemyWaves[randomWaveIndex].transform;
            wayPoints = new Transform[waveTransform.childCount];
            for (int i = 0; i < waveTransform.childCount; i++)
            {
                wayPoints[i] = waveTransform.GetChild(i);
            }
        } else
        {
            Debug.LogError("Enemy Waves is not assigned in EnemySpawner, or it is empty!");
        }
    }

    void Update()
    {
        // Increment timeSinceLastSpawn by the amount of time since the last frame
        timeSinceLastSpawn += Time.deltaTime;

        // If enough time has passed, spawn a new enemy
        if (timeSinceLastSpawn >= timeBetweenEnemies && numEnemiesSpawned < gameManager.GetMaximumEnemyNumber())
        {
            // Reset timeSinceLastSpawn
            timeSinceLastSpawn = 0.0f;

            // Choose a random position within spawnRadius of the camera
            Vector3 cameraPos = gameCamera.transform.position;
            Vector3 spawnPos = GetOffscreenSpawnPosition(cameraPos);

            int r = Random.Range(0, enemyPrefabs.Length);
            GameObject enemy = Instantiate(enemyPrefabs[r], spawnPos, Quaternion.identity);
            gameManager.IncreaseTotalEnemiesSpawned();
            EnemyMovement(enemy.transform);
            numEnemiesSpawned++;
            
            if (numEnemiesSpawned % 5 == 0)
            {
                int randomWaveIndex = Random.Range(0, enemyWaves.Length);
                Transform waveTransform = enemyWaves[randomWaveIndex].transform;
                wayPoints = new Transform[waveTransform.childCount];
                for (int i = 0; i < waveTransform.childCount; i++)
                {
                    wayPoints[i] = waveTransform.GetChild(i);
                }
            }
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

        while (enemyTransform != null)
        {
            Vector3 currentWaypoint = wayPoints[currentWaypointIndex].position;
            Vector3 direction = (currentWaypoint - enemyTransform.position).normalized;
            enemyTransform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(enemyTransform.position, currentWaypoint) < 0.1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % wayPoints.Length;
            }

            yield return null;
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
