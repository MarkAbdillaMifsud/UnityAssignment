using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    [Header("Player")]
    public GameObject playerPrefab;
    public int numOfLives = 3;
    public int maximumNumOfLives = 6;
    public Transform startingPosition;
    public float spawnSpeed;
    private bool firstTimePlayerSpawned;

    [Header("Enemies")]
    public GameObject enemySpawner;
    [Range(5, 50)]
    public int maxNumOfEnemies;
    private int totalEnemiesSpawned = 0;
    private int totalEnemiesKilled = 0;

    [Header("Collectibles")]
    public float collectibleDropChance = 0.5f;

    [Header("Game Variables")]
    public float timeLimit = 60.0f;
    private int finalPointsEarned;
    private bool isTimeRanOut = false;
    private bool isPlayerDefeated = false;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeAmount;
    public TextMeshProUGUI finalScoreText;

    private void Awake()
    {
        if (manager != null)
        {
            Destroy(this.gameObject);
        } else
        {
            manager = this;
        }

        if(GamePersist.Instance == null)
        {
            GameObject persist = new GameObject("GamePersist");
            persist.AddComponent<GamePersist>();
            GamePersist.Instance.FinalLives = numOfLives;
            GamePersist.Instance.FinalPointsEarned = 0;
        }
    }

    private void Start()
    {
        firstTimePlayerSpawned = true;
        enemySpawner.SetActive(false);
        SpawnNewPlayer();
        StartCoroutine(StartTimer());
        scoreText.text = "Score: " + GamePersist.Instance.FinalPointsEarned;
        lifeAmount.text = GamePersist.Instance.FinalLives.ToString();
    }

    private void SpawnNewPlayer()
    {
        Vector3 screenSpawningPosition = new Vector3(Screen.width / 2, -Screen.height / 10f, 10);
        Vector3 spawningPosition = Camera.main.ScreenToWorldPoint(screenSpawningPosition);
        GameObject player = Instantiate(playerPrefab, spawningPosition, transform.rotation);
        StartCoroutine(MovePlayerToStartingPosition(player.transform));
    }

    private IEnumerator StartTimer()
    {
        float timer = 0;

        while(timer < timeLimit)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        isTimeRanOut = true;
        LoadGamerOverScene();
        
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

    public float GetCollectibleDropChance()
    {
        return collectibleDropChance;
    }

    public int GetMaximumLives()
    {
        return maximumNumOfLives;
    }

    public void EnemyKilled(int points)
    {
        totalEnemiesKilled++;
        AddToScore(points);

        float percentageEnemiesKilled = ((float)totalEnemiesKilled / totalEnemiesSpawned) * 100;

        if(totalEnemiesKilled == maxNumOfEnemies)
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalSceneCount = SceneManager.sceneCountInBuildSettings;

        if(currentSceneIndex + 1 < totalSceneCount)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        } else
        {
            Debug.Log("No more scenes to load!");
        }
    }

    public void IncreaseTotalEnemiesSpawned()
    {
        totalEnemiesSpawned++;
    }

    public void IncreaseLife()
    {
        if(GamePersist.Instance.FinalLives < maximumNumOfLives)
        {
            GamePersist.Instance.FinalLives++;
            lifeAmount.text = GamePersist.Instance.FinalLives.ToString();
        }
    }

    public void DecreaseLife()
    {
        if (GamePersist.Instance.FinalLives > 0)
        {
            GamePersist.Instance.FinalLives--;
            lifeAmount.text = GamePersist.Instance.FinalLives.ToString();
        }

        if (GamePersist.Instance.FinalLives <= 0)
        {
            HandlePlayerDeath();
        }
    }

    public void HandlePlayerDeath()
    {
        isPlayerDefeated = true;
        LoadGamerOverScene();
    }

    public void AddToScore(int points)
    {
        GamePersist.Instance.FinalPointsEarned += points;
        scoreText.text = "Score " + GamePersist.Instance.FinalPointsEarned;
        finalScoreText.text = "Your final score is: " + GamePersist.Instance.FinalPointsEarned;
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

    public void LoadGamerOverScene()
    {
        if (isTimeRanOut)
        {
            finalScoreText.text = "Your time ran out. Final score is: " + GamePersist.Instance.FinalPointsEarned;
        } else if (isPlayerDefeated)
        {
            finalScoreText.text = "The enemy overwhelmed you. Final score is: " + GamePersist.Instance.FinalPointsEarned;
        }

        SceneManager.LoadScene("GameOverScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
