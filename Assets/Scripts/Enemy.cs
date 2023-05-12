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
    private Bullet bullet;
    private float nextFireTime = 0;

    [Header("Enemy Game Variables")]
    public float hitPoints = 3;
    public int enemyScore;
    public ParticleSystem damagedVFX;
    public ParticleSystem deathVFX;
    private float halfHealth;

    private bool canFire = true;
    private GameManager gameManager;

    private void Start()
    {
        halfHealth = hitPoints / 2;
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        bullet = enemyBulletPrefab.GetComponent<Bullet>();
    }

    private void Update()
    {
        float r = Random.Range(0.1f, 1.0f);
        if(Time.time >= nextFireTime && canFire)
        {
            StartCoroutine(EnemyFire());
            nextFireTime = Time.time + 1 / enemyFireRate; //calculate time interval between shots
        }
    }

    private IEnumerator EnemyFire()
    {
        GameObject firedBullet = Instantiate(enemyBulletPrefab, bulletSpawnPoint);
        Rigidbody firedBulletRb = firedBullet.GetComponent<Rigidbody>();
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
        ParticleSystem explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
        StartCoroutine(ManageEnemyDeath(explosion));
        gameManager.EnemyKilled(enemyScore);
        Destroy(this.gameObject);
    }

    private IEnumerator ManageEnemyDeath(ParticleSystem explosion)
    {
        yield return new WaitForSeconds(explosion.main.duration);
        Destroy(explosion.gameObject);
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
