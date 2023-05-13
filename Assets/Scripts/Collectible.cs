using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Type of Collectible")]
    public bool isHealth = false;
    public bool isLife = false;
    public bool isShield = false;
    public bool isMissile = false;

    [Header("Collectible Variables")]
    public float forceOfCollectible = 5f;

    private Rigidbody collectibleRb;

    private void Start()
    {
        DetermineCollectibleType();
        collectibleRb = GetComponent<Rigidbody>();
        collectibleRb.AddForce(Vector3.down * forceOfCollectible);
    }

    public string DetermineCollectibleType()
    {
        if (isHealth)
        {
            isLife = false;
            isShield = false;
            isMissile = false;
            return "isHealth";
        }
        else if (isLife)
        {
            isHealth = false;
            isShield = false;
            isMissile = false;
            return "isLife";
        }
        else if (isShield)
        {
            isHealth = false;
            isLife = false;
            isMissile = false;
            return "isShield";
        }
        else if (isMissile)
        {
            isHealth = false;
            isLife = false;
            isShield = false;
            return "isMissile";
        }

        return "None";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
