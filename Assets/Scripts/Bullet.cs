using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 4.0f;
    public float thrust = 4f;
    public int bulletDamage = 1;
    public ParticleSystem bulletImpactVFX;
    public AudioClip bulletImpactSFX;
    private AudioSource audioSource;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * thrust, ForceMode.Impulse);
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        BulletLifetime();
    }

    public int GetBulletDamage()
    {
        return bulletDamage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player Bullet" || collision.gameObject.tag == "Enemy Bullet")
        {
            ParticleSystem impactVFX = Instantiate(bulletImpactVFX, gameObject.transform.position, Quaternion.identity);
            audioSource.PlayOneShot(bulletImpactSFX);
            StartCoroutine(ImpactVFXLifetime(impactVFX));
            Destroy(gameObject);
        }
    }

    private IEnumerator ImpactVFXLifetime(ParticleSystem impactVFX)
    {
        yield return new WaitForSeconds(impactVFX.main.duration);
        Destroy(impactVFX.gameObject);
    }

    private void BulletLifetime()
    {
        Destroy(this.gameObject, lifetime);
    }
}
