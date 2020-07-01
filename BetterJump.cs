using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{

    public float fallMultiplayer = 2.5f;
    Rigidbody rb;
    private void Awake()
    {
        // It gets the Rigidbody of this object
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // If the velocity of Y is lower than 0
        if(rb.velocity.y < 0f)
        {
            // It applies gravity
            rb.velocity += Vector3.up * -9.8f * (fallMultiplayer) * Time.deltaTime;
        }
    }
}
