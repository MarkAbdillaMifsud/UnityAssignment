using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 4.0f;
    public float thrust = 4f;
    public ParticleSystem missileImpactVFX;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * thrust, ForceMode.Impulse);
    }
    private void Update()
    {
        BulletLifetime();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player Bullet" || collision.gameObject.tag == "Enemy Bullet")
        {
            ParticleSystem impactVFX = Instantiate(missileImpactVFX, gameObject.transform.position, Quaternion.identity);
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
