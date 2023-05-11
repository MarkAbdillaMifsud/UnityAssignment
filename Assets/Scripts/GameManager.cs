using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject playerPrefab;
    public Transform startingPosition;
    public float spawnSpeed;
    private bool firstTimePlayerSpawned;

    [Header("Enemies")]
    public GameObject enemySpawner;
    public int maxNumOfEnemies;

    [Header("UI")]
    public TextMeshProUGUI scoreText;

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

    public void AddToScore(int points)
    {
        scoreText.text = "Score " + points;
    }


}
