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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMovement();
        VerticalMovement();
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
}
