using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3.0f;
    public float maxXOffset = 8.0f;
    public float maxYOffset = 8.0f;
    public float minYOffset = -8.0f; //vertical not a mirror like X, therefore include a separate value for min Y offset

    [Header("Projectile")]
    public GameObject bulletPrefab;
    public Transform[] bulletSpawnPoints;
    public float bulletSpeed = 8.0f;
    public float fireRate = 0.9f;
    private int bulletSpawnIndex = 0;

    [Header("Life Variables")]
    public int lives = 3;
    public int hitPoints = 3;
    public ParticleSystem damageVFX;
    public ParticleSystem deathVFX;
    public Transform respawnPath;

    private int currentHitPoints;
    private float halfHealth;
    private bool canFire = true;
    private bool playerIsDead = false;
    private bool isRespawning = false;
    private Rigidbody playerRb;
    private float originalY;
    private bool hasReachedStartingPos;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        halfHealth = hitPoints / 2;
        currentHitPoints = hitPoints;
        hasReachedStartingPos = false;
    }
    public void SetStartingPosition()
    {
        originalY = transform.position.y;
        hasReachedStartingPos = true;
    }

    // Update is called once per frame
    void Update()
    {
            HorizontalMovement();
            VerticalMovement();
            if (Input.GetButtonDown("Fire1") && canFire == true)
            {
                StartCoroutine(Shoot());
            }
        if (currentHitPoints == halfHealth)
        {
            damageVFX.Play();
        }
        if (currentHitPoints <= 0 && !playerIsDead)
        {
            Respawn();
        }
    }

    private void HorizontalMovement()
    {
        float horMovement = Input.GetAxis("Horizontal");
        Vector3 newPos = transform.position + Vector3.right * horMovement * speed * Time.deltaTime;
        newPos.x = Mathf.Clamp(newPos.x, -maxXOffset, maxXOffset);

        transform.position = newPos;
    }

    private void VerticalMovement()
    {
        float verMovement = Input.GetAxis("Vertical");
        Vector3 newPos = transform.position + Vector3.up * verMovement * speed * Time.deltaTime;
        newPos.y = Mathf.Clamp(newPos.y, minYOffset, maxYOffset);

        transform.position = newPos;
    }

    private IEnumerator Shoot()
    {
        GameObject playerBullet = Instantiate(bulletPrefab, bulletSpawnPoints[bulletSpawnIndex].position, Quaternion.identity);
        Rigidbody bulletRb = playerBullet.GetComponent<Rigidbody>();
        canFire = false;
        bulletSpawnIndex = (bulletSpawnIndex + 1) % bulletSpawnPoints.Length; // Increment the spawn point index, or reset it if we've reached the end of the array
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            currentHitPoints = 0;
        } else if(collision.gameObject.tag == "Enemy Bullet")
        {
            currentHitPoints--;
        }
    }

    private void Respawn()
    {
        if(!isRespawning)
        {
            StartCoroutine(RespawnProcess());
        }
    }

    private IEnumerator RespawnProcess()
    {
        isRespawning = true;
        lives--;
        deathVFX.Emit(100);
        if(lives <= 0)
        {
            playerIsDead = true;
        } else
        {
            transform.position = respawnPath.GetChild(0).transform.position;
            while(Vector3.Distance(transform.position, respawnPath.GetChild(1).transform.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, respawnPath.GetChild(1).transform.position, speed * Time.deltaTime);
                yield return null;
            }
            playerIsDead = false;
        }
        isRespawning = false;
    }
}
