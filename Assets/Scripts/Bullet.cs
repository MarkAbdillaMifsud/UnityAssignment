using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 4.0f;
    public float thrust = 4f;

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
            Destroy(this.gameObject);
        }
    }

    private void BulletLifetime()
    {
        Destroy(this.gameObject, lifetime);
    }
}
