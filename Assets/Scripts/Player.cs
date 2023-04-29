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
    public Transform bulletSpawn;
    public float bulletSpeed = 8.0f;
    public float fireRate = 0.9f;

    [Header("Life Variables")]
    public int hitPoints = 3;
    public float halfHealth;
    public ParticleSystem damageVFX;
    public ParticleSystem deathVFX;

    private bool canFire = true;

    // Start is called before the first frame update
    void Start()
    {
        halfHealth = hitPoints / 2;
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
        if(hitPoints == halfHealth)
        {
            damageVFX.Play();
        }
        if(hitPoints <= 0)
        {
            GameOver();
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
        GameObject playerBullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        Rigidbody bulletRb = playerBullet.GetComponent<Rigidbody>();
        canFire = false;
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            hitPoints = 0;
        } else if(collision.gameObject.tag == "Enemy Bullet")
        {
            hitPoints--;
        }
    }

    private void GameOver()
    {
        Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
