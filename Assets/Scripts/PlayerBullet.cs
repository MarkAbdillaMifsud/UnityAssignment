using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float lifetime = 4.0f;
    private void Update()
    {
        BulletLifetime();
    }

    private void BulletLifetime()
    {
        Destroy(this.gameObject, lifetime);
    }
}
