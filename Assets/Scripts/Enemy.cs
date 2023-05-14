using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Firing")]
    public GameObject enemyBulletPrefab;
    public Transform bulletSpawnPoint;
    [Range(0.1f, 1.0f)]
    public float enemyFireRate = 0.9f;
    public float timeBetweenShots = 5f;
    public AudioClip bulletFiredSFX;
    private Bullet bullet;
    private float nextFireTime = 0;

    [Header("Enemy Game Variables")]
    public float hitPoints = 3;
    public int enemyScore;
    public float enemyDeathDelay = 3.0f;
    public ParticleSystem damagedVFX;
    public ParticleSystem deathVFX;
    public AudioClip deathSFX;
    private float halfHealth;
    private bool isEnteredDeathAnimation = false;
    private Rigidbody enemyRb;
    private Collider enemyCollider;
    private AudioSource audioSource;

    [Header("Collectible Variables")]
    public GameObject[] collectibleTypes;
    private Collectible collectible;

    private bool canFire = true;
    private GameManager gameManager;

    private void Start()
    {
        halfHealth = hitPoints / 2;
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        bullet = enemyBulletPrefab.GetComponent<Bullet>();
        collectible = FindObjectOfType<Collectible>();
        enemyRb = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isEnteredDeathAnimation)
        {
            float r = Random.Range(0.1f, 1.0f);
            if (Time.time >= nextFireTime && canFire)
            {
                StartCoroutine(EnemyFire());
                nextFireTime = Time.time + 1 / enemyFireRate; //calculate time interval between shots
            }
        }
    }

    private IEnumerator EnemyFire()
    {
        GameObject firedBullet = Instantiate(enemyBulletPrefab, bulletSpawnPoint);
        Rigidbody firedBulletRb = firedBullet.GetComponent<Rigidbody>();
        audioSource.PlayOneShot(bulletFiredSFX);
        canFire = false;
        yield return new WaitForSeconds(timeBetweenShots);
        canFire = true;
    }

    private void TakeDamage(int damageAmount)
    {
        hitPoints -= damageAmount;

        if(hitPoints <= halfHealth)
        {
            damagedVFX.Play();
        }

        if(hitPoints <= 0)
        {
            EnemyDied();
        }
    }

    private void EnemyDied()
    {
        DropCollectible();
        isEnteredDeathAnimation = true;
        enemyRb.AddForce(Vector3.forward * 10, ForceMode.VelocityChange);
        enemyCollider.enabled = false;
        GameObject coroutineManager = new GameObject("CoroutineManager");
        coroutineManager.AddComponent<CoroutineManager>().StartCoroutine(ManageEnemyDeath(coroutineManager, enemyDeathDelay));
    }

    private IEnumerator ManageEnemyDeath(GameObject coroutineManager, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(deathSFX);
        ParticleSystem explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
        gameManager.EnemyKilled(enemyScore);
        float explosionDuration = explosion.main.duration;
        yield return new WaitForSeconds(explosionDuration);
        Destroy(explosion.gameObject);
        Destroy(this.gameObject);
        Destroy(coroutineManager);
    }

    private void DropCollectible()
    {
        float dropChance = Random.Range(0f, 1f);
        if(dropChance <= gameManager.GetCollectibleDropChance())
        {
            int collectibleIndex = Random.Range(0, collectibleTypes.Length);
            Instantiate(collectibleTypes[collectibleIndex], transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player Bullet")
        {
            TakeDamage(bullet.GetBulletDamage());
        } else if(collision.gameObject.tag == "Player")
        {
            TakeDamage((int)hitPoints);
        }
    }
}
