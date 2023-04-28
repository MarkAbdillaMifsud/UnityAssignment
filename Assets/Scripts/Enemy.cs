using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Firing")]
    public GameObject enemyBulletPrefab;
    public float enemyFireRate;
    public float bulletSpeed = 8f;
    [Range(0.1f, 1.0f)]
    public float fireProbability = 0.5f;
    public float timeBetweenShots = 5f;

    [Header("Enemy Game Values")]
    public int hitPoints = 3;

    private bool canFire = true;

    private void Start()
    {

    }

    private void Update()
    {
        float r = Random.Range(0.1f, 1.0f);
        if(r < fireProbability && canFire)
        {
            StartCoroutine(EnemyFire());
        }
        if(hitPoints <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator EnemyFire()
    {
        GameObject firedBullet = Instantiate(enemyBulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
        Rigidbody firedBulletRb = firedBullet.GetComponent<Rigidbody>();
        firedBulletRb.velocity = Vector3.down * bulletSpeed;
        canFire = false;
        yield return new WaitForSeconds(timeBetweenShots);
        canFire = true;
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
