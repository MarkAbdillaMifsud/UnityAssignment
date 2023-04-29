using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Firing")]
    public GameObject enemyBulletPrefab;
    public Transform bulletSpawnPoint;
    public float enemyFireRate;
    public float bulletSpeed = 8f;
    [Range(0.1f, 1.0f)]
    public float fireProbability = 0.5f;
    public float timeBetweenShots = 5f;

    [Header("Enemy Life Variables")]
    public float hitPoints = 3;
    public float halfHealth;
    public ParticleSystem damagedVFX;
    public ParticleSystem deathVFX;

    private bool canFire = true;

    private void Start()
    {
        halfHealth = hitPoints / 2;
    }

    private void Update()
    {
        float r = Random.Range(0.1f, 1.0f);
        if(r < fireProbability && canFire)
        {
            StartCoroutine(EnemyFire());
        }
        if(hitPoints == halfHealth)
        {
            damagedVFX.Play();
        }
        if(hitPoints <= 0)
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private IEnumerator EnemyFire()
    {
        //GameObject firedBullet = Instantiate(enemyBulletPrefab, bulletSpawnPoint);
        GameObject firedBullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
        Rigidbody firedBulletRb = firedBullet.GetComponent<Rigidbody>();
        canFire = false;
        yield return new WaitForSeconds(timeBetweenShots);
        canFire = true;
    }

    private IEnumerator DeathVFXTimer()
    {
        yield return new WaitForSeconds(deathVFX.main.duration);
        Destroy(deathVFX);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player Bullet")
        {
            hitPoints--;
        } else if(collision.gameObject.tag == "Player")
        {
            hitPoints = 0;
        }
    }
}
