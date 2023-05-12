using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject playerPrefab;
    public Transform startingPosition;
    public float spawnSpeed;
    private bool firstTimePlayerSpawned;

    [Header("Enemies")]
    public GameObject enemySpawner;
    [Range(5, 50)]
    public int maxNumOfEnemies;
    private int totalEnemiesSpawned = 0;
    private int totalEnemiesKilled = 0;

    [Header("Game Variables")]
    private int finalPointsEarned;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;

    private void Start()
    {
        firstTimePlayerSpawned = true;
        enemySpawner.SetActive(false);
        SpawnNewPlayer();
    }

    private void SpawnNewPlayer()
    {
        Vector3 screenSpawningPosition = new Vector3(Screen.width / 2, -Screen.height / 10f, 10);
        Vector3 spawningPosition = Camera.main.ScreenToWorldPoint(screenSpawningPosition);
        GameObject player = Instantiate(playerPrefab, spawningPosition, transform.rotation);
        StartCoroutine(MovePlayerToStartingPosition(player.transform));
    }

    private IEnumerator MovePlayerToStartingPosition(Transform player)
    {
        while (Vector3.Distance(startingPosition.position, player.position) > 0.05f)
        {
            player.Translate(Vector3.up * spawnSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        if (firstTimePlayerSpawned)
        {
            firstTimePlayerSpawned = false;
            enemySpawner.SetActive(true);
        }
        player.position = startingPosition.position;
        player.GetComponent<Player>().SetStartingPosition();
    }

    public int GetMaximumEnemyNumber()
    {
        return maxNumOfEnemies;
    }

    public void EnemyKilled(int points)
    {
        totalEnemiesKilled++;
        AddToScore(points);

        float percentageEnemiesKilled = ((float)totalEnemiesKilled / totalEnemiesSpawned) * 100;

        Debug.Log("Percentage killed: " + percentageEnemiesKilled + "%");
    }

    public void IncreaseTotalEnemiesSpawned()
    {
        totalEnemiesSpawned++;
    }

    public void HandlePlayerDeath()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void AddToScore(int points)
    {
        finalPointsEarned += points;
        scoreText.text = "Score " + points;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
